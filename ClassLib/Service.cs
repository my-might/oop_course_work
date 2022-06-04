﻿using Microsoft.EntityFrameworkCore;

namespace Classlib
{
    public class Service
    {
        public CarsRepository carsRepository;
        public EnginesRepository enginesRepository;
        public RentRepository rentRepository;
        public ClientsRepository clientsRepository;
        public WorkersRepository workersRepository;
        private ServiceContext context;
        public Service()
        {
            CreateContext();
            this.carsRepository = new CarsRepository(context);
            this.enginesRepository = new EnginesRepository(context);
            this.rentRepository = new RentRepository(context);
            this.clientsRepository = new ClientsRepository(context);
            this.workersRepository = new WorkersRepository(context);
        }
        public void CreateContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ServiceContext>();
            context = new ServiceContext("Host=localhost;Database=cars_rent;Username=postgres;Password=Lera2003");
            var result = -1;
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "SELECT COUNT(*) FROM pg_publication";
                context.Database.OpenConnection();
                using (var reader = command.ExecuteReader())
                    if (reader.Read())
                    {
                        result = reader.GetInt32(0);
                    }
                context.Database.CloseConnection();
            }

            if (result == 0)
            {
                context.Database.ExecuteSqlRaw("CREATE PUBLICATION logical_pub FOR ALL TABLES;");
            }

            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "SELECT COUNT(*) FROM pg_replication_slots";
                context.Database.OpenConnection();
                using (var reader = command.ExecuteReader())
                    if (reader.Read())
                    {
                        result = reader.GetInt32(0);
                    }
                context.Database.CloseConnection();
            }

            if (result == 0)
            {
                context.Database.ExecuteSqlRaw("SELECT * FROM pg_create_logical_replication_slot('logical_slot', 'pgoutput');");
            }
            replica = new ServiceContext("Host=localhost;Database=new_online_shop;Username=postgres;Password=LeraLera");
            replica.CreateSubscription();
        }
    }
}