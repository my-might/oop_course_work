using Terminal.Gui;
using ClassLib;
using System;

namespace App
{
    public class RentDatesDialog : Dialog
    {
        private DateField fromDate;
        private DateField toDate;
        private Rent rentToReturn;
        private Service service;
        private User user;
        private int carId;
        public bool canceled;
        public RentDatesDialog()
        {
            this.Title = "Rent dates";
            Button ok = new Button("OK");
            ok.Clicked += DialogSubmit;
            Button cancel = new Button("Cancel"); 
            cancel.Clicked += DialogCanceled;
            this.AddButton(ok);
            this.AddButton(cancel);

            
            Label fromDateLabel = new Label(2, 14, "From date:");
            fromDate = new DateField()
            {
                X = 20, Y = Pos.Top(fromDateLabel), Width = Dim.Percent(30),
                IsShortFormat = true, ReadOnly = true
            };
            this.Add(fromDateLabel, fromDate);

            Label toDateLabel = new Label(2, 16, "To date:");
            toDate = new DateField()
            {
                X = 20, Y = Pos.Top(toDateLabel), Width = Dim.Percent(30),
                IsShortFormat = true, ReadOnly = true
            };
            this.Add(toDateLabel, toDate);
        }
        private void DialogSubmit()
        {
            string errorText = "";
            if(!DateTime.TryParse(fromDate.Text.ToString(), out DateTime frDate) || frDate < DateTime.Now.Date)
            {
                errorText = "From date must be in date format and bigger than today`s date.";
            }
            else if(!DateTime.TryParse(toDate.Text.ToString(), out DateTime tDate) || tDate < DateTime.Now.Date)
            {
                errorText = "To date must be in date format and bigger than today`s date.";
            }
            else if(frDate.Date > tDate.Date)
            {
                errorText = "To date must be bigger than from date.";
            }
            else
            {
                rentToReturn = new Rent() {
                    from_date = frDate,
                    to_date = tDate,
                    car_id = carId
                };
                try 
                {
                    bool isAvailable = service.rentProxy.IsAvailableForDates(frDate, tDate, carId);
                    if(!isAvailable)
                    {
                        int result = MessageBox.Query("Failed", "Car is not available in entered dates. Do you want to know if it will free?", "Yes", "No");
                        if(result == 0)
                        {
                            CarRentObserver observer = new CarRentObserver(rentToReturn, user);
                            RentManager rentManager = new RentManager();
                            rentManager.Attach(observer);
                        }
                        return;
                    }
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
        public void SetInfo(Service service, int carId, User user)
        {
            this.service = service;
            this.carId = carId;
            this.user = user;
        }
        public Rent GetRent()
        {
            return this.rentToReturn;
        }
        private void DialogCanceled()
        {
            this.canceled = true;
            Application.RequestStop();
        }
    }
}