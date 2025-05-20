using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PFMS.API.CronJobs;
using PFMS.API.Mappers;
using PFMS.API.Middlewares;
using PFMS.BLL.BOs;
using PFMS.BLL.Interfaces;
using PFMS.BLL.Mappers;
using PFMS.BLL.Services;
using PFMS.DAL.Data;
using PFMS.DAL.DTOs;
using PFMS.DAL.Entities;
using PFMS.DAL.Interfaces;
using PFMS.DAL.Mapper;
using PFMS.DAL.Repositories;
using PFMS.DAL.UnitOfWork;
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

builder.Services.AddHostedService<SendTransactionNotificationJob>();
builder.Services.AddHostedService<RecurringTransactionJob>();

builder.Services.Configure<SmtpSettingsBo>(builder.Configuration.GetSection("SmtpSettings"));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IRolesService, RolesService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IOneTimePasswordsService, OneTimePasswordsService>();
builder.Services.AddScoped<IBudgetsService, BudgetsService>();
builder.Services.AddScoped<IPermissionsService, PermissionsService>();
builder.Services.AddScoped<ITransactionNotificationsService, TransactionNotificationsService>();
builder.Services.AddScoped<IRecurringTransactionsService, RecurringTransactionsService>();

builder.Services.AddScoped<IUserRepository<UserDto>, UserRepository<UserDto, User>>();
builder.Services.AddScoped<ITransactionRepository<TransactionDto>, TransactionRepository<TransactionDto, Transaction>>();
builder.Services.AddScoped<ICategoryRepository<TransactionCategoryDto>, CategoryRepository<TransactionCategoryDto , TransactionCategory>>();
builder.Services.AddScoped<IRolesRepository<RoleDto>, RolesRepository<RoleDto, Role>>();
builder.Services.AddScoped<IOneTimePasswordsRespository<OneTimePasswordDto>, OneTimePasswordsRepository<OneTimePasswordDto, OneTimePassword>>();
builder.Services.AddScoped<IBudgetsRepository<BudgetDto>, BudgetsRepository<BudgetDto, Budget>>();
builder.Services.AddScoped<ITotalTransactionAmountRespository<TotalTransactionAmountDto>, TotalTransactionAmountRepository<TotalTransactionAmountDto, TotalTransactionAmount>>();
builder.Services.AddScoped<ITransactionNotificationsRepository<TransactionNotificationDto>, TransactionNotificationsRepository<TransactionNotificationDto, TransactionNotification>>();
builder.Services.AddScoped<IRecurringTransactionsRepository<RecurringTransactionDto>, RecurringTransactionsRepository<RecurringTransactionDto, RecurringTransaction>>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

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

builder.Services.AddAutoMapper(typeof(TransactionScreenshotBLLMapper));
builder.Services.AddAutoMapper(typeof(TransactionScreenshotDALMapper));

builder.Services.AddAutoMapper(typeof(PermissionBLLMapper));
builder.Services.AddAutoMapper(typeof(PermissionDALMapper));

builder.Services.AddAutoMapper(typeof(TransactionNotificationMapper));
builder.Services.AddAutoMapper(typeof(TransactionNotificationBLLMapper));
builder.Services.AddAutoMapper(typeof(TransactionNotificationDALMapper));

builder.Services.AddAutoMapper(typeof(RecurringTransactionMapper));
builder.Services.AddAutoMapper(typeof(RecurringTransactionBLLMapper));
builder.Services.AddAutoMapper(typeof(RecurringTransactionDALMapper));


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