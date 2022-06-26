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
            List<Car> result = context.cars.FromSqlRaw("SELECT * FROM cars").ToList();
            return result;
        }
        public override int GetSearchPagesCount(string searchFullname, CarParams carParams)
        {
            const int pageSize = 10;
            List<Car> byFullname = context.cars.FromSqlRaw("SELECT * FROM cars WHERE fullname LIKE '%' || {0} || '%'", searchFullname).ToList();
            int result = GetCarsByParams(carParams, byFullname).Count;
            return (int)Math.Ceiling((int)result / (double)pageSize);
        }
        public override List<Car> GetSearchPage(string searchFullname, int page, CarParams carParams)
        {
            const int pageSize = 10;
            List<Car> byFullname = context.cars.FromSqlRaw("SELECT * FROM cars WHERE fullname LIKE '%' || {0} || '%' LIMIT {1} OFFSET {2}", searchFullname, pageSize, pageSize*(page-1)).ToList();
            List<Car> result = GetCarsByParams(carParams, byFullname);
            return result;
        }
        public override List<Car> GetCarsByParams(CarParams parameters, List<Car> selectFrom)
        {
            if(parameters.type != "not selected")
            {
                Console.WriteLine(parameters.type);
                selectFrom = GetByType(parameters.type, selectFrom);
            }
            if(parameters.color != "not selected")
            {
                Console.WriteLine(parameters.color);
                selectFrom = GetByColor(parameters.color, selectFrom);
            }
            if(parameters.location != "not selected")
            {
                Console.WriteLine(parameters.location);
                selectFrom = GetByLocation(parameters.location, selectFrom);
            }
            if(parameters.enginePower != 0)
            {
                selectFrom = GetByEnginePower(parameters.enginePower, selectFrom);
            }
            if(parameters.minprice != 0 && parameters.maxPrice != 0)
            {
                selectFrom = GetByPrice(parameters.minprice, parameters.maxPrice, selectFrom);
            }
            if(parameters.fromDate != new DateTime(2001, 01, 01))
            {
                selectFrom = GetByDates(parameters.fromDate, parameters.todate, selectFrom);
            }
            return selectFrom;
        }
        public override List<string> GetAllColors()
        {
            List<Car> all = GetAll();
            List<string> result = new List<string>();
            foreach(Car car in all)
            {
                if(!result.Contains(car.color))
                {
                    result.Add(car.color);
                }
            }
            return result;
        }
        public override List<string> GetAllLocations()
        {
            List<Car> all = GetAll();
            List<string> result = new List<string>();
            foreach(Car car in all)
            {
                if(!result.Contains(car.location))
                {
                    result.Add(car.location);
                }
            }
            return result;
        }
        public override List<string> GetAllTypes()
        {
            List<Car> all = GetAll();
            List<string> result = new List<string>();
            foreach(Car car in all)
            {
                if(!result.Contains(car.type))
                {
                    result.Add(car.type);
                }
            }
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