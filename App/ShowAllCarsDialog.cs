using Terminal.Gui;
using System.Collections.Generic;
using ClassLib;

namespace App
{
    public class ShowAllCarsDialog : Dialog
    {
        private Service repo;
        private int page = 1;
        private Label totalPagesLabel;
        private Label currentPageLabel;
        private TextField searchFullname;
        private string searchValue = "";
        private ListView allCars;
        private UserState currentUser;
        public ShowAllCarsDialog()
        {
            Button backToOptions = new Button("Go back");
            backToOptions.Clicked += BackToOptions;
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
            ShowFilmDialog dialog = new ShowFilmDialog();
            dialog.SetService(repo);
            dialog.SetFilm(film);
            dialog.SetUser(currentUser);
            Application.Run(dialog);
            if(dialog.deleted)
            {
                repo.filmRepository.DeleteById(film.id);
                repo.roleRepository.DeleteByFilmId(film.id);
                repo.reviewRepository.DeleteByFilmId(film.id);
            }
            if(page > repo.filmRepository.GetSearchPagesCount(searchValue) && page > 1)
            {
                page--;
            }
            if(dialog.updated)
            {
                repo.filmRepository.Update((long)film.id, dialog.GetFilm());
                List<int> updatedRoles = dialog.GetUpdatedRoles();
                    foreach(int actorId in updatedRoles)
                    {
                        if(!repo.roleRepository.IsExist(film.id, actorId))
                        {
                            Role currentRole = new Role(){actorId = actorId, filmId = film.id};
                            repo.roleRepository.Insert(currentRole);
                        }
                    }
                    foreach(Actor actor in roles)
                    {
                        bool isExist = false;
                        if(updatedRoles.Count != 0)
                        {
                            for(int i = 0; i<updatedRoles.Count; i++)
                            {
                                if(actor.id == updatedRoles[i])
                                {
                                    isExist = true;
                                    break;
                                }
                            }
                        }
                        if(!isExist)
                        {
                            repo.roleRepository.Delete(actor.id, film.id);
                        }
                    }
            }
            ShowCurrentPage();
        }
        public void SetRepository(RemoteService repo)
        {
            this.repo = repo;
            ShowCurrentPage();
        }
        public void SetUser(User user)
        {
            this.currentUser = user;
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
            int totalPages = repo.filmRepository.GetSearchPagesCount(searchValue);
            if(page == totalPages)
            {
                return;
            }
            this.page++;
            ShowCurrentPage();
        }
        private void ShowCurrentPage()
        {
            /////select by fullname
            int totalPages = repo.filmRepository.GetSearchPagesCount(searchValue);
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
                emptyText.Add("There are no films in the database.");
                this.allCars.SetSource(emptyText);
            }
            else
            {
                this.currentPageLabel.Text = this.page.ToString();
                this.totalPagesLabel.Text = totalPages.ToString();
                this.allCars.SetSource(repo.filmRepository.GetSearchPage(searchValue, page));
            }
        }
        private void BackToOptions()
        {

        }
        private void DialogCanceled()
        {
            Application.RequestStop();
        }
    }
}