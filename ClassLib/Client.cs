using System.Collections.Generic;

namespace ClassLib
{
    public class Client
    {
        public int id { get; set; }
        public string fullname { get; set; }
        public int age { get; set; }
        public string email { get; set; }
        public string driver_license_num { get; set; }
        public string categories { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public List<Rent> rents { get; set; }
    }
}