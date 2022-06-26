using System.Collections.Generic;
using System;

namespace ClassLib
{
    public class Car
    {
        public int id { get; set; }
        public string type { get; set; }
        public string color { get; set; }
        public int engine_power { get; set; }
        public double engine_fuel_consumption { get; set; }
        public bool conditioner { get; set; }
        public double price_per_day { get; set; }
        public string location { get; set; }
        public string fullname { get; set; }
        public string category { get; set; }
        public List<Rent> rents { get; set; }
        
        public override string ToString()
        {
            return string.Format($"[{this.id}] {this.fullname}, {this.price_per_day.ToString()} UAH");
        }
    }

    public class CarParams
    {
        public string color;
        public string type;
        public string location;
        public int enginePower;
        public int minprice;
        public int maxPrice;
        public DateTime fromDate;
        public DateTime todate;
    }
}