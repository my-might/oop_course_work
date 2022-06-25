using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
namespace ClassLib
{
    public class UserRepo : UserAbstract
    {
        private ServiceContext context;
        public UserRepo(ServiceContext context)
        {
            this.context = context;
        }
        public override int Insert(User user)
        {
            context.users.Add(user);
            context.SaveChanges();
            return user.id;
        }
        public override void Update(User client)
        {
            var local = context.users.Find(client.id);
            if (local != null)
            {
                context.Entry(local).State = EntityState.Detached;
            }
            context.Entry(client).State = EntityState.Modified;
            context.SaveChanges();
        }
        public override void DeleteById(int id)
        {
            context.users.Remove(context.users.Find(id));
            context.SaveChanges();
        }
        public override User GetById(int id)
        {
            User result = context.users.Find(id);
            return result;
        }
        public override User GetByLogin(string login)
        {
            User result = context.users.Where(u => u.login == login).First();
            return result;
        }
    }
}