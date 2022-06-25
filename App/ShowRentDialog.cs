using Terminal.Gui;
using ClassLib;

namespace App
{
    public class ShowRentDialog : Dialog
    {
        private TextField idField;
        private DateField actionDate;
        private TimeField actionTime;
        private TextField carId;
        private TextField carFullname;
        private TextField clientId;
        private TextField clientUsername;
        private DateField fromDate;
        private DateField toDate;
        private CheckBox isDeleted;
        private Button delete;
        public bool deleted;
        private Rent rentToShow;
        private Service service;
        public ShowRentDialog()
        {
            this.Title = "Show rent";
            Button ok = new Button("OK");
            ok.Clicked += DialogCanceled;
            this.AddButton(ok);
            Label idLabel = new Label(2, 2, "ID:");
            idField = new TextField("")
            {
                X = 20, Y = Pos.Top(idLabel), Width = Dim.Percent(50),
                ReadOnly = true
            };
            this.Add(idLabel, idField);

            Label actionDateLabel = new Label(2, 4, "Action time:");
            actionDate = new DateField()
            {
                X = 20, Y = Pos.Top(actionDateLabel), Width = Dim.Percent(30),
                IsShortFormat = true, ReadOnly = true
            };
            actionTime = new TimeField()
            {
                X = 30, Y = Pos.Top(actionDateLabel), Width = Dim.Percent(30),
                IsShortFormat = true, ReadOnly = true
            };
            this.Add(actionDateLabel, actionDate, actionTime);

            Label carIdLabel = new Label(2, 6, "Car id:");
            carId = new TextField("")
            {
                X = 20, Y = Pos.Top(carIdLabel), Width = Dim.Percent(50),
                ReadOnly = true
            };
            this.Add(carIdLabel, carId);

            Label carFullnameLabel = new Label(2, 8, "Car fullname:");
            carFullname = new TextField("")
            {
                X = 20, Y = Pos.Top(carFullnameLabel), Width = Dim.Percent(50),
                ReadOnly = true
            };
            this.Add(carFullnameLabel, carFullname);

            Label clientIdLable = new Label(2, 10, "Client id:");
            clientId = new TextField("")
            {
                X = 20, Y = Pos.Top(clientIdLable), Width = Dim.Percent(50),
                ReadOnly = true
            };
            this.Add(clientIdLable, clientId);

            Label clientUsernameLabel = new Label(2, 12, "Client username:");
            clientUsername = new TextField("")
            {
                X = 20, Y = Pos.Top(clientUsernameLabel), Width = Dim.Percent(50),
                ReadOnly = true
            };
            this.Add(clientUsernameLabel, clientUsername);

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

            Label isDeletedLabel = new Label(2, 18, "Canceled:");
            isDeleted = new CheckBox()
            {
                X = 20, Y = Pos.Top(toDateLabel)
            };
            this.Add(isDeletedLabel, isDeleted);

            delete = new Button("Cancel rent")
            {
                X = 20, Y = 20
            };
            delete.Clicked += OnDeleteRent;
        }
        public void SetInfo(Rent rent, Service service, UserState user)
        {
            this.service = service;
            this.rentToShow = rent;
            this.idField.Text = rent.id.ToString();
            this.actionDate.Date = rent.action_time.Date;
            this.actionTime.Time = rent.action_time.TimeOfDay;
            this.carId.Text = rent.car_id.ToString();
            this.carFullname.Text = rent.car.fullname;
            this.clientId.Text = rent.client_id.ToString();
            this.clientUsername.Text = rent.client.fullname;
            this.fromDate.Date = rent.from_date.Date;
            this.toDate.Date = rent.to_date.Date;
            this.isDeleted.Checked = rent.isDeleted;
            if(user.User.id != rent.client_id)
            {
                delete.Visible = false;
            }
        }
        private void OnDeleteRent()
        {
            if(rentToShow.isDeleted)
            {
                MessageBox.Query("Cancel rent", "Rent is already canceled", "OK");
                return;
            }
            int index = MessageBox.Query("Cancel rent", "Are you sure?", "No", "Yes");
            if(index == 1)
            {
                rentToShow.isDeleted = true;
                service.rentProxy.Update(rentToShow);
                this.deleted = true;
                Application.RequestStop();
            }
        }
        private void DialogCanceled()
        {
            Application.RequestStop();
        }
    }
}