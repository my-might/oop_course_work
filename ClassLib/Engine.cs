using System.Collections.Generic;

namespace ClassLib
{
    public class Engine
    {
        public int id { get; set; }
        public int power { get; set; }
        public int torque { get; set; }
        public double fuel_consumption { get; set; }
        public string fullname { get; set; }
        public List<Car> cars { get; set; }
    }
}