﻿using Domain.Interfaces;
using Newtonsoft.Json;

namespace Domain.Entities
{
    public class AirlineCompany : IPoco, IUser
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
        public User User { get; set; }

        public AirlineCompany()
        {

        }

        public AirlineCompany(string name, int countryId, User user, long id = 0)
        {
            Name = name;
            CountryId = countryId;
            User = user;
            Id = id;
        }

        public static bool operator ==(AirlineCompany company1, AirlineCompany company2)
        {
            if (ReferenceEquals(company1, null) && ReferenceEquals(company2, null))
                return true;
            if (ReferenceEquals(company1, null) || ReferenceEquals(company2, null))
                return false;

            return company1.Id == company2.Id;
        }
        public static bool operator !=(AirlineCompany company1, AirlineCompany company2)
        {
            return !(company1 == company2);
        }
        public override bool Equals(object obj)
        {
            AirlineCompany company = obj as AirlineCompany;
            return this == company;
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
