using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


IdentityModelEventSource.ShowPII = true;
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.Authority = builder.Configuration["Jwt:Authority"];
    //o.Audience = builder.Configuration["Jwt:Audience"];
    o.RequireHttpsMetadata = false;
    o.TokenValidationParameters.ValidAudiences = new[]
    {
        "account",
        "ftc-spa-client",
    };
    o.TokenValidationParameters.ValidIssuers = new[]
    {
        "http://keycloak:8080/auth/realms/master",
        "http://localhost:8080/auth/realms/master",
    };
    o.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = c =>
        {
            c.NoResult();

            c.Response.StatusCode = 500;
            c.Response.ContentType = "text/plain";
            if (builder.Environment.IsDevelopment())
            {
                return c.Response.WriteAsync(c.Exception.ToString());
            }
            return c.Response.WriteAsync("An error occurred processing your authentication.");
        }
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("User", policy => policy.RequireClaim("ftc_roles", "User"));
    options.AddPolicy("Administrator", policy => policy.RequireClaim("ftc_roles", "Administrator"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
