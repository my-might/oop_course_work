using ClassLib;

namespace App
{
    public abstract class Strategy
    {
        public abstract void GenerateReceipt(RentData data, Car car, string destination);
    }
    public class PDFStrategy : Strategy
    {
        public override void GenerateReceipt(RentData data, Car car, string destination)
        {
            throw new System.NotImplementedException();
        }
    }
    public class DOCStrategy : Strategy
    {
        public override void GenerateReceipt(RentData data, Car car, string destination)
        {
            throw new System.NotImplementedException();
        }
    }

    public class Receipt
    {
        Strategy strategy;
        RentData data;
        Car carToRent;
        string destination;
        public Receipt(RentData data, Car carToRent, Strategy strategy, string destination)
        {
            this.strategy = strategy;
            this.carToRent = carToRent;
            this.data = data;
            this.destination = destination;
        }
        public void CreateReceipt()
        {
            strategy.GenerateReceipt(data, carToRent, destination);
        }
    }
}