using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BmsKhameleon.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace BmsKhameleon.Core.Domain.IdentityEntities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
    }
}
