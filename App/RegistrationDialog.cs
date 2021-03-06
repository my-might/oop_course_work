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
        private User clientToAdd;
        private UserProxy clientProxy;
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
                X = Pos.Center(), Y = Pos.Bottom(fullnameLabel), Width = Dim.Percent(50)
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

            Label emailLabel = new Label("Email:")
            {
                X = Pos.Center(), Y = 7
            };
            email = new TextField("")
            {
                X = Pos.Center(), Y = Pos.Bottom(emailLabel), Width = Dim.Percent(50)
            };
            this.Add(emailLabel, email);

            Label drLicenseLabel = new Label("Number of driver license:")
            {
                X = Pos.Center(), Y = 9
            };
            drLicenseNum = new TextField("")
            {
                X = Pos.Center(), Y = Pos.Bottom(drLicenseLabel), Width = Dim.Percent(50)
            };
            this.Add(drLicenseLabel, drLicenseNum);

            Label categoriesLabel = new Label("Category:")
            {
                X = Pos.Center(), Y = 11
            };
            categories = new TextField("")
            {
                X = Pos.Center(), Y = Pos.Bottom(categoriesLabel), Width = Dim.Percent(50)
            };
            this.Add(categoriesLabel, categories);

            Label loginLabel = new Label("Login:")
            {
                X = Pos.Center(), Y = 13
            };
            login = new TextField("")
            {
                X = Pos.Center(), Y = Pos.Bottom(loginLabel), Width = Dim.Percent(50)
            };
            this.Add(loginLabel, login);

            Label passwordLabel = new Label("Password:")
            {
                X = Pos.Center(), Y = 15
            };
            password = new TextField("")
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
            string errorText = "";
            if(fullname.Text.ToString() == "" || age.Text.ToString() == "" || email.Text.ToString() == "" || drLicenseNum.Text.ToString() == ""
                || categories.Text.ToString() == "" || login.Text.ToString() == "" || password.Text.ToString() == "")
            {
                errorText = "You have to fill all fields first.";
            }
            else if(!int.TryParse(age.Text.ToString(), out int ageParsed))
            {
                errorText = "Unavailable age value.";
            }
            else
            {
                clientToAdd = new User() {
                    fullname = fullname.Text.ToString(),
                    age = ageParsed,
                    email = email.Text.ToString(),
                    driver_license_num = drLicenseNum.Text.ToString(),
                    category = categories.Text.ToString(),
                    login = login.Text.ToString(),
                    password = password.Text.ToString(),
                    vip = false,
                    is_worker = false
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
                    errorText = ex.Message;
                }
            }
            if(errorText != "")
            {
                MessageBox.ErrorQuery("Error", errorText, "OK");
            }
            else
            {
                this.canceled = false;
                Application.RequestStop();
            }
        }
        public User GetClient()
        {
            return this.clientToAdd;
        }
        public void SetRepository(UserProxy userProxy)
        {
            this.clientProxy = userProxy;
        }
    }
}