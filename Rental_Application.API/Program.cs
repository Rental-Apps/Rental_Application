using NLog.Extensions.Logging;
using Rental_Appication.BusinessAccessLayer.UserService;
using Rental_Application.DataAccessLayer.DataRepository;
using Rental_Application.DataAccessLayer.LoginLogRepository;
using Rental_Application.DataAccessLayer.LogRepository;
using Rental_Application.DataAccessLayer.UserRepository;
using Rental_Application.IBusinessAccessLayer.IUserService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserDataRepository, UserDataRepository>();
builder.Services.AddScoped<ILoginLogRepository, LoginLogRepository>();
builder.Services.AddScoped<IDapper, DapperContext>();
builder.Services.AddScoped<ITransactionLoggingRepository, TransactionLoggingRepository>();
// Add services to the container.
// Configure NLog
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.SetMinimumLevel(LogLevel.Trace);
});

// Add NLog as the logger provider
builder.Services.AddSingleton<ILoggerProvider, NLogLoggerProvider>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
