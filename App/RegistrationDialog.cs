using Terminal.Gui;
using ClassLib;
using System;

namespace App
{
    public class RegistrationDialog : Dialog
    {
        public bool canceled;
        private TextField age;
        private TextField fullname;
        private TextField password;
        private TextField email;
        private TextField drLicenseNum;
        private TextField categories;
        private TextField login;
        private Client clientToAdd;
        private ClientProxy clientProxy;
        public RegistrationDialog()
        {
            this.Title = "Registration";
            this.Height = 23;
            this.Width = 50;
            Button ok = new Button("OK");
            ok.Clicked += DialogSubmit;
            Button cancel = new Button("Cancel"); 
            cancel.Clicked += DialogCanceled;
            this.AddButton(ok);
            this.AddButton(cancel);

            Label info = new Label("Sign up")
            {
                X = Pos.Center(), Y = 1
            };
            this.Add(info);

            Label fullnameLabel = new Label("Fullname:")
            {
                X = Pos.Center(), Y = 3
            };
            fullname = new TextField("")
            {
                X = Pos.Center(), Y = Pos.Bottom(fullname), Width = Dim.Percent(50)
            };
            this.Add(fullnameLabel, fullname);

            Label ageLabel = new Label("Age:")
            {
                X = Pos.Center(), Y = 5
            };
            age = new TextField("")
            {
                X = Pos.Center(), Y = Pos.Bottom(ageLabel), Width = Dim.Percent(50)
            };
            this.Add(ageLabel, age);

            Label drLicenseLabel = new Label("Number of driver license:")
            {
                X = Pos.Center(), Y = 7
            };
            drLicenseNum = new TextField("")
            {
                X = Pos.Center(), Y = Pos.Bottom(drLicenseLabel), Width = Dim.Percent(50)
            };
            this.Add(drLicenseLabel, drLicenseNum);

            Label categoriesLabel = new Label("Categories:")
            {
                X = Pos.Center(), Y = 9
            };
            categories = new TextField("")
            {
                X = Pos.Center(), Y = Pos.Bottom(categoriesLabel), Width = Dim.Percent(50)
            };
            this.Add(categoriesLabel, categories);

            Label loginLabel = new Label("Login:")
            {
                X = Pos.Center(), Y = 11
            };
            login = new TextField("")
            {
                X = Pos.Center(), Y = Pos.Bottom(loginLabel), Width = Dim.Percent(50)
            };
            this.Add(loginLabel, login);

            Label passwordLabel = new Label("Password:")
            {
                X = Pos.Center(), Y = 13
            };
            password = new TextField()
            {
                X = Pos.Center(), Y = Pos.Bottom(passwordLabel), Width = Dim.Percent(50),
                Secret = true
            };
            this.Add(passwordLabel, password);
        }
        private void DialogCanceled()
        {
            this.canceled = true;
            Application.RequestStop();
        }
        private void DialogSubmit()
        {
            clientToAdd = new Client() {
                fullname = fullname.Text.ToString(),
                age = Int32.Parse(age.Text.ToString()),
                email = email.Text.ToString(),
                driver_license_num = drLicenseNum.Text.ToString(),
                categories = categories.Text.ToString(),
                login = login.Text.ToString(),
                password = password.Text.ToString()
            };
            try
            {
                clientToAdd.id = clientProxy.Insert(clientToAdd);
                MessageBox.Query("Registration", "You have registered successfully!", "OK");
                this.canceled = false;
                Application.RequestStop();
            }
            catch(Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message,  "OK");
            }
        }
        public Client GetClient()
        {
            return this.clientToAdd;
        }
        public void SetRepository(ClientProxy repo)
        {
            this.clientProxy = repo;
        }
    }
}