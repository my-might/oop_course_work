using Terminal.Gui;
using ClassLib;

namespace App
{
    public class MainWindowUser : Window
    {
        private Service service;
        private UserState user;
        private Button promoteUser;
        private Button createCar;
        private Label loggedUser;
        private Button logOut;
        private Button profile;
        public MainWindowUser()
        {
            this.Title = "Car rental salon";
            this.X = 0;
            this.Y = 1;

            Label hello = new Label("We are happy to see you in our car rental salon!")
            {
                X = Pos.Center(), Y = 5
            };
            Label available = new Label("There are available options for you:")
            {
                X = Pos.Center(), Y = 6
            };
            this.Add(hello, available);

            profile = new Button("My profile")
            {
                X = Pos.Center() - 20, Y = 9
            };
            profile.Clicked += ClickShowProfile;
            Button viewCars = new Button("Search for cars")
            {
                X = Pos.Center(), Y = 10
            };
            viewCars.Clicked += ClickShowCars;

            createCar = new Button("Create car")
            {
                X = Pos.Center(), Y = 12
            };
            createCar.Clicked += ClickCreateCar;
        
            promoteUser = new Button("Promote user")
            {
                X = Pos.Center(), Y = 14
            };
            promoteUser.Clicked += ClickPromoteUser;

            this.Add(profile, viewCars, createCar, promoteUser);

            loggedUser = new Label(" ")
            {
                X = Pos.Percent(2), Y = Pos.Percent(2),
                Width = Dim.Fill() - 20
            };
            
            logOut = new Button("Log out")
            {
                X = Pos.Percent(85), Y = Pos.Y(loggedUser)
            };
            logOut.Clicked += OnLogOut;
            this.Add(loggedUser, logOut);

        }
        public void SetUser(UserState user)
        {
            this.user = user;
            if(user.User == null)
            {
                profile.Visible = false;
                logOut.Visible = false;
                createCar.Visible = false;
                promoteUser.Visible = false;
            }
            else if(!user.User.is_worker)
            {
                loggedUser.Text = "You are successfully logged as user.";
                createCar.Visible = false;
                promoteUser.Visible = false;
            }
            else
            {
                loggedUser.Text = "You are successfully logged as worker.";
            }
        }
        private void OnLogOut()
        {
            user.LogOut();
        }
        private void ClickPromoteUser()
        {
            PromoteUserDialog dialog = new PromoteUserDialog();
            dialog.SetService(service);
            Application.Run(dialog);
        }
        private void ClickCreateCar()
        {
            CreateCarDialog dialog = new CreateCarDialog();
            dialog.SetRepository(service);
            Application.Run(dialog);
            if(!dialog.canceled)
            {
                ShowCarDialog carDialog = new ShowCarDialog();
                carDialog.SetInfo(dialog.GetCar(), service, user);
                Application.Run(carDialog);
            }
        }
        private void ClickShowProfile()
        {
            UserProfileDialog dialog = new UserProfileDialog();
            dialog.SetInfo(user, service);
            Application.Run(dialog);
        }
        private void ClickShowCars()
        {
            CarParametersDialog parametersDialog = new CarParametersDialog();
            parametersDialog.SetRepository(service);
            Application.Run(parametersDialog);
            ShowAllCarsDialog dialog = new ShowAllCarsDialog();
            dialog.SetInfo(service, user, parametersDialog.GetParams());
            Application.Run(dialog);
        }
        public void SetService(Service service)
        {
            this.service = service;
        }
    }
}