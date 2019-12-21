using System;

namespace Store.Model
{
    public class Human
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? Birthday { get; set; }

        public bool IsFriend { get; set; }
    }
}