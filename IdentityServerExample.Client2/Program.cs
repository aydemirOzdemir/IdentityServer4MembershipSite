using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultScheme = "Cookies";
    opt.DefaultChallengeScheme = "oidc";
}).AddCookie("Cookies", opt =>
{
    opt.AccessDeniedPath = "/Home/AccessDenied";

}).AddOpenIdConnect("oidc", opt =>
{
    opt.SignInScheme = "Cookies";
    opt.Authority = "https://localhost:7009";
    opt.ClientId = "Client2.Mvc";
    opt.ClientSecret = "secret";
    opt.ResponseType = "code id_token";
    opt.GetClaimsFromUserInfoEndpoint = true;//user biligilerini ulaþmayý saðlar. userinfoya istek atar.ve benim User.Claims Propertimin içine yükler.
    opt.SaveTokens = true;//baþarýlý authentication sonu refresh ve accesstokenlarý kaydeder.default false.
    opt.Scope.Add("api1.read");//scopelara bunu da ekle
    opt.Scope.Add("offline_access");//refresh token istediðimi bellirtiyorum.
    opt.Scope.Add("CountryAndCity");//belirli claimleri burada istiyorum
    opt.ClaimActions.MapUniqueJsonKey("Country", "Country");
    opt.ClaimActions.MapUniqueJsonKey("City", "City");//Custom claimler olduðu için country ve cityi maplemem gerek
    opt.Scope.Add("Roles");
    opt.ClaimActions.MapUniqueJsonKey("role", "role");
    opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters { RoleClaimType = "role" };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
