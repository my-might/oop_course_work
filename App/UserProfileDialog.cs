using Terminal.Gui;
using System;
using ClassLib;
using System.Collections.Generic;

namespace App
{
    public class UserProfileDialog : Dialog
    {
        private TextField login;
        private TextField fullname;
        private TextField age;
        private TextField email;
        private TextField driverLicenseNum;
        private TextField category;
        private TextField password;
        private Button showPassword;
        private CheckBox vip;
        private ListView rents;
        private UserState user;
        private Service service;
        public UserProfileDialog()
        {
            this.Title = "My profile";
            Button ok = new Button("OK");
            ok.Clicked += DialogCanceled;
            this.AddButton(ok);

            Label loginLabel = new Label(2, 1, "Login:");
            login = new TextField("")
            {
                X = 25, Y = Pos.Top(loginLabel), Width = Dim.Percent(40),
                ReadOnly = true
            };
            this.Add(loginLabel, login);

            Label passwordLabel = new Label(2, 2, "Password:");
            password = new TextField("")
            {
                X = 25, Y = Pos.Top(passwordLabel), Width = Dim.Percent(40),
                ReadOnly = true, Secret = true
            };
            this.Add(passwordLabel, password);


            Label fullnameLabel = new Label(2, 3, "Fullname:");
            fullname = new TextField("")
            {
                X = 25, Y = Pos.Top(fullnameLabel), Width = Dim.Percent(40),
                ReadOnly = true
            };
            this.Add(fullnameLabel, fullname);

            Label ageLabel = new Label(2, 4, "Age:");
            age = new TextField("")
            {
                X = 25, Y = Pos.Top(ageLabel), Width = Dim.Percent(40),
                ReadOnly = true
            };
            this.Add(ageLabel, age);

            Label emailLabel = new Label(2, 5, "Email:");
            email = new TextField("")
            {
                X = 25, Y = Pos.Top(emailLabel), Width = Dim.Percent(45),
                ReadOnly = true
            };
            this.Add(emailLabel, email);

            Label licenseLabel = new Label(2, 6, "Driver license number:");
            driverLicenseNum = new TextField("")
            {
                X = 25, Y = Pos.Top(licenseLabel), Width = Dim.Percent(40),
                ReadOnly = true
            };
            this.Add(licenseLabel, driverLicenseNum);

            Label categoryLabel = new Label(2, 7, "Category:");
            category = new TextField("")
            {
                X = 25, Y = Pos.Top(categoryLabel), Width = Dim.Percent(40),
                ReadOnly = true
            };
            this.Add(categoryLabel, category);

            Label vipLabel = new Label(2, 8, "Vip:");
            vip = new CheckBox()
            {
                X = 25, Y = Pos.Top(vipLabel), Width = Dim.Percent(40)
            };
            this.Add(vipLabel, vip);

            Label rentsLabel = new Label(2, 10, "My rents");
            rents = new ListView()
            {
                Width = Dim.Fill(), Height = Dim.Fill()
            };
            rents.OpenSelectedItem += OnOpenRent;

            FrameView frameView = new FrameView("")
            {
                X = 25, Y = Pos.Top(rentsLabel),
                Width = Dim.Percent(50),
                Height = 5
            };
            frameView.Add(rents);
            this.Add(rentsLabel, frameView);
        }
        private void ClickShowPassword() 
        {
            password.Secret = false;
        }
        public void SetInfo(UserState user, Service service)
        {
            this.service = service;
            this.user = user;
            this.fullname.Text = user.User.fullname;
            this.login.Text = user.User.login;
            this.password.Text = user.User.password;
            this.age.Text = user.User.age.ToString();
            this.email.Text = user.User.email;
            this.driverLicenseNum.Text = user.User.driver_license_num;
            this.category.Text = user.User.category;
            this.vip.Checked = user.User.vip;
            SetRents();
        }
        public void SetRents()
        {
            List<Rent> rents = service.rentProxy.GetByUserId(user.User.id);
            if(rents.Count == 0)
            {
                List<string> emptyText = new List<string>();
                emptyText.Add("There are no rents.");
                this.rents.SetSource(emptyText);
            }
            else
            {
                this.rents.SetSource(rents);
            }
        }
        private void OnOpenRent(ListViewItemEventArgs args)
        {
            Rent currentRent = new Rent();
            try
            {
                currentRent = (Rent)args.Value;
                currentRent.car = service.carProxy.GetById(currentRent.car_id);
                currentRent.client = service.userProxy.GetById(currentRent.client_id);
            }
            catch
            {
                return;
            }
            ShowRentDialog dialog = new ShowRentDialog();
            dialog.SetInfo(currentRent, service, user);
            Application.Run(dialog);
            if(dialog.deleted)
            {
                SetRents();
            }
        }
        private void DialogCanceled()
        {
            Application.RequestStop();
        }
    }
}