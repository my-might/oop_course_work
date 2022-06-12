namespace ClassLib
{
    abstract public class ClientAbstract {
        abstract public int Insert(Client client);
        abstract public void DeleteById(int id);
        abstract public void Update(Client client);
        abstract public Client GetById(int id);
    }
    public class ClientProxy : ClientAbstract
    {
        private ClientAbstract _clientRepo;
        public ClientProxy(ServiceContext service) {
            _clientRepo = new ClientRepo(service);
        }

        public override void DeleteById(int id)
        {
            throw new System.NotImplementedException();
        }

        public override Client GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public override int Insert(Client client)
        {
            throw new System.NotImplementedException();
        }

        public override void Update(Client client)
        {
            throw new System.NotImplementedException();
        }
    }
}