namespace IdentityServerExample.Client1.Services;

public interface IApiResourceHttpClient
{
    Task<HttpClient> GetHttpClientAsync();
}
