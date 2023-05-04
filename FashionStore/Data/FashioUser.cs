using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FashionStore.Data
{
    public class FashioUser: IdentityUser
    {
        public string? Name { get; set; }
    }
}
