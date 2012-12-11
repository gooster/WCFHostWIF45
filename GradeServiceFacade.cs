using System;
using System.Security;
using System.Security.Claims;
using System.IdentityModel.Services;
using System.Threading;

namespace WCFHostWIF45
{
    class GradeServiceFacade : IGradeService
    {
        public string GetGrade(int value)
        {
            // 
            // Method 1. Simple access check using static method.  
            // Expect this to be most common method. 
            //
            ClaimsPrincipalPermission.CheckAccess("Grade", "read");
            string result = new GradeAction().GetGrade(value);
            Console.WriteLine(result);

            // 
            // Method 2. Programmatic check using the permission class 
            // Follows model found at http://msdn.microsoft.com/en-us/library/system.security.permissions.principalpermission.aspx 
            //
            var cpp = new ClaimsPrincipalPermission("Grade", "read");
            cpp.Demand();
            result = new GradeAction().GetGrade(value);
            Console.WriteLine(result);

            // 
            // Method 3. Access check interacting directly with the authorization manager. 
            //            
            var am = new ClaimsAuthorizationManager();

            if (!am.CheckAccess(new AuthorizationContext((ClaimsPrincipal)Thread.CurrentPrincipal, "Grade", "read")))
                throw new SecurityException("Access denied");
            result = new GradeAction().GetGrade(value);
            Console.WriteLine(result);

            // 
            // Method 4. Call a method that is protected using the permission attribute class 
            //
            result = new GradeAction().ProtectedGetGrade(value);
            Console.WriteLine(result);

            return result;
        }

        public void WriteGrade(int value)
        {
            new GradeAction().ProtectedWriteGrade(value);
        }
    }
}
