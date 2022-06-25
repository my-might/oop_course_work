using ClassLib;
using System;
using Terminal.Gui;

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
            rent.car_id = car.id;
            rent.client_id = user.User.id;
            try
            {
                service.rentProxy.Insert(rent);
            }
            catch(Exception ex)
            {
                MessageBox.ErrorQuery("Error", $"{ex.Message}",  "OK");
            }
            ReceiptDialog receiptDialog = new ReceiptDialog();
            receiptDialog.SetInfo(result, car);
            Application.Run(receiptDialog);
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
        public override User SignIn() {
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