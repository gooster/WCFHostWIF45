
Hi everybody,

I made this sample application to learn how to wire up authentication and authorization for WCF services and business functionality.

To call the service I suggest the use of WcfTestClient.exe which can be found in ..\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\IDE

Key features:

- Separation of access control to a service operation and to a business operation.
  - Service operation access control through the ServiceAuthorizationManager.
  - Business operation access control through the ClaimsAuthorizationManager.

- Hooking up a ServiceAuthenticationManager.
  - Every call to the service will first go to the ServiceAuthenticationManager.Authenticate method where the listen uri
    and the message can be evaluated for authentication.
  -	This is also a good place to create and setup the principal together with its claims.

- Hooking up a ServiceAuthorizationManager.
  - This is where the call will end up after the authentication.
  - Here we can evaluate the OperationContext and the Message to grant access to a specific service operation.

- Hooking up a ClaimsAuthorizationManager to be able to grant access to business logic through a declarative approach
  using ClaimsPrincipalPermissionAttribute.
  - Here is where access to actions on specific resources is granted.
  - The ClaimsAuthorizationManager.CheckAccess is called with an AuthorizationContext where information about the action,
    the resource and the principal with its claims can be found.
	Together with the authorization policy of your choice there should be enough information to decide if access should be granted.

- Show different ways to do access checks using ClaimsPrincipalPermission and ClaimsPrincipalPermissionAttribute.
