using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading;
using System.ComponentModel;

namespace ClassLib
{
    abstract public class UserAbstract {
        abstract public int Insert(User client);
        abstract public void DeleteById(int id);
        abstract public void Update(User client);
        abstract public User GetById(int id);
        abstract public User GetByLogin(string login);
    }
    public class UserProxy : UserAbstract
    {
        private UserAbstract userRepo;
        public UserProxy(ServiceContext service) {
            userRepo = new UserRepo(service);
        }

        public override void DeleteById(int id)
        {
            try
            {
                userRepo.GetById(id);
            }
            catch
            {
                throw new Exception("User with entered id does not exist.");
            }
            userRepo.DeleteById(id);
        }

        public override User GetById(int id)
        {
            User result = new User();
            try
            {
                result = userRepo.GetById(id);
            }
            catch
            {
                throw new Exception("User with entered id does not exist.");
            }
            return result;
        }

        public override int Insert(User client)
        {
            if(client.category.Length != 1)
            {
                throw new Exception("Invalid car category format.");
            }
            try
            {
                userRepo.GetByLogin(client.login);
                throw new Exception("User with entered login already exists.");
            }
            catch {}
            if(client.password.Length < 8)
            {
                throw new Exception("Password length must be equal or bigger than 8.");
            }
            int result = userRepo.Insert(client);
            if(result == 0)
            {
                throw new Exception("Something went wrong!");
            }
            return result;
        }

        public override void Update(User client)
        {
            if(client.category.Length != 1)
            {
                throw new Exception("Invalid car category format.");
            }
            try
            {
                User existing = userRepo.GetByLogin(client.login);
                if(existing.id != client.id)
                {
                    throw new Exception("User with entered login already exists.");
                }
            }
            catch {}
            if(client.password.Length < 8)
            {
                throw new Exception("Password length must be equal or bigger than 8.");
            }
            userRepo.Update(client);
        }
        public override User GetByLogin(string login)
        {
            User result = new User();
            try
            {
                result = userRepo.GetByLogin(login);
            }
            catch
            {
                throw new Exception("User with entered login does not exist.");
            }
            return result;
        }
    }
}