using Presentation_Tier.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JWT setting 
builder.Services.AddJWTServices();

// Register DI 
builder.Services.AddInternalServices();
builder.Services.AddThirdPartyServices();
builder.Services.AddOrdinaryServices();

//  Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVercelFrontend", policy =>
    {
        policy.WithOrigins("https://manage-it-nine.vercel.app")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

//  Use the CORS policy
app.UseCors("AllowVercelFrontend");

app.UseHttpsRedirection();

app.UseWebSockets(); // enable WebSocket

app.UseMiddleware<WebSocketJwtMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapWebSocketEndpoint();
app.MapControllers();

app.Run();
