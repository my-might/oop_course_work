using Terminal.Gui;
using System.Collections.Generic;
using ClassLib;
using System;

namespace App
{
    public class ShowAllCarsDialog : Dialog
    {
        private Service service;
        private int page = 1;
        private Label totalPagesLabel;
        private Label currentPageLabel;
        private TextField searchFullname;
        private string searchValue = "";
        private ListView allCars;
        private CarParams carParams;
        private UserState user;
        public ShowAllCarsDialog()
        {
            Button ok = new Button("OK");
            ok.Clicked += DialogCanceled;
            this.AddButton(ok);
            allCars = new ListView(new List<Car>())
            {
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            allCars.OpenSelectedItem += OnOpenCar;
            Label title = new Label("Cars list")
            {
                X = Pos.Center(), Y = 0
            };
            Button prevPage = new Button("Prev")
            {
                X = Pos.Center() - 28, Y = 2
            };
            prevPage.Clicked += ClickPrevPage;
            Button nextPage = new Button("Next")
            {
                X = Pos.Center() + 20, Y = 2
            };
            nextPage.Clicked += ClickNextPage;
            currentPageLabel = new Label("?")
            {
                X = Pos.Right(prevPage) + 4, Y = 2,
                Width = 5
            };
            totalPagesLabel = new Label("?")
            {
                X = Pos.Left(nextPage) - 6, Y = 2,
                Width = 5
            };
            FrameView frameView = new FrameView("")
            {
                X = 3, Y = 3,
                Width = Dim.Fill() - 4,
                Height = 12
            };
            frameView.Add(allCars);

            Label fullnameLabel = new Label("Search by fullname:")
            {
                X = 5, Y = Pos.Bottom(frameView)
            };
            searchFullname = new TextField("")
            {
                X = Pos.Right(fullnameLabel) + 1, Y = Pos.Y(fullnameLabel), Width = Dim.Percent(50)
            };
            searchFullname.KeyPress += OnSearchFullnameEnter;

            this.Add(title, prevPage, nextPage, currentPageLabel, totalPagesLabel, 
                    frameView, fullnameLabel, searchFullname);
        }
        private void OnSearchFullnameEnter(KeyEventEventArgs args)
        {
            if(args.KeyEvent.Key == Key.Enter)
            {
                page = 1;
                this.searchValue = this.searchFullname.Text.ToString();
                ShowCurrentPage();
            }
        }
        private void OnOpenCar(ListViewItemEventArgs args)
        {
            Car car = new Car();
            try
            {
                car = (Car)args.Value;
            }
            catch
            {
                return;
            }
            ShowCarDialog dialog = new ShowCarDialog();
            dialog.SetInfo(car, service, user);
            Application.Run(dialog);
            if(page > service.carProxy.GetSearchPagesCount(searchValue, carParams) && page > 1)
            {
                page--;
            }
            ShowCurrentPage();
        }
        public void SetInfo(Service service, UserState user, CarParams carParams)
        {
            this.service = service;
            this.user = user;
            this.carParams = carParams;
            try
            {
                ShowCurrentPage();
            }
            catch(Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        }
        private void ClickPrevPage()
        {
            if(page == 1)
            {
                return;
            }
            this.page--;
            ShowCurrentPage();
        }
        private void ClickNextPage()
        {
            int totalPages = service.carProxy.GetSearchPagesCount(searchValue, carParams);
            if(page == totalPages)
            {
                return;
            }
            this.page++;
            ShowCurrentPage();
        }
        private void ShowCurrentPage()
        {
            int totalPages = service.carProxy.GetSearchPagesCount(searchValue, carParams);
            if(page > totalPages && page > 1)
            {
                page = totalPages;
            }
            if(totalPages != 0 && page == 0)
            {
                page = 1;
            }
            if(totalPages == 0)
            {
                this.page = 0;
                this.currentPageLabel.Text = this.page.ToString();
                this.totalPagesLabel.Text = totalPages.ToString();
                List<string> emptyText = new List<string>();
                emptyText.Add("There are no cars in the database.");
                this.allCars.SetSource(emptyText);
            }
            else
            {
                this.currentPageLabel.Text = this.page.ToString();
                this.totalPagesLabel.Text = totalPages.ToString();
                this.allCars.SetSource(service.carProxy.GetSearchPage(searchValue, page, carParams));
            }
        }
        private void DialogCanceled()
        {
            Application.RequestStop();
        }
    }
}