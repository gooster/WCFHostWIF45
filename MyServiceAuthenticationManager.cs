using System;
using System.Collections.ObjectModel;
using System.IdentityModel.Policy;
using System.Security.Claims;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;

namespace WCFHostWIF45
{
    class MyServiceAuthenticationManager : ServiceAuthenticationManager
    {
        public override ReadOnlyCollection<IAuthorizationPolicy> Authenticate(
            ReadOnlyCollection<IAuthorizationPolicy> authPolicy, Uri listenUri, ref Message message)
        {
            // Simulate a principal and assign it to the current thread
            var claims = new[]
                {
                    new Claim(ClaimTypes.GivenName, "Kalle"),
                    new Claim(ClaimTypes.Surname, "Kula"),
                    new Claim(ClaimTypes.Role, "Teacher"),
                };

            var identity = new MyClaimsIdentity(claims);

            var entityClaim = new Claim(Security.Claims.ClaimTypes.Resource, "Grade");
            entityClaim.Properties.Add("read", "");
            identity.AddClaim(entityClaim);

            var principal = new ClaimsPrincipal(identity);

            Thread.CurrentPrincipal = principal;
            
            return authPolicy;
        }
    }
}
