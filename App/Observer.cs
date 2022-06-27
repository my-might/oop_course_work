using ClassLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Net;
using System.Net.Mail;

namespace App
{
    [XmlInclude(typeof(CarRentObserver))]
    public abstract class Observer
    {
        [XmlElement("rent")]
        public Rent rent;
        public Observer()
        {
            this.rent = new Rent();
        }
        public Observer(Rent rent)
        {
            this.rent = rent;
        }
        public abstract void Update();
    }
    public class CarRentObserver : Observer
    {
        public CarRentObserver(Rent rent) : base(rent) {}
        public CarRentObserver()
        {
            this.rent = new Rent();
        }
        public override void Update()
        {
            MailMessage message = new MailMessage();  
            SmtpClient smtp = new SmtpClient();  
            message.From = new MailAddress("rent.company@gmail.com");  
            message.To.Add(new MailAddress(rent.client.email));  
            message.Subject = "Rent subscription";  
            message.IsBodyHtml = true;
            message.Body = $"<h1>Car with id [{rent.id}] is free for dates from {rent.from_date} to {rent.to_date}!<h1>";  
            smtp.Port = 587;  
            smtp.Host = "smtp.gmail.com"; 
            smtp.EnableSsl = true;  
            smtp.UseDefaultCredentials = false;  
            smtp.Credentials = new NetworkCredential("rent.company@gmail.com", "company.email.password");  
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;  
            //smtp.Send(message);
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
        public RentManager()
        {
            observers = XMLSerialization.ImportData();
        }
        public override void Attach(Observer observer)
        {
            observers = XMLSerialization.ImportData();
            observers.Add(observer);
            XMLSerialization.ExportData(observers);
        }
        public override void Detach(Observer observer)
        {
            observers = XMLSerialization.ImportData();
            observers.Remove(observer);
            XMLSerialization.ExportData(observers);
        }
        public override void Notify(List<Observer> observersSpecial)
        {
            foreach (Observer obs in observersSpecial)
            {
                obs.Update();
                observers.Remove(obs);
            }
            XMLSerialization.ExportData(observers);
        }
        public override void RentModification(int carId, DateTime from, DateTime to)
        {
            observers = XMLSerialization.ImportData();
            List<Observer> observersSpecial = observers.Where(r => r.rent.car_id == carId && 
                                ((from <= r.rent.from_date && to >= r.rent.from_date) 
                                || (from <= r.rent.to_date && to >= r.rent.to_date) 
                                || (from >= r.rent.from_date && to <= r.rent.to_date))).ToList();
            Notify(observersSpecial);
        }
    }
}