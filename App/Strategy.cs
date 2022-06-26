using ClassLib;
using System;
using System.IO;
using System.IO.Compression;
using System.Xml.Linq;
using System.Xml;
using Aspose.Pdf;
using Aspose.Pdf.Text;

namespace App
{
    public abstract class Strategy
    {
        public abstract void GenerateReceipt(RentData data, Car car, UserState user, string destination);
    }
    public class PDFStrategy : Strategy
    {
        public override void GenerateReceipt(RentData data, Car car, UserState user, string destination)
        {
            Document document = new Document();

            Page page = document.Pages.Add();

            TextFragment textFragment1 = new TextFragment("Receipt");
            textFragment1.Position = new Position(page.PageInfo.Width/2, 10);
            textFragment1.TextState.FontSize = 14;
            textFragment1.TextState.Font = FontRepository.FindFont("TimesNewRoman");
            textFragment1.TextState.FontStyle = FontStyles.Bold;
            TextBuilder textBuilder = new TextBuilder(page);
            textBuilder.AppendText(textFragment1);

            TextFragment textFragment2 = new TextFragment("Client info");
            textFragment2.Position = new Position(10, 20);
            textFragment2.TextState.FontSize = 14;
            textFragment2.TextState.Font = FontRepository.FindFont("TimesNewRoman");
            textFragment2.TextState.FontStyle = FontStyles.Bold;
            textBuilder = new TextBuilder(page);
            textBuilder.AppendText(textFragment2);

            TextFragment textFragment3 = new TextFragment($"ID: {user.User.id}\nFullname: {user.User.fullname}\nAge: {user.User.age}\nEmail: {user.User.email}\nDriver license number: {user.User.driver_license_num}\nCategory: {user.User.category}\nLogin: {user.User.login}\nVip: {user.User.vip.ToString()}");
            textFragment3.Position = new Position(10, 25);
            textFragment3.TextState.FontSize = 14;
            textFragment3.TextState.Font = FontRepository.FindFont("TimesNewRoman");
            textBuilder = new TextBuilder(page);
            textBuilder.AppendText(textFragment3);

            TextFragment textFragment4 = new TextFragment("Rent info");
            textFragment4.Position = new Position(10, textFragment3.Rectangle.Height + 25);
            textFragment4.TextState.FontSize = 14;
            textFragment4.TextState.Font = FontRepository.FindFont("TimesNewRoman");
            textFragment4.TextState.FontStyle = FontStyles.Bold;
            textBuilder = new TextBuilder(page);
            textBuilder.AppendText(textFragment4);

            TextFragment textFragment5 = new TextFragment($"ID: {data.rent.id}\nFullname: {data.rent.action_time.ToString()}\nCar ID: {data.rent.car_id}\nCar fullname: {car.fullname}\nFrom date: {data.rent.from_date.ToShortDateString()}\nTo date: {data.rent.to_date.ToShortDateString()}");
            textFragment5.Position = new Position(10, textFragment4.Position.YIndent + 5);
            textFragment5.TextState.FontSize = 14;
            textFragment5.TextState.Font = FontRepository.FindFont("TimesNewRoman");
            textBuilder = new TextBuilder(page);
            textBuilder.AppendText(textFragment5);

            TextFragment textFragment6 = new TextFragment("Rent conditions");
            textFragment6.Position = new Position(10, textFragment5.Rectangle.Height + 30);
            textFragment6.TextState.FontSize = 14;
            textFragment6.TextState.Font = FontRepository.FindFont("TimesNewRoman");
            textFragment6.TextState.FontStyle = FontStyles.Bold;
            textBuilder = new TextBuilder(page);
            textBuilder.AppendText(textFragment6);

            TextFragment textFragment7 = new TextFragment(data.conditions);
            textFragment7.Position = new Position(10, textFragment6.Position.YIndent + 5);
            textFragment7.TextState.FontSize = 14;
            textFragment7.TextState.Font = FontRepository.FindFont("TimesNewRoman");
            textBuilder = new TextBuilder(page);
            textBuilder.AppendText(textFragment7);

            TextFragment textFragment8 = new TextFragment($"Total price: {data.totalPrice} UAH");
            textFragment8.Position = new Position(10, textFragment7.Rectangle.Height + 35);
            textFragment8.TextState.FontSize = 14;
            textFragment8.TextState.Font = FontRepository.FindFont("TimesNewRoman");
            textFragment8.TextState.FontStyle = FontStyles.Bold;
            textBuilder = new TextBuilder(page);
            textBuilder.AppendText(textFragment8);

            document.Save(destination + @"/Receipt "+ DateTime.Now.ToString().Replace("/", ".")+".pdf");
        }
    }
    public class DOCStrategy : Strategy
    {
        RentData data;
        Car carToRent;
        UserState user;
        public override void GenerateReceipt(RentData data, Car car, UserState user, string destination)
        {
            this.data = data;
            this.carToRent = car;
            this.user = user;
            string zipPath = @"./../data/Sample.docx";
            string extractPath = @"./../data/example";
            ZipFile.ExtractToDirectory(zipPath, extractPath);

            XElement root = XElement.Load(@"./../data/example/word/document.xml");

            FindAndReplace(root);

            root.Save(@"./../data/example/word/document.xml");

            ZipFile.CreateFromDirectory(@"./../data/example", destination + @"/Receipt "+ DateTime.Now.ToString().Replace("/", ".")+".docx");
            Directory.Delete(@"./../data/example", true);
        }
        private void FindAndReplace(XElement root)
        {
            if (root.FirstNode != null
            && root.FirstNode.NodeType == XmlNodeType.Text)
            {
                switch (root.Value)
                {
                    case "{{": root.Value = ""; break;
                    case "}}": root.Value = ""; break;
                    case "clientId": root.Value = $"{user.User.id}"; break;
                    case "clientFullname": root.Value = $"{user.User.fullname}"; break;
                    case "clientAge": root.Value = $"{user.User.age}"; break;
                    case "clientEmail": root.Value = $"{user.User.email}"; break;
                    case "clientLicense": root.Value = $"{user.User.driver_license_num}"; break;
                    case "clientCategory": root.Value = $"{user.User.category}"; break;
                    case "clientLogin": root.Value = $"{user.User.login}"; break;
                    case "clientVip": root.Value = $"{user.User.vip.ToString()}"; break;
                    case "rentId": root.Value = $"{data.rent.id}"; break;
                    case "rentDate": root.Value = $"{data.rent.action_time.ToString()}"; break;
                    case "rentCarId": root.Value = $"{data.rent.car_id}"; break;
                    case "rentCarFullname": root.Value = $"{carToRent.fullname}"; break;
                    case "rentFrom": root.Value = $"{data.rent.from_date.ToShortDateString()}"; break;
                    case "rentTo": root.Value = $"{data.rent.to_date.ToShortDateString()}"; break;
                    case "conditions": root.Value = $"{data.conditions}"; break;
                    case "totalPrice": root.Value = $"{data.totalPrice.ToString()}"; break;
                }
            }
            foreach (XElement el in root.Elements())
            {
                FindAndReplace(el);
            }
        } 
    }
    

    public class Receipt
    {
        Strategy strategy;
        RentData data;
        Car carToRent;
        UserState user;
        string destination;
        public Receipt(RentData data, Car carToRent, UserState user, Strategy strategy, string destination)
        {
            this.strategy = strategy;
            this.carToRent = carToRent;
            this.data = data;
            this.destination = destination;
            this.user = user;
        }
        public void CreateReceipt()
        {
            strategy.GenerateReceipt(data, carToRent, user, destination);
        }
    }
}