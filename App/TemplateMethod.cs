using ClassLib;
using Terminal.Gui;

namespace App
{
    public abstract class RentCar
    {
        protected RentData data = new RentData();
        public RentData Renting(User user, Car car, int daysToRent) {
            CheckCategory(user.category, car.category);
            CheckAge(user.age);
            Confirm(user.email);
            TotalPrice(car.price_per_day, daysToRent);
            Conditions();
            return data;
        }
        protected void CheckCategory(string userCategory, string carCategory) 
        {
            if(userCategory == carCategory)
            {
                this.data.allowed = true;
            }
        }
        protected void CheckAge(int age)
        {
            if(age > 21 && age < 70)
            {
                this.data.allowed = true;
            }
        }
        protected abstract void Confirm(string email);
        protected abstract void TotalPrice(double carPrice, int daysToRent);
        protected abstract void Conditions();
    }
    public class VipRentCar : RentCar
    {
        protected override void Confirm(string email)
        {
            int result = MessageBox.Query("Rent", "Do you confirm your rent?", "No", "Yes");
            if(result == 1) {this.data.confirmed = true;}
        }
        protected override void TotalPrice(double carPrice, int daysToRent)
        {
            double priceTotal = carPrice*daysToRent;
            double priceSale = priceTotal - 0.2*priceTotal;
            this.data.totalPrice = priceSale;
        }
        protected override void Conditions()
        {
            string conditions = "In the event of an accident, insurance is paid in half.\nRent can be canceled no later than one day before the date of issue of the car.";
            this.data.conditions = conditions;
        }
    }
    public class OrdinaryRentCar : RentCar
    {
        protected override void Confirm(string email)
        {
            MessageBox.Query("Rent", "Please, confirm your rent by letter on your email.", "OK");
            this.data.confirmed = true;
        }
        protected override void TotalPrice(double carPrice, int daysToRent)
        {
            double priceTotal = carPrice*daysToRent;
            double priceWithSecure = priceTotal + 0.05*priceTotal;
            this.data.totalPrice = priceWithSecure;
        }
        protected override void Conditions()
        {
            string conditions = "In the event of an accident, insurance is paid fully by client.\nRent can be canceled no later than three days before the date of issue of the car.";
            this.data.conditions = conditions;
        }   
    }
    public class RentData 
    {
        public bool allowed;
        public bool confirmed;
        public string conditions;
        public double totalPrice;
        public Rent rent;
    }
}