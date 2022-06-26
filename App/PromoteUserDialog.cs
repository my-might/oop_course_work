using Terminal.Gui;
using ClassLib;
using System;

namespace App
{
    public class PromoteUserDialog : Dialog
    {
        private Service service;
        private TextField loginField;
        public PromoteUserDialog()
        {
            this.Title = "Promote user";
            Button ok = new Button("OK");
            ok.Clicked += DialogSubmit;
            Button cancel = new Button("Cancel"); 
            cancel.Clicked += DialogCanceled;
            this.AddButton(ok);
            this.AddButton(cancel);

            Label info = new Label("Here you can promote user to worker by login:")
            {
                X = Pos.Center(), Y = 5
            };

            Label loginLabel = new Label("Login:")
            {
                X = Pos.Center(), Y = Pos.Bottom(info) + 3
            };
            loginField = new TextField("")
            {
                X = Pos.Center(), Y = Pos.Bottom(loginLabel), Width = Dim.Percent(40)
            };
            this.Add(info, loginLabel, loginField);
        }
        public void SetService(Service service)
        {
            this.service = service;
        }
        private void DialogCanceled()
        {
            Application.RequestStop();
        }
        private void DialogSubmit()
        {
            string errorText = "";
            string loginInput = this.loginField.Text.ToString();
            try
            {
                User existing = service.userProxy.GetByLogin(loginInput);
                if(loginInput == "")
                {
                    errorText = "Login field mustn`t be empty";
                }
                else if(existing.is_worker)
                {
                    errorText = "User is already a worker";
                }
                else
                {
                    existing.is_worker = true;
                    service.userProxy.Update(existing);
                }
            }
            catch(Exception ex)
            {
                errorText = ex.Message;
            }
            if(errorText != "")
            {
                MessageBox.ErrorQuery("Promoting", errorText, "OK");
            }
            else
            {
                MessageBox.Query("Promoting", "User was promoted successfully!", "OK");
                Application.RequestStop();
            }
        }
    }
}