using Terminal.Gui;
using System.Collections.Generic;
using ClassLib;

namespace App
{
    public class MainWindowUser : Window
    {
        private Service service;
        private Button viewProfile;
        private Button viewCars;
        private Button createCar;
        private Button promoteUser;
        private UserState user;
        
        private bool isModerator;
        public MainWindowUser()
        {
            this.Title = "Car rental salon";
            this.X = 0;
            this.Y = 1;

            Label hello = new Label("We are happy to see you in our car rental salon!")
            {
                X = Pos.Center(), Y = 5, Width = 40
            };
            Label available = new Label("There are available options for you:")
            {
                X = Pos.Center(), Y = 6
            };
            this.Add(hello, available);

            Button profile = new Button("My profile")
            {
                X = Pos.Center() - 20, Y = 9
            };
            profile.Clicked += ClickShowProfile;
            Button viewFilms = new Button("View all cars")
            {
                X = Pos.Center() + 5, Y = 9
            };
            viewFilms.Clicked += ClickShowFilms;
            createActor = new Button("Create actor")
            {
                X = Pos.Center() - 20, Y = 15
            };
            createActor.Clicked += ClickCreateActor;

            createFilm = new Button("Create film")
            {
                X = Pos.Center() - 20, Y = 18
            };
            createFilm.Clicked += ClickCreateFilm;

            promoteUser = new Button("Promote user")
            {
                X = Pos.Center() + 5, Y = 18
            };
            promoteUser.Clicked += ClickPromoteUser;

            this.Add(profile, viewFilms, viewActors, viewReviews, createReview, createFilm, createActor, promoteUser);

            loggedUser = new Label(" ")
            {
                X = Pos.Percent(2), Y = Pos.Percent(2),
                Width = Dim.Fill() - 20
            };
            
            Button logOut = new Button("Log out")
            {
                X = Pos.Percent(85), Y = Pos.Y(loggedUser)
            };
            logOut.Clicked += OnLogOut;
            this.Add(loggedUser, logOut);

        }
        public void SetUser(User user)
        {
            this.currentUser = user;
            this.loggedUser.Text = $"Logged user: {user.username}";
            if(user.isModerator == 0)
            {
                logged.Text = "You are successfully logged as user.";
                isModerator = false;
                createActor.Visible = false;
                createFilm.Visible = false;
                promoteUser.Visible = false;
            }
            else
            {
                logged.Text = "You are successfully logged as moderator.";
                isModerator = true;
            }
        }
        private void OnLogOut()
        {
            Application.Top.RemoveAll();
            UserInterface.ProcessRegistration();
        }
        private void ClickPromoteUser()
        {
            PromoteUserDialog dialog = new PromoteUserDialog();
            dialog.SetService(repo);
            Application.Run(dialog);
            if(!dialog.canceled)
            {
                string username = dialog.GetUsername();
                User user = repo.userRepository.GetByUsername(username);
                user.isModerator = 1;
                repo.userRepository.Update(user.id, user);
            }
        }
        private void ClickCreateFilm()
        {
            CreateFilmDialog dialog = new CreateFilmDialog();
            dialog.SetRepository(repo.actorRepository);
            Application.Run(dialog);
            if(!dialog.canceled)
            {
                Film film = dialog.GetFilm();
                int insertedId = (int)repo.filmRepository.Insert(film);
                film.id = insertedId;
                List<int> actorIds = dialog.GetActorIds();
                foreach(int id in actorIds)
                {
                    Role currentRole = new Role(){actorId = id, filmId = insertedId};
                    repo.roleRepository.Insert(currentRole);
                }
                List<Actor> roles = repo.roleRepository.GetAllActors(film.id);
                if(roles.Count != 0)
                {
                    film.actors = new Actor[roles.Count];
                    roles.CopyTo(film.actors);
                }
                else
                {
                    film.actors = null;
                }
                ShowFilmDialog filmDialog = new ShowFilmDialog();
                filmDialog.SetService(repo);
                filmDialog.SetFilm(film);
                filmDialog.SetUser(currentUser);
                Application.Run(filmDialog);
                if(filmDialog.deleted)
                {
                    repo.filmRepository.DeleteById(film.id);
                    repo.roleRepository.DeleteByFilmId(film.id);
                    repo.reviewRepository.DeleteByFilmId(film.id);
                }
                if(filmDialog.updated)
                {
                    repo.filmRepository.Update((long)film.id, dialog.GetFilm());
                    List<int> updatedRoles = filmDialog.GetUpdatedRoles();
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
            }
        }
        private void ClickShowProfile()
        {
            ShowProfileDialog dialog = new ShowProfileDialog();
            dialog.SetService(repo);
            dialog.SetUser(currentUser);
            dialog.SetReviews();
            Application.Run(dialog);
        }
        private void ClickShowFilms()
        {
            ShowAllFilmsDialog dialog = new ShowAllFilmsDialog();
            dialog.SetRepository(repo);
            dialog.SetUser(currentUser);
            Application.Run(dialog);
        }
        public void SetService(Service service)
        {
            this.repo = repo;
        }
    }
}