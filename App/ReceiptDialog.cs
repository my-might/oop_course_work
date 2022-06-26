using Terminal.Gui;
using ClassLib;

namespace App
{
    public class ReceiptDialog : Dialog
    {
        private RentData data;
        private Car car;
        private RadioGroup receiptType;
        private TextField directory;
        private UserState user;
        public ReceiptDialog()
        {
            this.Title = "Rent receipt";
            Button ok = new Button("OK");
            ok.Clicked += DialogSubmit;
            this.AddButton(ok);
            
            Label receiptTypeLabel = new Label(2, 4, "Select receipt file type:");
            receiptType = new RadioGroup(new NStack.ustring[]{"PDF file", "Word file"})
            {
                X = Pos.Center(), Y = Pos.Top(receiptTypeLabel)
            };
            this.Add(receiptTypeLabel, receiptType);

            Label filePathLabel = new Label("Select directory:")
            {
                X = Pos.Center(), Y = 8
            };
            Button openDirectory = new Button("Open directory")
            {
                X = Pos.Center(), Y = Pos.Bottom(filePathLabel)
            };
            openDirectory.Clicked += SelectDirectory;
            directory = new TextField("not selected")
            {
                X = Pos.Center(), Y = Pos.Bottom(openDirectory) + 1, Width = Dim.Fill() - 4,
                ReadOnly = true
            };
        }
        private void SelectDirectory()
        {
            OpenDialog dialog = new OpenDialog("Open directory", "Open?");
            dialog.CanChooseDirectories = true;
            dialog.CanChooseFiles = false;
            
            Application.Run(dialog);
        
            if (!dialog.Canceled)
            {
                NStack.ustring filePath = dialog.FilePath;
                directory.Text = filePath;
            }
            else
            {
                directory.Text = "not selected.";
            }
        }
        public void SetInfo(RentData data, Car car, UserState user)
        {
            this.data = data;
            this.car = car;
            this.user = user;
        }
        private void DialogSubmit()
        {
            string errorText = "";
            if(directory.Text.ToString() == "")
            {
                errorText = "You must choose directory to save receipt.";
            }
            else if(receiptType.SelectedItem == -1)
            {
                errorText = "You must select receipt file type.";
            }
            else
            {
                Strategy strategy;
                if(receiptType.SelectedItem == 0) { strategy = new PDFStrategy(); }
                else { strategy = new DOCStrategy(); }
                Receipt receipt = new Receipt(data, car, user, strategy, directory.Text.ToString());
                receipt.CreateReceipt();
            }
            if(errorText != "")
            {
                MessageBox.ErrorQuery("Error", errorText, "OK");
            }
            else
            {
                MessageBox.Query("Success", "Receipt was created successfully!", "OK");
                Application.RequestStop();
            }
        }
    }
}