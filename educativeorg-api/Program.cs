//using educativeorg_data.Data;
//using educativeorg_models.Models;
//using educativeorg_models.ViewModels;
using educativeorg_api.Helper;
using educativeorg_data.Data;
using educativeorg_models.Models;
using educativeorg_models.ViewModels;
using educativeorg_services.Services.AccountServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

var config = new AppConfigViewModel();
builder.Configuration.Bind("Configs", config);

// Add services to the container.

builder.Services.ConfigureDbContext(builder.Configuration.GetConnectionString("BbConnection")!);

builder.Services.ConfigureIdentity();

builder.Services.ConfigureJWT();

builder.Services.AddAuthorization();

builder.Services.ConfigureCors(config);

builder.Services.ConfigureControllers();

builder.Services.ConfigureSwagger();


builder.Services.AddAutoMapper(typeof(Program));

builder.Services.ConfigureServices();

var app = builder.Build();

app.UseExceptionHandler("/exceptionhandler");

if (app.Environment.IsDevelopment())
{
    
}


app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
