using System;
using System.Collections.Generic;
using System.Web;
using Antlr.Runtime.Tree;
using Connected.Common;
using Infrastructure.WebApps.Common;

namespace Connected.Configuration.WebApp
{
    public class UserClaimsManager : IUserClaimsManager
    {
        private List<string> GetUserClaims()
        {
            //Temp solution ; 
            List<string> userClaims = new List<string>()
            {
                "Sample1Plugin",
                "TestPlugin", 
                "AnotherPlugin", 
                "TestPlugin/TestController",
                "Sample1Plugin/Sample1Controller",
                "TestPlugin/TestController/Index", 
                "TestPlugin/TestController/Delete",
                "Sample1Plugin/Sample1Controller/Index"
            };
            return userClaims;
        }

        public bool CheckUserClaim(string claim)
        {
            //TODO : control if request comes from client or server
            //TODO : if comes from server, set claims all
            
            //Temp solution ->
            try
            {
                var request = HttpContext.Current.Request;

                //TODO : Search sub strings 
                return GetUserClaims().Contains(claim);
            }
            catch (Exception)
            {
                return true;
            }
        }
    }
}