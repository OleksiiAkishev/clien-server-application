using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UniversityMgmtSystem.Data;
using UniversityMgmtSystem.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString
    = builder
    .Configuration
    .GetConnectionString("AppDBContextConnection")
    ?? throw new InvalidOperationException("Connection string 'AppDBContextConnection' not found.");
builder
.Services
    .AddDbContext<AppDbContext>(options => options
    .UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
		.AddEntityFrameworkStores<AppDbContext>()
		.AddDefaultTokenProviders();


builder.Services.AddMemoryCache();


builder.Services
				.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.SaveToken = true;
					options.RequireHttpsMetadata = false;
					options.TokenValidationParameters = new TokenValidationParameters()
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidAudience = builder.Configuration["JWT:ValidAudience"],
						ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
					};
				});

builder.Services.AddAuthentication(options =>
options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme
);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
