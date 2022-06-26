using ClassLib;
using System;
using Terminal.Gui;
using System.Linq;

namespace App
{
    public class UserState
    {
        private User user;
        private State state;
        private Service service;
        public User User
        {
            get { return user; }
            set { user = value; }
        }
        public State State
        {
            get { return state; }
            set { state = value; }
        }
        public UserState(Service service) 
        {
            this.state = new UnauthorizedState(service, this);
            this.service = service;
        }
        public void SignIn() 
        {
            User userSigned = state.SignIn();
            if(userSigned != null) {
                this.user = userSigned;
            }
        }
        public void SignUp() 
        {
            User userSigned = state.SignUp();
            if(userSigned != null) {
                this.user = userSigned;
            }
        }
        public void LogOut() 
        {
            state.LogOut();
            user = null;
        }     
        public void RentCar(Car car)
        {
            state.RentCar(car);
        }
    }

    public abstract class State 
    {
        protected UserState user;
        protected Service service;
        public UserState User
        {
            get { return user; }
            set { user = value; }
        }
        public Service Service
        {
            get { return service; }
            set { service = value; }
        }
        public abstract User SignIn();
        public abstract User SignUp();
        public abstract void LogOut();
        public abstract void RentCar(Car car);
        
    }

    public class AuthorizedState : State
    {
        public AuthorizedState(State state) : this(state.Service, state.User) {}
        public AuthorizedState(Service service, UserState user)
        {
            this.service = service;
            this.user = user;
        }
        public override User SignIn() 
        {
            MessageBox.ErrorQuery("Error", "You are already signed in!",  "OK");
            return null;
        }
        public override User SignUp() 
        {
            MessageBox.ErrorQuery("Error", "You are already signed up!",  "OK");
            return null;
        }
        public override void LogOut()
        {
            user.State = new UnauthorizedState(this);
        }
        public override void RentCar(Car car)
        {
            RentDatesDialog datesDialog = new RentDatesDialog();
            datesDialog.SetInfo(service, car.id, user.User);
            Application.Run(datesDialog);
            if(datesDialog.canceled)
            {
                return;
            }
            Rent rent = datesDialog.GetRent();
            RentCar rentTemplate;
            if(user.User.vip) { rentTemplate = new VipRentCar(); }
            else { rentTemplate = new OrdinaryRentCar(); }
            RentData result = rentTemplate.Renting(user.User, car, ((int)(rent.to_date.Date-rent.from_date.Date).TotalDays));
            if(!result.allowed)
            {
                MessageBox.ErrorQuery("Error", "You are not allowed to rent this car!",  "OK");
                return;
            }
            if(!result.confirmed)
            {
                MessageBox.ErrorQuery("Error", "You haven`t confirmed your rent!",  "OK");
                return;
            }
            rent.action_time = DateTime.Now;
            rent.client_id = user.User.id;
            try
            {
                service.rentProxy.Insert(rent);
            }
            catch(Exception ex)
            {
                MessageBox.ErrorQuery("Error", $"{ex.Message}",  "OK");
            }
            result.rent = rent;
            ReceiptDialog receiptDialog = new ReceiptDialog();
            receiptDialog.SetInfo(result, car, user);
            Application.Run(receiptDialog);

            if(!user.User.vip && service.rentProxy.GetByUserId(user.User.id).Where(u => u.is_deleted != true).ToList().Count > 3)
            {
                user.User.vip = true;
                service.userProxy.Update(user.User);
                MessageBox.Query("Congratulations!", "You were successfully promoted to vip user!", "Thanks!");
            }
        }
    }

    public class UnauthorizedState : State 
    {
        public UnauthorizedState(State state) : this(state.Service, state.User) {}
        public UnauthorizedState(Service service, UserState user)
        {
            this.service = service;
            this.user = user;
        }
        public override User SignIn() 
        {
            LoginDialog dialog = new LoginDialog();
            dialog.SetRepository(service.userProxy);
            Application.Run(dialog);
            if(!dialog.canceled)
            {
                user.State = new AuthorizedState(this);
                return dialog.GetClient();
            }
            return null;
        }
        public override User SignUp()
        {
            RegistrationDialog dialog = new RegistrationDialog();
            dialog.SetRepository(service.userProxy);
            Application.Run(dialog);
            if(!dialog.canceled)
            {
                user.State = new AuthorizedState(this);
                return dialog.GetClient();
            }
            return null;
        }
        public override void LogOut()
        {
            MessageBox.ErrorQuery("Error", "You are not signed in!",  "OK");
        }
        public override void RentCar(Car car)
        {
            int result = MessageBox.ErrorQuery("Error", "You have to sign up first!",  "OK", "Sign in", "Sign up");
            if(result == 1) {user.SignIn();}
            else if (result == 2) {user.SignUp();}
        }
    }
}