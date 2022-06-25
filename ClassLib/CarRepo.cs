using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ClassLib
{
    public class CarRepo : CarAbstract
    {
        private ServiceContext context;
        private RentRepo rentRepo;
        public CarRepo(ServiceContext context)
        {
            this.context = context;
            this.rentRepo = new RentRepo(context);
        }
        public override int Insert(Car car)
        {
            context.cars.Add(car);
            context.SaveChanges();
            return car.id;
        }
        public override void Update(Car car)
        {
            var local = context.cars.Find(car.id);
            if (local != null)
            {
                context.Entry(local).State = EntityState.Detached;
            }
            context.Entry(car).State = EntityState.Modified;
            context.SaveChanges();
        }
        public override void DeleteById(int id)
        {
            context.cars.Remove(context.cars.Find(id));
            context.SaveChanges();
        }
        public override Car GetById(int id)
        {
            Car result = context.cars.Find(id);
            return result;
        }
        public override List<Car> GetAll()
        {
            List<Car> result = context.cars.Select(c => c).ToList();
            return result;
        }
        public override int GetSearchPagesCount(string searchFullname)
        {
            const int pageSize = 10;
            int result = context.cars.Where(c => c.fullname.Contains(searchFullname)).Count();
            return (int)Math.Ceiling((int)result / (double)pageSize);
        }
        public override List<Car> GetSearchPage(string searchFullname, int page)
        {
            const int pageSize = 10;
            List<Car> result = context.cars.FromSqlRaw("SELECT * FROM cars WHERE fullname LIKE '%' || {0} || '%' LIMIT {1} OFFSET {2}", searchFullname, pageSize, pageSize*(page-1)).ToList();
            return result;
        }
        public override List<Car> GetCarsByParams(CarParams parameters)
        {
            List<Car> result = GetAll();
            if(parameters.type != "")
            {
                result = GetByType(parameters.type, result);
            }
            if(parameters.color != "")
            {
                result = GetByColor(parameters.color, result);
            }
            if(parameters.location != "")
            {
                result = GetByLocation(parameters.location, result);
            }
            if(parameters.enginePower != 0)
            {
                result = GetByEnginePower(parameters.enginePower, result);
            }
            if(parameters.minprice != 0 && parameters.maxPrice != 0)
            {
                result = GetByPrice(parameters.minprice, parameters.maxPrice, result);
            }
            if(parameters.fromDate != new DateTime())
            {
                result = GetByDates(parameters.fromDate, parameters.todate, result);
            }
            return result;
        }
        public override List<string> GetAllColors()
        {
            List<string> result = context.cars.Select(c => c.color).ToList();
            return result;
        }
        public override List<string> GetAllLocations()
        {
            List<string> result = context.cars.Select(c => c.location).ToList();
            return result;
        }
        public override List<string> GetAllTypes()
        {
            List<string> result = context.cars.Select(c => c.type).ToList();
            return result;
        }
        private List<Car> GetByColor(string color, List<Car> car)
        {
            List<Car> result = car.Where(c => c.color == color).ToList();
            return result;
        }
        private List<Car> GetByType(string type, List<Car> car)
        {
            List<Car> result = car.Where(c => c.type == type).ToList();
            return result;
        }
        private List<Car> GetByLocation(string location, List<Car> car)
        {
            List<Car> result = car.Where(c => c.location == location).ToList();
            return result;
        }
        private List<Car> GetByEnginePower(int enginePower, List<Car> car)
        {
            List<Car> result = car.Where(c => c.engine_power == enginePower).ToList();
            return result;
        }
        private List<Car> GetByPrice(int minPrice, int maxPrice, List<Car> car)
        {
            List<Car> result = car.Where(c => c.price_per_day >= minPrice && c.price_per_day <= maxPrice).ToList();
            return result;
        }
        private List<Car> GetByDates(DateTime fromDate, DateTime toDate, List<Car> cars)
        {
            List<Car> result = new List<Car>();
            foreach (Car car in cars)
            {
                if(rentRepo.IsAvailableForDates(fromDate, toDate, car.id))
                {
                    result.Add(car);
                }
            }      
            return result;
        }
    }
}