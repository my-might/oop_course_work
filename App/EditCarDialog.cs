using ClassLib;

namespace App
{
    public class EditCarDialog : CreateCarDialog
    {
        public EditCarDialog()
        {
            this.Title = "Edit car";
        }
        public void SetCar(Car car)
        {
            this.inputFullname.Text = car.fullname;
            this.inputType.Text = car.type;
            this.inputColor.Text = car.color;
            this.inputLocation.Text = car.location;
            this.inputEnginePower.Text = car.engine_power.ToString();
            this.inputEngineConsump.Text = car.engine_fuel_consumption.ToString();
            this.inputPricePerDay.Text = car.price_per_day.ToString();
            this.inputConditioner.Checked = car.conditioner;
        }
    }
}