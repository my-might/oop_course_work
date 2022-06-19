using Terminal.Gui;
using System.Collections.Generic;
using ClassLib;

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
        public bool deleted;
        public bool updated;
        public Car carToShow;
        private Service repo;
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
                X = 20, Y = Pos.Top(idLabel), Width = Dim.Percent(50),
                ReadOnly = true
            };
            this.Add(idLabel, idField);

            Label fullnameLabel = new Label(2, 3, "Fullname:");
            fullnameField = new TextField("")
            {
                X = 20, Y = Pos.Top(fullnameLabel), Width = Dim.Percent(50),
                ReadOnly = true
            };
            this.Add(fullnameLabel, fullnameField);

            Label typeLabel = new Label(2, 5, "Type:");
            typeField = new TextField("")
            {
                X = 20, Y = Pos.Top(typeLabel), Width = Dim.Percent(50),
                ReadOnly = true
            };
            this.Add(typeLabel, typeField);

            Label colorLabel = new Label(2, 7, "Color:");
            colorfield = new TextField("")
            {
                X = 20, Y = Pos.Top(colorLabel), Width = Dim.Percent(50),
                ReadOnly = true
            };
            this.Add(colorLabel, colorfield);

            Label locationLabel = new Label(2, 11, "Location:");
            locationField = new TextField("")
            {
                X = 20, Y = Pos.Top(locationLabel), Width = Dim.Percent(50),
                ReadOnly = true
            };
            this.Add(locationLabel, locationField);

            Label enginePowerLabel = new Label(2, 13, "Engine power:");
            enginePowerField = new TextField("")
            {
                X = 20, Y = Pos.Top(enginePowerLabel), Width = Dim.Percent(50),
                ReadOnly = true
            };
            this.Add(enginePowerLabel, enginePowerField);

            Label engineConsumpLabel = new Label(2, 15, "Engine consumption:");
            engineConsumpField = new TextField("")
            {
                X = 20, Y = Pos.Top(engineConsumpLabel), Width = Dim.Percent(50),
                ReadOnly = true
            };
            this.Add(engineConsumpLabel, engineConsumpField);

            Label pricePerDayLabel = new Label(2, 17, "Price per day:");
            pricePerDayField = new TextField("")
            {
                X = 20, Y = Pos.Top(pricePerDayLabel), Width = Dim.Percent(50),
                ReadOnly = true
            };
            Label uahLabel = new Label("UAH")
            {
                X = Pos.Right(pricePerDayField) + 5, Y = Pos.Top(pricePerDayLabel)
            };
            this.Add(pricePerDayLabel, pricePerDayField, uahLabel);

            Label conditionerLabel = new Label(2, 19, "Conditioner:");
            conditioner = new CheckBox()
            {
                X = 20, Y = Pos.Top(conditionerLabel)
            };
            this.Add(conditionerLabel, conditioner);
            

            delete = new Button(2, 21, "Delete");
            delete.Clicked += OnDeleteCar;
            edit = new Button(2, 22, "Edit");
            edit.Clicked += OnEditCar;
            rent = new Button(2, 23, "Rent car");
            rent.Clicked += OnRentCar;
            viewRents = new Button(2, 23, "View all rents");
            viewRents.Clicked += OnShowRents;
            this.Add(delete, edit, rent, viewRents);
        }
        private void OnDeleteCar()
        {
            int index = MessageBox.Query("Delete film", "Are you sure?", "No", "Yes");
            if(index == 1)
            {
                ////service.DeleteCar()
                this.deleted = true;
                Application.RequestStop();
            }
        }
        private void OnEditCar()
        {
            EditCarDialog dialog = new EditCarDialog();
            dialog.SetCar(this.carToShow);
            dialog.SetRepository(this.repo);
            Application.Run(dialog);

            if(!dialog.canceled)
            {
                MessageBox.Query("Edit", "Car was updated!", "OK");
                Car updatedCar = dialog.GetCar();
                updatedCar.id = carToShow.id;
                this.updated = true;
                SetCar(updatedCar);
            }
        }
        private void OnRentCar()
        {
            user.RentCar(carToShow);
        }
        private void OnShowRents()
        {
            ShowCarRents dialog = new ShowCarRents();
            dialog.SetRepository(repo, carToShow.id);
            Application.Run(dialog);
        }
        private void OnSubscribe()
        {
            
        }
        public void SetCar(Car car)
        {
            this.carToShow = car;
            this.idField.Text = car.id.ToString();
            this.fullnameField.Text = car.fullname;
            this.colorfield.Text = car.color;
            this.typeField.Text = car.type;
            this.enginePowerField.Text = car.engine_power.ToString();
            this.engineConsumpField.Text = car.engine_fuel_consumption.ToString();
            this.locationField.Text = car.location;
            this.pricePerDayField.Text = car.price_per_day.ToString();
            this.conditioner.Checked = car.conditioner;
        }
        public void SetService(Service repo)
        {
            this.repo = repo;
        }
        public void SetUser(UserState user)
        {
            this.user = user;
            if(!user.User.isWorker)
            {
                delete.Visible = false;
                edit.Visible = false;
                viewRents.Visible = false;
            }
        }
        private void DialogCanceled()
        {
            Application.RequestStop();
        }
    }
}