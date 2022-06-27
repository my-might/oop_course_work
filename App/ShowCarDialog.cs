using Terminal.Gui;
using ClassLib;
using System;

namespace App
{
    public class ShowCarDialog : Dialog
    {
        private TextField idField;
        private TextField typeField;
        private TextField colorfield;
        private TextField enginePowerField;
        private TextField engineConsumpField;
        private CheckBox conditioner;
        private TextField pricePerDayField;
        private TextField locationField;
        private TextField fullnameField;
        private TextField category;
        public bool deleted;
        public bool updated;
        public Car carToShow;
        private Service service;
        private UserState user;
        private Button delete;
        private Button edit;
        private Button rent;
        private Button viewRents;
        public ShowCarDialog()
        {
            this.Title = "Show car";
            Button ok = new Button("OK");
            ok.Clicked += DialogCanceled;
            this.AddButton(ok);
            Label idLabel = new Label(2, 1, "ID:");
            idField = new TextField("")
            {
                X = 20, Y = Pos.Top(idLabel), Width = Dim.Percent(40),
                ReadOnly = true
            };
            this.Add(idLabel, idField);

            Label fullnameLabel = new Label(2, 2, "Fullname:");
            fullnameField = new TextField("")
            {
                X = 20, Y = Pos.Top(fullnameLabel), Width = Dim.Percent(40),
                ReadOnly = true
            };
            this.Add(fullnameLabel, fullnameField);

            Label typeLabel = new Label(2, 3, "Type:");
            typeField = new TextField("")
            {
                X = 20, Y = Pos.Top(typeLabel), Width = Dim.Percent(40),
                ReadOnly = true
            };
            this.Add(typeLabel, typeField);

            Label colorLabel = new Label(2, 4, "Color:");
            colorfield = new TextField("")
            {
                X = 20, Y = Pos.Top(colorLabel), Width = Dim.Percent(40),
                ReadOnly = true
            };
            this.Add(colorLabel, colorfield);

            Label locationLabel = new Label(2, 5, "Location:");
            locationField = new TextField("")
            {
                X = 20, Y = Pos.Top(locationLabel), Width = Dim.Percent(40),
                ReadOnly = true
            };
            this.Add(locationLabel, locationField);

            Label enginePowerLabel = new Label(2, 6, "Engine power:");
            enginePowerField = new TextField("")
            {
                X = 20, Y = Pos.Top(enginePowerLabel), Width = Dim.Percent(40),
                ReadOnly = true
            };
            this.Add(enginePowerLabel, enginePowerField);

            Label engineConsumpLabel = new Label(2, 7, "Engine consumption:");
            engineConsumpField = new TextField("")
            {
                X = 20, Y = Pos.Top(engineConsumpLabel), Width = Dim.Percent(40),
                ReadOnly = true
            };
            this.Add(engineConsumpLabel, engineConsumpField);

            Label pricePerDayLabel = new Label(2, 8, "Price per day:");
            pricePerDayField = new TextField("")
            {
                X = 20, Y = Pos.Top(pricePerDayLabel), Width = Dim.Percent(40),
                ReadOnly = true
            };
            Label uahLabel = new Label("UAH")
            {
                X = Pos.Right(pricePerDayField) + 3, Y = Pos.Top(pricePerDayLabel)
            };
            this.Add(pricePerDayLabel, pricePerDayField, uahLabel);

            Label conditionerLabel = new Label(2, 9, "Conditioner:");
            conditioner = new CheckBox()
            {
                X = 20, Y = Pos.Top(conditionerLabel)
            };
            this.Add(conditionerLabel, conditioner);
            
            Label categoryLabel = new Label(2, 10, "Category:");
            category = new TextField("")
            {
                X = 20, Y = Pos.Top(categoryLabel), Width = Dim.Percent(40),
                ReadOnly = true
            };
            this.Add(categoryLabel, category);

            delete = new Button(2, 12, "Delete");
            delete.Clicked += OnDeleteCar;
            edit = new Button(2, 13, "Edit");
            edit.Clicked += OnEditCar;
            rent = new Button(2, 14, "Rent car");
            rent.Clicked += OnRentCar;
            viewRents = new Button(2, 15, "View all rents");
            viewRents.Clicked += OnShowRents;
            this.Add(delete, edit, rent, viewRents);
        }
        private void OnDeleteCar()
        {
            int index = MessageBox.Query("Delete car", "Are you sure?", "No", "Yes");
            if(index == 1)
            {
                service.carProxy.DeleteById(carToShow.id);
                this.deleted = true;
                Application.RequestStop();
            }
        }
        private void OnEditCar()
        {
            EditCarDialog dialog = new EditCarDialog();
            dialog.SetCar(this.carToShow);
            dialog.SetRepository(this.service);
            Application.Run(dialog);

            if(!dialog.canceled)
            {
                Car carToUpdate = new Car();
                try
                {
                    carToUpdate = dialog.GetCar();
                    carToUpdate.id = carToShow.id;
                    service.carProxy.Update(carToUpdate);
                    MessageBox.Query("Edit", "Car was updated!", "OK");
                }
                catch(Exception ex)
                {
                    MessageBox.ErrorQuery("Error", ex.Message, "OK");
                    return;
                }
                this.updated = true;
                SetInfo(carToUpdate, service, user);
            }
        }
        private void OnRentCar()
        {
            user.RentCar(carToShow);
        }
        private void OnShowRents()
        {
            ShowCarRents dialog = new ShowCarRents();
            dialog.SetInfo(user, service, carToShow.id);
            Application.Run(dialog);
        }
        public void SetInfo(Car car, Service service, UserState user)
        {
            this.service = service;
            this.user = user;
            if(user.User == null || !user.User.is_worker)
            {
                delete.Visible = false;
                edit.Visible = false;
                viewRents.Visible = false;
            }
            this.carToShow = car;
            this.idField.Text = car.id.ToString();
            this.fullnameField.Text = car.fullname;
            this.colorfield.Text = car.color;
            this.typeField.Text = car.type;
            this.enginePowerField.Text = car.engine_power.ToString();
            this.engineConsumpField.Text = car.engine_fuel_consumption.ToString().Substring(0, 2);
            this.locationField.Text = car.location;
            this.pricePerDayField.Text = car.price_per_day.ToString();
            this.conditioner.Checked = car.conditioner;
            this.category.Text = car.category;
        }
        private void DialogCanceled()
        {
            Application.RequestStop();
        }
    }
}