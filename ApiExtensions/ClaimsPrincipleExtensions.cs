using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.ApiExtensions
{
    public static class ClaimsPrincipalExtensions
    {

         public static string getEmailfromPrincipleClaims (this ClaimsPrincipal user){
               
               if (user == null)
                throw new ArgumentNullException(nameof(user));

                var email = user.FindFirstValue(ClaimTypes.Email);
               
            return email;

         }
        
    }
}