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

    public class Movie
    {
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public int time { get; set; }
        public float comment { get; set; }
        public string picture { get; set; }
        public string description { get; set; }

        public Movie(string id, string name, string type, int time, float comment, string picture, string description)
        {
            this.id = id;
            this.name = name;
            this.type = type;
            this.time = time;
            this.comment = comment;
            this.picture = picture;
            this.description = description;
        }

        public Movie()
        {

        }
    }
}