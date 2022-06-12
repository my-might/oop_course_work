namespace ClassLib
{
    public class CarRepo
    {
        private ServiceContext context;
        public CarRepo(ServiceContext context)
        {
            this.context = context;
        }
    }
}