using IdentityModel.Client;
using IdentityServerExample.Client1.Models;
using IdentityServerExample.Client1.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace IdentityServerExample.Client1.Controllers;
[Authorize]
public class ProductController : Controller
{
    private readonly IConfiguration configuration;
    private readonly IApiResourceHttpClient apiResourceHttpClient;

    public ProductController(IConfiguration configuration,IApiResourceHttpClient apiResourceHttpClient)
    {
        this.configuration = configuration;
        this.apiResourceHttpClient = apiResourceHttpClient;
    }
    public async Task< IActionResult> Index()
    {
        var httpClient= await apiResourceHttpClient.GetHttpClientAsync();
    
        
        HttpResponseMessage response = await httpClient.GetAsync("https://localhost:7010/api/products/getproducts");
        if (response.IsSuccessStatusCode)
        {
            var content= await response.Content.ReadAsStringAsync();
            List<Product> products = new();
             products = JsonConvert.DeserializeObject<List<Product>>(content);

            return View(products);
        }
        else
        {
//loglama
        }

        return View();
    }
}
