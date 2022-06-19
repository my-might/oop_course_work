﻿using Microsoft.EntityFrameworkCore;

namespace ClassLib
{
    public class Service
    {
        public CarProxy carProxy;
        public RentProxy rentProxy;
        public UserProxy userProxy;
        private ServiceContext context;
        public Service()
        {
            CreateContext();
            this.carProxy = new CarProxy(context);
            this.rentProxy = new RentProxy(context);
            this.userProxy = new UserProxy(context);
        }
        public void CreateContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ServiceContext>();
            context = new ServiceContext("Host=localhost;Database=cars_rent;Username=postgres;Password=Lera2003");
        }
    }
}