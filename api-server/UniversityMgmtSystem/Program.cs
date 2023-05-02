using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
					var serverSecret =
					new SymmetricSecurityKey
					(Encoding.UTF8.GetBytes
					(builder.Configuration
					["JWT:ServerSecret"]));
					options.TokenValidationParameters = new TokenValidationParameters
					{
						IssuerSigningKey = serverSecret,
						ValidIssuer = builder.Configuration["JWT:Issuer"],
						ValidAudience = builder.Configuration["JWT:Audience"]
					};
				});

builder.Services.AddAuthentication(options => 
options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme);


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
