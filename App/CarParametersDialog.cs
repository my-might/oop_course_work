using Terminal.Gui;
using System;
using System.Collections.Generic;
using ClassLib;

namespace App
{
    public class CarParametersDialog : Dialog
    {
        public bool canceled;
        protected Service service;
        protected ComboBox color;
        protected ComboBox type;
        protected TextField minPrice;
        protected TextField maxPrice;
        protected ComboBox location;
        protected TextField enginePower;
        protected DateField fromDate;
        protected DateField toDate; 
        private CarParams paramsToSearch;
        public CarParametersDialog()
        {
            this.Title = "Car search options";
            Button ok = new Button("Search!");
            ok.Clicked += DialogSubmit;
            Button cancel = new Button("Cancel"); 
            cancel.Clicked += DialogCanceled;
            this.AddButton(ok);
            this.AddButton(cancel);

            Label carColor = new Label(2, 2, "Color:");
            color = new ComboBox("not selected")
            {
                X = 20, Y = Pos.Top(carColor), Width = Dim.Percent(50), Height = Dim.Percent(30)
            };
            this.Add(carColor, color);

            Label carType = new Label(2, 4, "Type:");
            type = new ComboBox("not selected")
            {
                X = 20, Y = Pos.Top(carType), Width = Dim.Percent(50), Height = Dim.Percent(30)
            };
            this.Add(carType, type);

            Label carLocation = new Label(2, 6, "Location:");
            location = new ComboBox("not selected")
            {
                X = 20, Y = Pos.Top(carLocation), Width = Dim.Percent(50), Height = Dim.Percent(30)
            };
            this.Add(carLocation, location);

            Label carEngine = new Label(2, 10, "Engine power:");
            enginePower = new TextField("")
            {
                X = 20, Y = Pos.Top(carEngine), Width = Dim.Percent(40)
            };
            this.Add(carEngine, enginePower);

            Label price = new Label("< price <") 
            {
                X = Pos.Percent(50), Y = 12
            };
            minPrice = new TextField("")
            {
                X = Pos.Percent(50) - 10, Y = 12, Width = Dim.Percent(10)
            };
            maxPrice = new TextField("")
            {
                X = Pos.Percent(50) + 10, Y = 12, Width = Dim.Percent(10)
            };
            this.Add(price, minPrice, maxPrice);

            Label date = new Label("< date <") 
            {
                X = Pos.Percent(50), Y = 14
            };
            fromDate = new DateField()
            {
                X = Pos.Percent(50) - 13, Y = 14
            };
            toDate = new DateField()
            {
                X = Pos.Percent(50) + 10, Y = 14
            };
            this.Add(date, fromDate, toDate);
        }
        public CarParams GetParams()
        {
            return this.paramsToSearch;
        }
        public void SetRepository(Service service)
        {
            this.service = service;
            color.SetSource(service.carProxy.GetAllColors());
            type.SetSource(service.carProxy.GetAllTypes());
            location.SetSource(service.carProxy.GetAllLocations());
        }
        private void DialogCanceled()
        {
            this.canceled = true;
            Application.RequestStop();
        }
        private void DialogSubmit()
        {
            string errorText = "";
            paramsToSearch = new CarParams();
            Console.WriteLine(fromDate.Date.ToString());
            if(enginePower.Text.ToString() != "" && (!int.TryParse(enginePower.Text.ToString(), out paramsToSearch.enginePower) || paramsToSearch.enginePower < 0))
            {
                errorText = "Engine power must be positive integer.";
            }
            else if(minPrice.Text.ToString() != "" && (!int.TryParse(minPrice.Text.ToString(), out paramsToSearch.minprice) || paramsToSearch.minprice < 0))
            {
                errorText = "Min price must be positive integer.";
            }
            else if(maxPrice.Text.ToString() != "" && (!int.TryParse(maxPrice.Text.ToString(), out paramsToSearch.maxPrice) || paramsToSearch.maxPrice < 0))
            {
                errorText = "Max price must be positive integer.";
            }
            else if(fromDate.Date != new DateTime() && (!DateTime.TryParse(fromDate.Text.ToString(), out paramsToSearch.fromDate) || paramsToSearch.fromDate < DateTime.Now.Date))
            {
                errorText = "From date must be in date format and bigger than today`s date.";
            }
            else if(toDate.Date != new DateTime() && (!DateTime.TryParse(toDate.Text.ToString(), out paramsToSearch.todate) || paramsToSearch.todate < DateTime.Now.Date))
            {
                errorText = "To date must be in date format and bigger than today`s date.";
            }
            else
            {
                paramsToSearch.color = this.color.Text.ToString();
                paramsToSearch.type = this.type.Text.ToString();
                paramsToSearch.location = this.location.Text.ToString();
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