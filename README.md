# Protector Auth Token
Simple token authentication for .NET Core API and MVC projects. It looks for an access token in the request header under the "ProtectorToken" key and verifies its validity when accessing a controller or an action tagged with the [Protect] attribute.

This project uses Microsoft's Data Protection library (https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/introduction?view=aspnetcore-6.0) to secure the access token.

Usage:

1. Add the ProtectorTokenAuth (https://www.nuget.org/packages/ProtectorTokenAuth/) Nuget package to your .NET Core API or MVC project.
2. Enable it in your project's Startup.cs:

```cs
public void ConfigureServices(IServiceCollection services) {
  services.AddProtectorAuth();
}
```
`NOTE: It is recommended to use the AddProtectorAuthWithOptions() method and specify your own unique provider name so that tokens issued from other systems won't be valid on yours.`

3. Inject the IAuthService authentication service into any controllers or services you wish to create or validate access tokens and hash passwords:

```cs
public class MyController : ControllerBase 
{
  public MyController(IAuthService authService) 
  {
    this.authService = authService;
  }
}
```
4. Issue a token on successful login like so:

```cs
public string YourLoginMethod()
{
  // ... login functionality here
  
  return authService.CreateAccessToken(new TokenPayload
  {
    Username = username,
    UserRoles = new string[] { roleName },
    CustomData = new Dictionary<string,string>()
  });
}
```

5. Protect your controllers or actions using the [Protect] attribute. You can also specify which roles the attribute should allow.

```cs
[Protect]
public StatusMessageVM SomeMethod() {
  // ...
}

[Protect("Admin")]
public StatusMessageVM AdminOnlyMethod() {
  // ...
}


[Protect("Admin", "Moderator")]
public StatusMessageVM AdminAndModeratorOnlyMethod() {
  // ...
}
```

6. You can access a user's name, roles and custom data everywhere the IAuthService is injected by looking at `authService.ActiveUser`, `authService.ActiveUserRoles` and `authService.ActiveUserData`

7. You can use the `HashPassword` of the `AuthService` to hash passwords before storing them to your database. It has an optional salt parameter for further security.
