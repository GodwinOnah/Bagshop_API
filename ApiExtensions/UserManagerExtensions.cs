

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Identity;
using core.Entities.Identity;

namespace API.ApiErrorMiddleWares
{
    public static class UserManagerExtensions
    {

        // public static async Task<User> FindUserByClaimPrincipleWIthAddress
        // (this UserManager<User> userManager, ClaimsPrincipal user )
        // {
        //      var email = user.FindFirstValue(ClaimTypes.Email);
        //           Console.WriteLine("\n\n\n"+email+1122+"\n\n\n\n\n");
        //         return await userManager.Users.Include(x => x.address)
        //                 .SingleOrDefaultAsync(x => x.Email == email);
        // }

        public static async Task<User> FindByEmailByClaimPrinciple
        (this UserManager<User> userManager, ClaimsPrincipal user )
        {

            if (user == null)
                throw new ArgumentNullException(nameof(user));
            // var email = user.FindFirstValue(ClaimTypes.Email);
                //  Console.WriteLine("\n\n\n"+email+1133+"\n\n\n\n\n");
                return await userManager.Users
                        .SingleOrDefaultAsync(x => x.Email == user.FindFirstValue(ClaimTypes.Email)); //this work
        }    

        public static async Task<User> FindUserByClaimsPrincipleWithAddress
        (this UserManager<User> userManager, ClaimsPrincipal user )
        {

            if (user == null)
                throw new ArgumentNullException(nameof(user));
            // var email = ;
            //      Console.WriteLine("\n\n\n"+email+11433+"\n\n\n\n\n");
                return await userManager.Users.Include(x => x.address)
                        .SingleOrDefaultAsync(x => x.Email == user.FindFirstValue(ClaimTypes.Email)); //this work
        }        
    }
}