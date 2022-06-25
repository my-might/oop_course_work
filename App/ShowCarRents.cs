using Terminal.Gui;
using System.Collections.Generic;
using ClassLib;

namespace App
{
    public class ShowCarRents : Dialog
    {
        private Service service;
        private ListView allRents;
        private UserState user;
        private int carId;
        public ShowCarRents()
        {
            Button ok = new Button("OK");
            ok.Clicked += DialogCanceled;
            this.AddButton(ok);
            allRents = new ListView(new List<Rent>())
            {
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            allRents.OpenSelectedItem += OnOpenRent;
            Label title = new Label("Rents list")
            {
                X = Pos.Center(), Y = 0
            };
            FrameView frameView = new FrameView("")
            {
                X = 3, Y = 3,
                Width = Dim.Fill() - 4,
                Height = 12
            };
            frameView.Add(allRents);
            this.Add(title, frameView);
        }
        private void OnOpenRent(ListViewItemEventArgs args)
        {
            Rent rent = new Rent();
            try
            {
                rent = (Rent)args.Value; 
            }
            catch
            {
                return;
            }
            ShowRentDialog dialog = new ShowRentDialog();
            dialog.SetInfo(rent, service, user);
            Application.Run(dialog);
            ShowCurrentPage();
        }
        public void SetInfo(UserState user, Service repo, int carId)
        {
            this.user = user;
            this.service = repo;
            this.carId = carId;
            ShowCurrentPage();
        }
        private void ShowCurrentPage()
        {
            List<Rent> rents = service.rentProxy.GetByCarId(carId);
            if(rents.Count != 0)
            {
                this.allRents.SetSource(rents);
            }
            else
            {
                List<string> emptyText = new List<string>();
                emptyText.Add("There are no rents for this car.");
                this.allRents.SetSource(emptyText);
            }
        }
        private void DialogCanceled()
        {
            Application.RequestStop();
        }
    }
}