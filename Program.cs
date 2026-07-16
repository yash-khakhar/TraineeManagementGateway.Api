using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
    };
});

builder.Services.AddAuthorization(options =>
{
    
    options.AddPolicy("MentorOnly", policy => policy.RequireRole("MENTOR"));
    
    options.AddPolicy("TraineeOnly", policy => policy.RequireRole("TRAINEE"));

    options.AddPolicy("AdminOnly", policy => policy.RequireRole("ADMIN"));

    options.AddPolicy("AdminOrTrainee", policy => policy.RequireRole("ADMIN", "TRAINEE"));

    options.AddPolicy("AdminOrMentor", policy => policy.RequireRole("ADMIN", "MENTOR"));

    options.AddPolicy("TraineeOrMentor", policy => policy.RequireRole("TRAINEE", "MENTOR"));

    options.AddPolicy("AdminOrTraineeOrMentor", policy => policy.RequireRole("ADMIN", "TRAINEE", "MENTOR"));

});

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));


builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapReverseProxy();

app.MapControllers();

app.Run();
