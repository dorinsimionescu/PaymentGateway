﻿using System;
using System.Collections.Generic;

#nullable disable

namespace PaymentGateway.Models
{
    public partial class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Cnp { get; set; }
        public int TypeOfPerson { get; set; }
    }
}
