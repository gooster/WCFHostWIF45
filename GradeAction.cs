using System;
using System.IdentityModel.Services;
using System.Security.Permissions;

namespace WCFHostWIF45
{
    internal class GradeAction
    {
        public string GetGrade(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        [ClaimsPrincipalPermission(SecurityAction.Demand, Operation = "read", Resource = "Grade")]
        public string ProtectedGetGrade(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        [ClaimsPrincipalPermission(SecurityAction.Demand, Operation = "read", Resource = "Grade")]
        [ClaimsPrincipalPermission(SecurityAction.Demand, Operation = "write", Resource = "Grade")]
        internal void ProtectedWriteGrade(int value)
        {
            Console.WriteLine(value);
        }
    }
}
