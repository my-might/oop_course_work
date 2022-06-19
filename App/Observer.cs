using ClassLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace App
{
    public abstract class Observer
    {
        public Rent rent;
        public Observer(Rent rent)
        {
            this.rent = rent;
        }
        public abstract void Update();
    }
    public class CarRentObserver : Observer
    {
        public CarRentObserver(Rent rent) : base(rent) {}
        public override void Update()
        {
            //send email to client
        }
    }

    public abstract class IRentManager
    {
        protected List<Observer> observers = new List<Observer>();
        public abstract void Attach(Observer observer);
        public abstract void Detach(Observer observer);
        public abstract void Notify(List<Observer> observersSpecial);
        public abstract void RentModification(int carId, DateTime from, DateTime to);
    }
    public class RentManager : IRentManager
    {
        public override void Attach(Observer observer)
        {
            observers.Add(observer);
        }
        public override void Detach(Observer observer)
        {
            observers.Remove(observer);
        }
        public override void Notify(List<Observer> observersSpecial)
        {
            foreach (Observer obs in observersSpecial)
            {
                obs.Update();
                observers.Remove(obs);
            }
        }
        public override void RentModification(int carId, DateTime from, DateTime to)
        {
            List<Observer> observersSpecial = observers.Where(o => o.rent.car_id == carId).
                                                Where(o => o.rent.from_date == from).
                                                Where(o => o.rent.to_date == to).ToList();
            Notify(observersSpecial);
        }
    }
}