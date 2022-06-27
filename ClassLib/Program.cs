using System;
using System.Collections.Generic;

namespace ClassLib
{
    class Program
    {
        static void Main(string[] args)
        {
            Service service = new Service();
            Console.WriteLine(service.carProxy.GetAllColors().Count.ToString());
        }
    }
}
