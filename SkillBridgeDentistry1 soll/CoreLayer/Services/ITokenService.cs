using CoreLayer.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Serveses
{
   public interface ITokenService
    {
        Task<string> CreateTokenAsync(ApplicationUser User, UserManager<ApplicationUser> userManager);

    }
}
