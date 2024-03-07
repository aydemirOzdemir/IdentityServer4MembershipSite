using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Globalization;

namespace IdentityServerExample.Client1.Controllers;
[Authorize]
public class UserController : Controller
{
    public IActionResult Index()
    {

        
        return View();
    }
    public async Task Logout()
    {
        await HttpContext.SignOutAsync("Cookies");//default sema
        await HttpContext.SignOutAsync("oidc");//authserverdan çıkış
    }


    public async Task<IActionResult> GetRefreshToken()
    {
        HttpClient client = new HttpClient();
        DiscoveryDocumentResponse discovery = await client.GetDiscoveryDocumentAsync("https://localhost:7009");
        if(discovery.IsError) 
        { //loglama yap
        }
        var refreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
        RefreshTokenRequest refreshTokenRequest = new RefreshTokenRequest();
        refreshTokenRequest.ClientId = "Client1.Mvc";
        refreshTokenRequest.ClientSecret = "secret";
        refreshTokenRequest.RefreshToken = refreshToken;
        refreshTokenRequest.Address = discovery.TokenEndpoint;
        TokenResponse token =await  client.RequestRefreshTokenAsync(refreshTokenRequest);


        var tokens = new List<AuthenticationToken>
        {
            new AuthenticationToken(){Name=OpenIdConnectParameterNames.IdToken,Value=token.IdentityToken},
            new AuthenticationToken(){Name=OpenIdConnectParameterNames.AccessToken,Value=token.AccessToken},
            new AuthenticationToken(){Name=OpenIdConnectParameterNames.RefreshToken,Value=token.RefreshToken},
            new AuthenticationToken(){Name=OpenIdConnectParameterNames.ExpiresIn,Value=DateTime.UtcNow.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)},
        };

        var authenticationResult = await HttpContext.AuthenticateAsync();
        var properties=authenticationResult.Properties;
        properties.StoreTokens(tokens);
        await HttpContext.SignInAsync("Cookies",authenticationResult.Principal,properties);
return RedirectToAction("Index");
    }
    [Authorize(Roles ="Admin")]
    public IActionResult AdminAction()
    {
        return View();
    }
    [Authorize(Roles = "Customer")]
    public IActionResult CustomerAction()
    {
        return View();
    }


}
