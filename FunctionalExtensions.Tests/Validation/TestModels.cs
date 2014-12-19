﻿using System.Collections.Generic;

namespace FunctionalExtensions.Tests.Validation
{
    internal class Address
    {
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public string Postcode { get; set; }
    }

    internal class Order
    {
        public string ProductName { get; set; }
        public decimal? Cost { get; set; }
    }

    internal class Customer
    {
        public int Id { get; set; }
        public string Surname { get; set; }
        public string Forename { get; set; }
        public string Gender { get; set; }
        public decimal Discount { get; set; }
        public Address Address { get; set; }
        public IList<Order> Orders { get; set; }
    }
}
