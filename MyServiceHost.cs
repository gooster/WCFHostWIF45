using System;
using System.Collections.Generic;
using System.IdentityModel.Configuration;
using System.IdentityModel.Services;
using System.IdentityModel.Services.Configuration;
using System.Net;
using System.Security.Claims;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace WCFHostWIF45
{
    class MyServiceHost : ServiceHost
    {
        public MyServiceHost()
            : base(typeof(GradeServiceFacade), new Uri("http://localhost:8081"))
        { }

        protected override void OnOpening()
        {
            AddServiceEndpoint(typeof(IGradeService), new BasicHttpBinding(), "");

            var smb = Description.Behaviors.Find<ServiceMetadataBehavior>();
            if (smb == null)
            {
                smb = new ServiceMetadataBehavior();
                Description.Behaviors.Add(smb);
            }

            smb.HttpGetEnabled = true;
            smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;

            AddServiceEndpoint(ServiceMetadataBehavior.MexContractName, MetadataExchangeBindings.CreateMexHttpBinding(), "mex");

            // only way (in code) to hook up our own IdentityConfiguration and ClaimsAuthorizationManager
            FederatedAuthentication.FederationConfigurationCreated += (sender, args) =>
                {
                    args.FederationConfiguration = new MyFederationConfiguration();
                };

            var serviceAuthorizationBehavior = Description.Behaviors.Find<ServiceAuthorizationBehavior>();
            serviceAuthorizationBehavior.PrincipalPermissionMode = PrincipalPermissionMode.None;
            var sm = new MyServiceAuthorizationManager();
            serviceAuthorizationBehavior.ServiceAuthorizationManager = sm;

            var serviceAuthenticationBehavior = Description.Behaviors.Find<ServiceAuthenticationBehavior>();
            serviceAuthenticationBehavior.AuthenticationSchemes = AuthenticationSchemes.None;
            serviceAuthenticationBehavior.ServiceAuthenticationManager = new MyServiceAuthenticationManager();

            base.OnOpening();
        }
    }

    class MyFederationConfiguration : FederationConfiguration
    {
        public MyFederationConfiguration() : base(false) { }

        public override void Initialize()
        {
            IdentityConfiguration = new IdentityConfiguration
                {
                    ClaimsAuthorizationManager = new MyClaimsAuthorizationManager(),
                };
            IsInitialized = true;
        }
    }

    class MyClaimsIdentity : ClaimsIdentity
    {
        public MyClaimsIdentity(IEnumerable<Claim> claims)
            : base(claims)
        {

        }
        public override string Name
        {
            get
            {
                return string.Format("{0} {1}", FindFirst(ClaimTypes.GivenName).Value, FindFirst(ClaimTypes.Surname).Value);
            }
        }

        public override bool IsAuthenticated { get { return true; } }
    }

    namespace Security.Claims
    {
        public static class ClaimTypes { public const string Resource = "http://edu.welfarelabs.net/2012/12/identity/claims/resource"; }
    }
}
