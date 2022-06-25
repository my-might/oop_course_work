using System;

namespace ClassLib
{
    public class Rent
    {
        public int id { get; set; }
        public DateTime action_time { get; set; }
        public int car_id { get; set; }
        public Car car { get; set; }
        public int client_id { get; set; }
        public User client { get; set; }
        public DateTime from_date { get; set; }
        public DateTime to_date { get; set; }
        public bool isDeleted { get; set; }
    }
}