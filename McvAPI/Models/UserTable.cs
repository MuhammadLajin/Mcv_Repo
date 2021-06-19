using System;
using System.Collections.Generic;

#nullable disable

namespace McvAPI.Models
{
    public partial class UserTable
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string TelephoneNumber { get; set; }
        public string Address { get; set; }
        public string BirthDate { get; set; }
        public string Email { get; set; }
    }
}
