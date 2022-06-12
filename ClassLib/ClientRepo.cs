using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
namespace ClassLib
{
    public class ClientRepo : ClientAbstract
    {
        private ServiceContext context;
        public ClientRepo(ServiceContext context)
        {
            this.context = context;
        }
        public override int Insert(Client client)
        {
            context.clients.Add(client);
            context.SaveChanges();
            return client.id;
        }
        public override void Update(Client client)
        {
            var local = context.clients.Find(client.id);
            if (local != null)
            {
                context.Entry(local).State = EntityState.Detached;
            }
            context.Entry(client).State = EntityState.Modified;
            context.SaveChanges();
        }
        public override void DeleteById(int id)
        {
            context.clients.Remove(context.clients.Find(id));
            context.SaveChanges();
        }
        public override Client GetById(int id)
        {
            Client result = context.clients.Find(id);
            return result;
        }
    }
}