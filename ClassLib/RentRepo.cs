using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ClassLib
{
    public class RentRepo : RentAbstract
    {
        private ServiceContext context;
        public RentRepo(ServiceContext context)
        {
            this.context = context;
        }
        public override int Insert(Rent rent)
        {
            context.rents.Add(rent);
            context.SaveChanges();
            return rent.id;
        }
        public override void Update(Rent rent)
        {
            var local = context.rents.Find(rent.id);
            if (local != null)
            {
                context.Entry(local).State = EntityState.Detached;
            }
            context.Entry(rent).State = EntityState.Modified;
            context.SaveChanges();
        }
        public override void DeleteById(int id)
        {
            context.rents.Remove(context.rents.Find(id));
            context.SaveChanges();
        }
        public override Rent GetById(int id)
        {
            Rent result = context.rents.Find(id);
            return result;
        }
        public override List<Rent> GetByCarId(int carId)
        {
            List<Rent> result = context.rents.Where(r => r.car_id == carId).ToList();
            return result;
        }
        public override List<Rent> GetByUserId(int userId)
        {
            List<Rent> result = context.rents.Where(r => r.client_id == userId).ToList();
            return result;
        }
        public override bool IsAvailableForDates(DateTime fromDate, DateTime toDate, int carId)
        {
            List<Rent> result = context.rents.Where(r => r.car_id == carId && 
                                ((r.from_date <= fromDate && r.to_date >= fromDate) 
                                || (r.from_date <= toDate && r.to_date >= toDate) 
                                || (r.from_date >= fromDate && r.to_date <= toDate))).ToList();
            if(result.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}