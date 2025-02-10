using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductWebAPI2025.DataLayer
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
    }
}
