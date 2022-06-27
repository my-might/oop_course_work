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
            textFragment1.Position = new Position(page.PageInfo.Width/2, 800);
            textFragment1.TextState.FontSize = 14;
            textFragment1.TextState.Font = FontRepository.FindFont("TimesNewRoman");
            textFragment1.TextState.FontStyle = FontStyles.Bold;
            TextBuilder textBuilder = new TextBuilder(page);
            textBuilder.AppendText(textFragment1);

            TextFragment textFragment2 = new TextFragment("Client info");
            textFragment2.Position = new Position(100, 760);
            textFragment2.TextState.FontSize = 14;
            textFragment2.TextState.Font = FontRepository.FindFont("TimesNewRoman");
            textFragment2.TextState.FontStyle = FontStyles.Bold;
            textBuilder = new TextBuilder(page);
            textBuilder.AppendText(textFragment2);

            TextFragment textFragment3 = new TextFragment($"ID: {user.User.id}" + Environment.NewLine + $"Fullname: {user.User.fullname}" + 
                Environment.NewLine + $"Age: {user.User.age}" + Environment.NewLine + $"Email: {user.User.email}");
            textFragment3.TextState.FontSize = 14;
            textFragment3.TextState.Font = FontRepository.FindFont("TimesNewRoman");
            TextParagraph par1 = new TextParagraph();
            par1.AppendLine(textFragment3);
            par1.Position = new Position(100, 700);
            textBuilder = new TextBuilder(page);
            textBuilder.AppendParagraph(par1);

            TextFragment textFragment9 = new TextFragment($"Driver license number: {user.User.driver_license_num}" + Environment.NewLine + $"Category: {user.User.category}" + Environment.NewLine + 
                $"Login: {user.User.login}" + Environment.NewLine + $"Vip: {user.User.vip.ToString()}");
            textFragment3.TextState.FontSize = 14;
            textFragment3.TextState.Font = FontRepository.FindFont("TimesNewRoman");
            TextParagraph par3 = new TextParagraph();
            par3.AppendLine(textFragment9);
            par3.Position = new Position(100, 640);
            textBuilder = new TextBuilder(page);
            textBuilder.AppendParagraph(par3);

            TextFragment textFragment4 = new TextFragment("Rent info");
            textFragment4.Position = new Position(100, 500);
            textFragment4.TextState.FontSize = 14;
            textFragment4.TextState.Font = FontRepository.FindFont("TimesNewRoman");
            textFragment4.TextState.FontStyle = FontStyles.Bold;
            textBuilder = new TextBuilder(page);
            textBuilder.AppendText(textFragment4);

            TextFragment textFragment5 = new TextFragment($"ID: {data.rent.id}" + Environment.NewLine + $"Fullname: {data.rent.action_time.ToString()}" 
                + Environment.NewLine + $"Car ID: {data.rent.car_id}" + Environment.NewLine + $"Car fullname: {car.fullname}");
            textFragment5.TextState.FontSize = 14;
            textFragment5.TextState.Font = FontRepository.FindFont("TimesNewRoman");
            TextParagraph par2 = new TextParagraph();
            par2.AppendLine(textFragment5);
            par2.Position = new Position(100, 470);
            textBuilder = new TextBuilder(page);
            textBuilder.AppendParagraph(par2);

            TextFragment textFragment10 = new TextFragment($"From date: {data.rent.from_date.ToShortDateString()}" 
                + Environment.NewLine + $"To date: {data.rent.to_date.ToShortDateString()}");
            textFragment10.TextState.FontSize = 14;
            textFragment10.TextState.Font = FontRepository.FindFont("TimesNewRoman");
            TextParagraph par4 = new TextParagraph();
            par4.AppendLine(textFragment10);
            par4.Position = new Position(100, 430);
            textBuilder = new TextBuilder(page);
            textBuilder.AppendParagraph(par4);

            TextFragment textFragment6 = new TextFragment("Rent conditions");
            textFragment6.Position = new Position(100, 440);
            textFragment6.TextState.FontSize = 14;
            textFragment6.TextState.Font = FontRepository.FindFont("TimesNewRoman");
            textFragment6.TextState.FontStyle = FontStyles.Bold;
            textBuilder = new TextBuilder(page);
            textBuilder.AppendText(textFragment6);

            string[] conditions = data.conditions.Split("\n");
            string conditionsToSet = "";
            foreach(string con in conditions)
            {
                conditionsToSet += con + Environment.NewLine;
            }
            TextFragment textFragment7 = new TextFragment(conditionsToSet);
            textFragment7.Position = new Position(100, 410);
            textFragment7.TextState.FontSize = 14;
            textFragment7.TextState.Font = FontRepository.FindFont("TimesNewRoman");
            textBuilder = new TextBuilder(page);
            textBuilder.AppendText(textFragment7);

            TextFragment textFragment8 = new TextFragment($"Total price: {data.totalPrice} UAH");
            textFragment8.Position = new Position(100, 300);
            textFragment8.TextState.FontSize = 14;
            textFragment8.TextState.Font = FontRepository.FindFont("TimesNewRoman");
            textFragment8.TextState.FontStyle = FontStyles.Bold;
            textBuilder = new TextBuilder(page);
            textBuilder.AppendText(textFragment8);

            
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