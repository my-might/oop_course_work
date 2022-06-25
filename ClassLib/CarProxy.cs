using System;
using System.Collections.Generic;

namespace ClassLib
{
    abstract public class CarAbstract {
        abstract public int Insert(Car car);
        abstract public void DeleteById(int id);
        abstract public void Update(Car car);
        abstract public Car GetById(int id);
        abstract public List<Car> GetAll();
        abstract public int GetSearchPagesCount(string searchFullname);
        abstract public List<Car> GetSearchPage(string searchFullname, int page);
        abstract public List<Car> GetCarsByParams(CarParams parameters);
        abstract public List<string> GetAllTypes();
        abstract public List<string> GetAllColors();
        abstract public List<string> GetAllLocations();
    }
    public class CarProxy : CarAbstract
    {
        private CarAbstract carRepo;
        public CarProxy(ServiceContext service) {
            carRepo = new CarRepo(service);
        }

        public override void DeleteById(int id)
        {
            if(carRepo.GetById(id) == null)
            {
                throw new Exception("Car with entered id does not exist.");
            }
            carRepo.DeleteById(id);
        }

        public override Car GetById(int id)
        {
            Car result = carRepo.GetById(id);
            if(result == null)
            {
                throw new Exception("Car with entered id does not exist.");
            }
            return result;
        }

        public override int Insert(Car car)
        {
            if(car.category.Length != 1)
            {
                throw new Exception("Invalid car category format.");
            }
            int result = carRepo.Insert(car);
            if(result == 0)
            {
                throw new Exception("Something went wrong!");
            }
            return result;
        }

        public override void Update(Car car)
        {
            Car existing = carRepo.GetById(car.id);
            if(existing == null)
            {
                throw new Exception("Car does not exist.");
            }
            if(car.category.Length != 1)
            {
                throw new Exception("Invalid car category format.");
            }
            if(existing.type != car.type || existing.category != car.category)
            {
                throw new Exception("Car type and category are not updatable parameters.");
            }
            carRepo.Update(car);
        }
        public override List<Car> GetAll()
        {
            List<Car> result = carRepo.GetAll();
            return result;
        }
        public override int GetSearchPagesCount(string searchFullname)
        {
            int result = carRepo.GetSearchPagesCount(searchFullname);
            return result;
        }
        public override List<Car> GetSearchPage(string searchFullname, int page)
        {
            if(page > carRepo.GetSearchPagesCount(searchFullname))
            {
                throw new Exception("Invalid page number");
            }
            List<Car> result = carRepo.GetSearchPage(searchFullname, page);
            return result;
        }
        public override List<Car> GetCarsByParams(CarParams parameters)
        {
            if(parameters.minprice > parameters.maxPrice)
            {
                throw new Exception("Max price must be bidder than min price");
            }
            if((parameters.fromDate != new DateTime() && parameters.todate == new DateTime()) || (parameters.fromDate == new DateTime() && parameters.todate != new DateTime()))
            {
                throw new Exception("You have to set both dates to search with them.");
            }
            if(parameters.fromDate.Date > parameters.todate.Date)
            {
                throw new Exception("To date must be bigger than from date.");
            }
            List<Car> result = carRepo.GetCarsByParams(parameters);
            return result;
        }
        public override List<string> GetAllColors()
        {
            List<string> result = carRepo.GetAllColors();
            return result;
        }
        public override List<string> GetAllLocations()
        {
            List<string> result = carRepo.GetAllLocations();
            return result;
        }
        public override List<string> GetAllTypes()
        {
            List<string> result = carRepo.GetAllTypes();
            return result;
        }
    }
}