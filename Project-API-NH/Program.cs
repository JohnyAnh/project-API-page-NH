global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Cors;
global using Project_API_NH.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

//global using Project_API_NH.Services.ProductService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<SalesNhContext>();
//builder.Services.AddScoped<IProductService, ProductService>();


builder.Services.AddCors(p => p.AddPolicy("CORS", opt =>
{
    opt.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
//builder.Services.AddSingleton<IAuthorizationHandler, nhomnetapi.Handlers.ValidBirthdayHandler>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SuperAdmin", policy => policy.RequireUserName("rootadmin"));
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("QA", policy => policy.RequireRole("QA"));
    options.AddPolicy("nhanvienbanhang", policy => policy.RequireRole("nhanvienbanhang"));
    options.AddPolicy("nhanvienlapdat", policy => policy.RequireRole("nhanvienlapdat"));
    options.AddPolicy("nhanvienquanly", policy => policy.RequireRole("nhanvienquanly"));




    //options.AddPolicy("Dev", policy => policy.RequireClaim("IT", "Developer"));
    //options.AddPolicy("QA", policy => policy.RequireClaim("IT", "QA"));
    //options.AddPolicy("BA", policy => policy.RequireClaim("IT", "BA"));

    //options.AddPolicy("DEV_QA", policy => policy.RequireAssertion(
    //    context => context.User.HasClaim(claim => claim.Type == "IT"
    //    && (claim.Value == "QA" || claim.Value == "Developer")
    //    )));

    //options.AddPolicy("DEV_ADMIN", policy => policy.RequireAssertion(
    //    context => context.User.HasClaim(claim => claim.Type == "IT" && claim.Value == "Developer")
    //       && context.User.IsInRole("Admin")
    //    ));
    //options.AddPolicy("Auth", policy => policy.RequireAuthenticatedUser());
    //var min = builder.Configuration["ValidYearOld:Min"];
    //options.AddPolicy("ValidYearOld", policy => policy.AddRequirements(
    //    new nhomnetapi.Requirements.YearOldRequirement(Convert.ToInt32(builder.Configuration["ValidYearOld:Min"]),
    //                            Convert.ToInt32(builder.Configuration["ValidYearOld:Max"]))
    //    ));
});












var app = builder.Build();
app.UseCors();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDeveloperExceptionPage();

app.UseCors("CORS");

app.UseHttpsRedirection();

app.MapControllers();

app.UseStaticFiles();

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

app.Run();
