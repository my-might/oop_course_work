using Terminal.Gui;
using System;
using System.Collections.Generic;
using ClassLib;

namespace App
{
    public class CreateCarDialog : Dialog
    {
        public bool canceled;
        protected Service repo;
        protected TextField inputType;
        protected TextField inputColor;
        protected TextField inputFullname;
        protected TextField inputEnginePower;
        protected TextField inputEngineConsump;
        protected TextField inputPricePerDay;
        protected CheckBox inputConditioner;
        protected TextField inputLocation;
        protected Car car;
        public CreateCarDialog()
        {
            this.Title = "Create car";
            Button ok = new Button("OK");
            ok.Clicked += DialogSubmit;
            Button cancel = new Button("Cancel"); 
            cancel.Clicked += DialogCanceled;
            this.AddButton(ok);
            this.AddButton(cancel);

            Label fullnameLabel = new Label(2, 2, "Fullname:");
            inputFullname = new TextField("")
            {
                X = 20, Y = Pos.Top(fullnameLabel), Width = Dim.Percent(50)
            };
            this.Add(fullnameLabel, inputFullname);

            Label typeLabel = new Label(2, 4, "Type:");
            inputType = new TextField("")
            {
                X = 20, Y = Pos.Top(typeLabel), Width = Dim.Percent(50)
            };
            this.Add(typeLabel, inputType);

            Label colorLabel = new Label(2, 6, "Color:");
            inputColor = new TextField("")
            {
                X = 20, Y = Pos.Top(colorLabel), Width = Dim.Percent(50)
            };
            this.Add(colorLabel, inputColor);

            Label locationLabel = new Label(2, 10, "Location:");
            inputLocation = new TextField("")
            {
                X = 20, Y = Pos.Top(locationLabel), Width = Dim.Percent(50)
            };
            this.Add(locationLabel, inputLocation);

            Label enginePowerLabel = new Label(2, 13, "Engine power:");
            inputEnginePower = new TextField("")
            {
                X = 20, Y = Pos.Top(enginePowerLabel), Width = Dim.Percent(50),
            };
            this.Add(enginePowerLabel, inputEnginePower);

            Label engineConsumpLabel = new Label(2, 15, "Engine consumption:");
            inputEngineConsump = new TextField("")
            {
                X = 20, Y = Pos.Top(engineConsumpLabel), Width = Dim.Percent(50),
            };
            this.Add(engineConsumpLabel, inputEngineConsump);

            Label pricePerDayLabel = new Label(2, 17, "Price per day:");
            inputPricePerDay = new TextField("")
            {
                X = 20, Y = Pos.Top(pricePerDayLabel), Width = Dim.Percent(50),
            };
            this.Add(pricePerDayLabel, inputPricePerDay);

            Label conditionerLabel = new Label(2, 19, "Conditioner:");
            inputConditioner = new CheckBox()
            {
                X = 20, Y = Pos.Top(conditionerLabel)
            };
            this.Add(conditionerLabel, inputConditioner);
        }
        public void SetRepository(Service repo)
        {
            this.repo = repo;
        }
        public Car GetCar()
        {
            return this.car;
        }
        private void DialogCanceled()
        {
            this.canceled = true;
            Application.RequestStop();
        }
        private void DialogSubmit()
        {
            string errorText = "";
            if(inputEnginePower.Text.ToString() == "" || !int.TryParse(inputEnginePower.Text.ToString(), out int enginePower) || enginePower < 0)
            {
                errorText = "Engine power must be positive integer.";
            }
            else if(inputEngineConsump.Text.ToString() == "" || !double.TryParse(inputEngineConsump.Text.ToString(), out double engineConsump) || engineConsump < 0)
            {
                errorText = "Engine consumption must be positive integer.";
            }
            else if(inputPricePerDay.Text.ToString() == "" || !double.TryParse(inputPricePerDay.Text.ToString(), out double pricePerDay) || pricePerDay < 0)
            {
                errorText = "Price per day must be positive integer.";
            }
            else if(inputFullname.Text.ToString() == "" || inputColor.Text.ToString() == "" || inputType.Text.ToString() == "" || inputLocation.Text.ToString() == "")
            {
                errorText = "All fields must be written.";
            }
            else
            {
                //insert
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
    }
}