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

            
            Label fromDateLabel = new Label(10, 5, "From date:");
            fromDate = new DateField()
            {
                X = Pos.Center(), Y = Pos.Top(fromDateLabel)
            };
            this.Add(fromDateLabel, fromDate);

            Label toDateLabel = new Label(10, 7, "To date:");
            toDate = new DateField()
            {
                X = Pos.Center(), Y = Pos.Top(toDateLabel)
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
                bool isAvailable = service.rentProxy.IsAvailableForDates(frDate, tDate, carId);
                    if(!isAvailable)
                    {
                        int result = MessageBox.Query("Failed", "Car is not available in entered dates. Do you want to know if it will free?", "Yes", "No");
                        if(result == 0)
                        {
                            rentToReturn.client = user;
                            CarRentObserver observer = new CarRentObserver(rentToReturn);
                            RentManager rentManager = new RentManager();
                            rentManager.Attach(observer);
                        }
                        return;
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