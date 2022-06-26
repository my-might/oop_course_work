using Terminal.Gui;

namespace App
{
    public class AboutDialog : Dialog
    {
        public AboutDialog()
        {
            this.Title = "About company";
            Rect frame1 = new Rect(20, 2, 41, 7);
            Label companyInfo = new Label(2, 2, "Company info:");
            TextView info = new TextView(frame1)
            {
                X = 20, Y = Pos.Top(companyInfo), 
                Width = Dim.Fill(), Height = Dim.Fill(),
                Text = "Our company providing cars for rent.",
                ReadOnly = true
            };
            Rect frame2 = new Rect(20, 11, 41, 5);
            Label authorInfo = new Label(2, 11, "Details:");
            TextView author = new TextView(frame2)
            {
                X = 20, Y = Pos.Top(authorInfo), 
                Width = Dim.Fill(),
                Text = "Owner: Krivosheeva Valeria.\nLocation: Kyiv, Shevchenko street, 56\nAll questions you can ask by e-mail: \nrent.company@gmail.com.",
                ReadOnly = true
            };
            this.Add(companyInfo, info, authorInfo, author);
            Button ok = new Button("OK");
            ok.Clicked += DialogCanceled;
            this.AddButton(ok);
        }
        private void DialogCanceled()
        {
            Application.RequestStop();
        }
    }
}