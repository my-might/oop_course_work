using System.Collections.Generic;

namespace ClassLib
{
    public class Car
    {
        public int id { get; set; }
        public string type { get; set; }
        public string color { get; set; }
        public int engine_id { get; set; }
        public Engine engine { get; set; }
        public bool conditioner { get; set; }
        public double price_per_day { get; set; }
        public string location { get; set; }
        public string fullname { get; set; }
        public List<Rent> rents { get; set; }
    }
}