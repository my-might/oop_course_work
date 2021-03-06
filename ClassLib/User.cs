using System.Collections.Generic;
using System.Xml.Serialization;

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
        public bool is_worker { get; set; }
        public bool vip { get; set; }
        [XmlIgnore]
        public List<Rent> rents { get; set; }
    }
}