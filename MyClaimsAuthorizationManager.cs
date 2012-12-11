using System.Linq;
using System.Security.Claims;

namespace WCFHostWIF45
{
    internal class MyClaimsAuthorizationManager : ClaimsAuthorizationManager
    {
        public override bool CheckAccess(AuthorizationContext context)
        {
            // we know that the principal is authenticated in ServiceAuthenticationManager.Authenticate
            
            var action = context.Action.First().Value;
            var resource = context.Resource.First().Value;

            var resourceClaim = context.Principal.FindFirst(
                x => x.Type == Security.Claims.ClaimTypes.Resource && x.Value == resource);
            if (resourceClaim == null)
                return false;

            string temp;
            if (!resourceClaim.Properties.TryGetValue(action, out temp))
                return false;

            return true;
        }
    }
}