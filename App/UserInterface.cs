using ClassLib;
using Terminal.Gui;
using System.Collections.Generic;

namespace App
{
    public class UserInterface
    {
        private static Service service;
        private static UserState user;
        private static Toplevel top;
        
        public static void SetService(Service service1)
        {
            service = service1;
        }
        public static void ProcessApplication()
        {
            Application.Init();
            top = Application.Top;
            ProcessMain();
        }
        public static void ProcessMain()
        {
            user = new UserState(service);
            MenuBar menu = new MenuBar(new MenuBarItem[] {
                new MenuBarItem ("_File", new MenuItem [] {
                    new MenuItem ("_Sign in", "", user.SignIn),
                    new MenuItem ("_Sign up", "", user.SignUp),
                    new MenuItem ("_Exit", "", OnQuit)
                }),
                new MenuBarItem ("_Contacts", new MenuItem [] {
                    new MenuItem ("_About us!", "", About)
                })
            });
            MainWindowUser userWin = new MainWindowUser();
            userWin.SetService(service);
            userWin.SetUser(user);
            top.Add(userWin, menu);
            Application.Run();
        }
        private static void OnQuit()
        {
            Application.RequestStop();
        }
        public static void About() 
        {
            AboutDialog dialog = new AboutDialog();
            Application.Run(dialog);
        }
    }
}