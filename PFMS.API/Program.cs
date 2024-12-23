using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PFMS.API.Mappers;
using PFMS.API.Middlewares;
using PFMS.BLL.BOs;
using PFMS.BLL.Interfaces;
using PFMS.BLL.Mappers;
using PFMS.BLL.Services;
using PFMS.DAL.Data;
using PFMS.DAL.Interfaces;
using PFMS.DAL.Mapper;
using PFMS.DAL.Repositories;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontendApp", builder =>
    {
        builder.WithOrigins("http://localhost:5173")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});

var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/PfmsLog.txt", rollingInterval: RollingInterval.Day)
    .MinimumLevel.Error()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddHttpContextAccessor();

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("pfmsDb"))
);

builder.Services.Configure<SmtpSettingsBo>(builder.Configuration.GetSection("SmtpSettings"));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IRolesService, RolesService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IOneTimePasswordsService, OneTimePasswordsService>();
builder.Services.AddScoped<IBudgetsService, BudgetsService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IRolesRepository, RolesRepository>();
builder.Services.AddScoped<IOneTimePasswordsRespository, OneTimePasswordsRepository>();
builder.Services.AddScoped<IBudgetsRepository, BudgetsRepository>();

builder.Services.AddScoped<ITotalTransactionAmountRespository, TotalTransactionAmountRepository>();

builder.Services.AddAutoMapper(typeof(UserMapper));
builder.Services.AddAutoMapper(typeof(UserBLLMapper));
builder.Services.AddAutoMapper(typeof(UsersDALMapper));

builder.Services.AddAutoMapper(typeof(TotalTransactionAmountBLLMapper));
builder.Services.AddAutoMapper(typeof(TotalTransactionAmountDALMapper));
builder.Services.AddAutoMapper(typeof(TotalTransactionAmountMapper));

builder.Services.AddAutoMapper(typeof(TransactionMapper));
builder.Services.AddAutoMapper(typeof(TransactionBLLMapper));
builder.Services.AddAutoMapper(typeof(TransactionDALMapper));

builder.Services.AddAutoMapper(typeof(TransactionCategoryMapper));
builder.Services.AddAutoMapper(typeof(TransactionCategoryBLLMapper));
builder.Services.AddAutoMapper(typeof(TransactionCategoryDALMapper));

builder.Services.AddAutoMapper(typeof(RoleMapper));
builder.Services.AddAutoMapper(typeof(RoleBLLMapper));
builder.Services.AddAutoMapper(typeof(RoleDALMapper));

builder.Services.AddAutoMapper(typeof(OneTimePasswordBLLMapper));
builder.Services.AddAutoMapper(typeof(OneTimePasswordDALMapper));

builder.Services.AddAutoMapper(typeof(BudgetMapper));
builder.Services.AddAutoMapper(typeof(BudgetBLLMapper));
builder.Services.AddAutoMapper(typeof(BudgetDALMapper));


builder.Services.AddScoped<IPasswordHasher<UserBo>, PasswordHasher<UserBo>>();

builder.Services.AddControllers();
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

// Ensure UseCors is before UseRouting
app.UseCors("AllowFrontendApp");

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseMiddleware<AuthMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();