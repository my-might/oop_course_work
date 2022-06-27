using System;
using System.Xml.Serialization;

namespace ClassLib
{
    public class Rent
    {
        public int id { get; set; }
        public DateTime action_time { get; set; }
        public int car_id { get; set; }
        [XmlIgnore]
        public Car car { get; set; }
        public int client_id { get; set; }
        public User client { get; set; }
        public DateTime from_date { get; set; }
        public DateTime to_date { get; set; }
        public bool is_deleted { get; set; }
        
        public override string ToString()
        {
            return string.Format($"[{this.id}] car {this.car_id}, user {this.client_id}");
        }
    }
}