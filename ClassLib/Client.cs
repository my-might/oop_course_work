using System.Collections.Generic;

namespace ClassLib
{
    public class User
    {
        public int id { get; set; }
        public string fullname { get; set; }
        public int age { get; set; }
        public string email { get; set; }
        public string driver_license_num { get; set; }
        public string category { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public bool isWorker { get; set; }
        public bool vip { get; set; }
        public List<Rent> rents { get; set; }
    }
}