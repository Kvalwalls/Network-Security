namespace CommonUser
{
    public class User
    {
        public string id;
        public string name;
        public string password;
        public string access;
        public float money;

        public User(string id,string name,string password,string access,float money)
        {
            this.id = id;
            this.name = name;
            this.password = password;
            this.access = access;
            this.money = money;
        }

        public User()
        {

        }
        
    }
}
