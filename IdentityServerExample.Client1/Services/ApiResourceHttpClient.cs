
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace IdentityServerExample.Client1.Services;

public class ApiResourceHttpClient : IApiResourceHttpClient
{
    private  HttpClient httpClient;
    private readonly IHttpContextAccessor httpContextAccessor;

    public ApiResourceHttpClient(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
        httpClient= new HttpClient();
        
    }
    public async Task<HttpClient> GetHttpClientAsync()
    {
        string accessToken = await httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
        httpClient.SetBearerToken(accessToken);
        return httpClient;
    }
}
