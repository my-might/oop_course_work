using ClassLib;
using Terminal.Gui;
using System.Collections.Generic;

namespace App
{
    public class UserInterface
    {
        private static Service service;
        private static Client client;
        private static Worker worker;
        
        private static TextField idToShow;
        private static Toplevel top;
        
        public static void SetService(Service repo1)
        {
            service = repo1;
        }
        public static void ProcessApplication()
        {
            Application.Init();
            top = Application.Top;
            ProcessRegistration();
        }
        private static void OnQuit()
        {
            Application.RequestStop();
        }
        public static void ProcessRegistration()
        {
            MenuBar menu = new MenuBar(new MenuBarItem[] {
                new MenuBarItem ("_File", new MenuItem [] {
                    new MenuItem ("_Sign in", "", SignIn),
                    new MenuItem ("_Sign up", "", SignUp),
                    new MenuItem ("_I'm worker", "", SignInWorker),
                    new MenuItem ("_Exit", "", OnQuit)
                }),
                new MenuBarItem ("_Help", new MenuItem [] {
                    new MenuItem ("_About!", "", Help)
                })
            });
            MainWindowUser userWin = new MainWindowUser();
            userWin.SetService(repo);
            userWin.SetUser(registered);
            top.Add(userWin, menu);
            Application.Run();
        }
        public static void SignUp()
        {
            RegistrationDialog dialog = new RegistrationDialog();
            dialog.SetRepository(service.clientProxy);
            top.Add(dialog);
            Application.Run(dialog);
            if(!dialog.canceled)
            {
                client = dialog.GetClient();
            }
        }
        public static void SignIn()
        {
            LoginDialog dialog = new LoginDialog();
            dialog.SetRepository(service.clientProxy);
            Application.Run(dialog);
            if(!dialog.canceled)
            {
                client = dialog.GetClient();
            }
        }
        public static void SignInWorker()
        {
            LoginWorkerDialog dialog = new LoginWorkerDialog();
            dialog.SetRepository(service.workerProxy);
            Application.Run(dialog);
            if(!dialog.canceled)
            {
                worker = dialog.GetWorker();
            }
        }
    }
}