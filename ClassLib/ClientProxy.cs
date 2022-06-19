namespace ClassLib
{
    abstract public class UserAbstract {
        abstract public int Insert(User client);
        abstract public void DeleteById(int id);
        abstract public void Update(User client);
        abstract public User GetById(int id);
    }
    public class UserProxy : UserAbstract
    {
        private UserAbstract _clientRepo;
        public UserProxy(ServiceContext service) {
            _clientRepo = new UserRepo(service);
        }

        public override void DeleteById(int id)
        {
            if(GetById(id) != null)
            {
                DeleteById(id);
            }
        }

        public override User GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public override int Insert(User client)
        {
            throw new System.NotImplementedException();
        }

        public override void Update(User client)
        {
            throw new System.NotImplementedException();
        }
    }
}