using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
{
    opt.Authority = "https://localhost:7009";//Benim public keyimi kim yayýnlýyor?
    opt.Audience = "resource_api1";//Baþta tanýmladýðým resourcelerle ayný isimde olmalý.Gelen tokenda audience kýsmýnda bu isim olmalý kesinlikle doðrulamam için. 
});

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("ReadProduct", policy =>
    {
        policy.RequireClaim("scope", "api1.read");
    });
    opt.AddPolicy("UpdateOrCreate", policy =>
    {
        policy.RequireClaim("scope", new[] { "api1.update", "api1.create" });
    });
});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
