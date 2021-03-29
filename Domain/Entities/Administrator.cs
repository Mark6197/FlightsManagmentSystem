using Domain.Interfaces;
using Newtonsoft.Json;

namespace Domain.Entities
{
    public class Administrator : IPoco, IUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Level { get; set; }
        public User User { get; set; }

        public Administrator()
        {

        }

        public Administrator(string firstName, string lastName, int level, User user, int id = 0)
        {
            FirstName = firstName;
            LastName = lastName;
            Level = level;
            User = user;
            Id = id;
        }

        public static bool operator ==(Administrator admin1, Administrator admin2)
        {
            if (ReferenceEquals(admin1, null) && ReferenceEquals(admin2, null))
                return true;
            if (ReferenceEquals(admin1, null) || ReferenceEquals(admin2, null))
                return false;

            return admin1.Id == admin2.Id;
        }
        public static bool operator !=(Administrator admin1, Administrator admin2)
        {
            return !(admin1 == admin2);
        }

        public override bool Equals(object obj)
        {
            Administrator admin = obj as Administrator;
            return this == admin;
        }

        public override int GetHashCode()
        {
            return this.Id;
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
