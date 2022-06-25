using System;
using ClassLib;

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {
            Service service = new Service();
            UserInterface.SetService(service);
            UserInterface.ProcessApplication();
        }
    }
}
