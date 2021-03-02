using Domain.Interfaces;
using Newtonsoft.Json;

namespace Domain.Entities
{
    public class Customer : IPoco, IUser
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string CreditCardNumber { get; set; }
        public User User { get; set; }

        public Customer()
        {

        }

        public Customer(string firstName, string lastName, string address, string phoneNumber, string creditCardNumber, User user, long id = 0)
        {
            FirstName = firstName;
            LastName = lastName;
            Address = address;
            PhoneNumber = phoneNumber;
            CreditCardNumber = creditCardNumber;
            User = user;
            Id = id;
        }

        public static bool operator ==(Customer customer1, Customer customer2)
        {
            if (ReferenceEquals(customer1, null) && ReferenceEquals(customer2, null))
                return true;
            if (ReferenceEquals(customer1, null) || ReferenceEquals(customer2, null))
                return false;

            return customer1.Id == customer2.Id;
        }
        public static bool operator !=(Customer customer1, Customer customer2)
        {
            return !(customer1 == customer2);
        }
        public override bool Equals(object obj)
        {
            Customer customer = obj as Customer;
            return this == customer;
        }

        public override int GetHashCode()
        {
            return (int)this.Id;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
