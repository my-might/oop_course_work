using System;
using System.Collections.Generic;

namespace ClassLib
{
    abstract public class RentAbstract {
        abstract public int Insert(Rent rent);
        abstract public void DeleteById(int id);
        abstract public void Update(Rent rent);
        abstract public Rent GetById(int id);
        abstract public List<Rent> GetByCarId(int carId);
        abstract public List<Rent> GetByUserId(int userId);
        abstract public bool IsAvailableForDates(DateTime fromDate, DateTime toDate, int carId);
    }
    public class RentProxy : RentAbstract
    {
        private RentAbstract rentRepo;
        private CarAbstract carProxy;
        private UserAbstract userProxy;
        public RentProxy(ServiceContext service) {
            rentRepo = new RentRepo(service);
            carProxy = new CarProxy(service);
            userProxy = new UserProxy(service);
        }

        public override void DeleteById(int id)
        {
            if(rentRepo.GetById(id) == null)
            {
                throw new Exception("Rent with entered id does not exist.");
            }
            rentRepo.DeleteById(id);
        }

        public override Rent GetById(int id)
        {
            Rent result = rentRepo.GetById(id);
            if(result == null)
            {
                throw new Exception("Rent with entered id does not exist.");
            }
            return result;
        }

        public override int Insert(Rent rent)
        {
            if(carProxy.GetById(rent.car_id) == null)
            {
                throw new Exception("Car with entered id does not exist");
            }
            if(userProxy.GetById(rent.client_id) == null)
            {
                throw new Exception("User with entered id does not exist.");
            }
            if(rent.from_date.Date > rent.to_date.Date)
            {
                throw new Exception("To date mist be bigger than from date.");
            }
            int result = rentRepo.Insert(rent);
            if(result == 0)
            {
                throw new Exception("Something went wrong!");
            }
            return result;
        }

        public override void Update(Rent rent)
        {
            Rent existing = rentRepo.GetById(rent.id);
            if(rent.car_id != existing.car_id || rent.client_id != existing.client_id
                || rent.from_date != existing.from_date || rent.to_date != existing.to_date
                || rent.action_time != existing.action_time)
            {
                throw new Exception("You are available only to edit isDeleted field.");
            }
            rentRepo.Update(rent);
        }
        public override List<Rent> GetByCarId(int carId)
        {
            if(carProxy.GetById(carId) == null)
            {
                throw new Exception("Car with entered id does not exist");
            }
            List<Rent> result = rentRepo.GetByCarId(carId);
            return result;
        }
        public override List<Rent> GetByUserId(int userId)
        {
            if(userProxy.GetById(userId) == null)
            {
                throw new Exception("User with entered id does not exist.");
            }
            List<Rent> result = rentRepo.GetByUserId(userId);
            return result;
        }
        public override bool IsAvailableForDates(DateTime fromDate, DateTime toDate, int carId)
        {
            if(carProxy.GetById(carId) == null)
            {
                throw new Exception("Car with entered id does not exist");
            }
            if(fromDate.Date > toDate.Date)
            {
                throw new Exception("To date mist be bigger than from date.");
            }
            return rentRepo.IsAvailableForDates(fromDate, toDate, carId);
        }
    }
}