using Terminal.Gui;
using ClassLib;
using System;

namespace App
{
    public class LoginDialog : Dialog
    {
        public bool canceled;
        private User loggedClient;
        private TextField login;
        private TextField password;
        private UserProxy clientProxy;
        public LoginDialog()
        {
            this.Title = "Sign in";
            this.Height = 23;
            this.Width = 50;
            Button ok = new Button("OK");
            ok.Clicked += DialogSubmit;
            Button cancel = new Button("Cancel"); 
            cancel.Clicked += DialogCanceled;
            this.AddButton(ok);
            this.AddButton(cancel);

            Label info = new Label("Sign in")
            {
                X = Pos.Center(), Y = 1
            };
            this.Add(info);

            Label loginLabel = new Label("Login:")
            {
                X = Pos.Center(), Y = 5
            };
            login = new TextField("")
            {
                X = Pos.Center(), Y = Pos.Bottom(loginLabel), Width = Dim.Percent(50)
            };
            this.Add(loginLabel, login);

            Label passwordLabel = new Label("Password:")
            {
                X = Pos.Center(), Y = 9
            };
            password = new TextField("")
            {
                X = Pos.Center(), Y = Pos.Bottom(passwordLabel), Width = Dim.Percent(50),
                Secret = true
            };
            this.Add(passwordLabel, password);
        }
        private void DialogSubmit()
        {
            string errorText = "";
            if(login.Text.ToString() == "" || password.Text.ToString() == "")
            {
                errorText = "You have to fill all fields first.";
            }
            else
            {
                try
                {
                    loggedClient = clientProxy.GetByLogin(login.Text.ToString());
                    if(loggedClient.password != password.Text.ToString()) {
                        throw new Exception("Wrong login or password");
                    }
                    MessageBox.Query("Log in", "You have logged in successfully!", "OK");
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
            return this.loggedClient;
        }
        private void DialogCanceled()
        {
            this.canceled = true;
            Application.RequestStop();
        }
        public void SetRepository(UserProxy userProxy)
        {
            this.clientProxy = userProxy;
        }
    }
}