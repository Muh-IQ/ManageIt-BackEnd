using Business_Tier;
using Business_Tier.Cloudinary_Service;
using Business_Tier.Email_Service;
using Business_Tier.Internal_service.Auths_Service;
using Business_Tier.Internal_service.Cryptography_service;
using Data_Access_Tier;
using Interface_Tier.Repository;
using Interface_Tier.Service;
using Interface_Tier.Service.Internal_service.Auths_Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// register DI Internal service
builder.Services.AddTransient<ICryptographyService, CryptographyService>();
builder.Services.AddTransient<IAccessTokenService, AccessTokenService>();
builder.Services.AddTransient<IRefreshTokenService, RefreshTokenService>();
builder.Services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();

// register DI Third-party services
builder.Services.AddTransient<IEmailService, SendGridService>();
builder.Services.AddTransient<IImageService, CloudinaryService>();



//----------------------------------\\

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IUserRepository, UserRepository>();


    
;builder.Services.AddTransient<IOTPService, OTPService>();
builder.Services.AddTransient<IOTPRepository, OTPRepository>();




var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseSwagger();
app.UseSwaggerUI();
///
///
//////


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
