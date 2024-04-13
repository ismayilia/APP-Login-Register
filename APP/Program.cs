using Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repository.Data;
using Repository.Repositories;
using Repository.Repositories.Interfaces;
using Service.Helpers;
using Service.Mappings;
using Service.Services;
using Service.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Service;
using Repository;
using Microsoft.OpenApi.Models;
using APP.Injections;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// JWT BEARER oldugun bildiririk acar isharesi gelir
builder.Services.AddSwagger();

// JWT istifade etdiyimizi bildirmeliyik

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
builder.Services
	.AddAuthentication(options =>
	{
		options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
		options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
		options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	})
	.AddJwtBearer(cfg =>
	{
		cfg.RequireHttpsMetadata = false;
		cfg.SaveToken = true;
		cfg.TokenValidationParameters = new TokenValidationParameters
		{
			ValidIssuer = builder.Configuration["JWTSettings:Issuer"],
			ValidAudience = builder.Configuration["JWTSettings:Issuer"],
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTSettings:Key"])),
			ClockSkew = TimeSpan.Zero // remove delay of token when expire
		};
	});

// APILOGIN

builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("JWTSettings"));


builder.Services.AddDbContext<AppDbContext>(options =>
		options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//APILOGIN
builder.Services.AddIdentity<AppUser, IdentityRole>()
				.AddEntityFrameworkStores<AppDbContext>();

//APILOGIN

builder.Services.Configure<IdentityOptions>(option =>
{
	option.Password.RequireNonAlphanumeric = true; //simvol olab biler
	option.Password.RequireDigit = true; //reqem olmalidir
	option.Password.RequireLowercase = true; //balaca herf olmalidir
	option.Password.RequireUppercase = true; //boyuk olmalidir
	option.Password.RequiredLength = 6; //minimum 6 

	option.User.RequireUniqueEmail = true;
	//Default lockout  settings

	option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
	option.Lockout.MaxFailedAccessAttempts = 5;
	option.Lockout.AllowedForNewUsers = true;
});

//repo ve service gore scop olanlar---------------------------------------------

builder.Services.AddServiceLayer();
builder.Services.AddRepositoryLayer();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
