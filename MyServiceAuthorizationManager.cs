using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;

namespace WCFHostWIF45
{
    internal class MyServiceAuthorizationManager : ServiceAuthorizationManager
    {
        // We will always get here before we executing the service facade
        public override bool CheckAccess(OperationContext operationContext, ref Message message)
        {
            // This service is for techers only
            // The function will look at all the claims of type http://schemas.microsoft.com/ws/2008/06/identity/claims/role

            return Thread.CurrentPrincipal.IsInRole("Teacher");
        }
    }
}
