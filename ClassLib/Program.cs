using System;
using System.Collections.Generic;

namespace ClassLib
{
    class Program
    {
        static void Main(string[] args)
        {
            Service service = new Service();
            CarParams parameters = new CarParams();
            parameters.fromDate = new DateTime(2001, 01, 01);
            parameters.todate = new DateTime(2001, 01, 01);
            Console.WriteLine(service.carProxy.GetSearchPagesCount("", parameters));
        }
    }
}
