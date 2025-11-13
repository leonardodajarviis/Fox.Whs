using Microsoft.EntityFrameworkCore;
using Fox.Whs.Data;
using Fox.Whs.Extensions;
using Fox.Whs.Services;
using Fox.Whs.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerCustomGen();

// Đăng ký SAP Service Layer Authentication Service
builder.Services.AddSapServiceLayer(builder.Configuration);

// Đăng ký JWT Authentication và AuthService
builder.Services.AddAuthServices(builder.Configuration);

// builder.Services.AddOverrideApiBehaviorOptions();

// Đăng ký các service khác
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<BlowingProcessService>();
builder.Services.AddScoped<RewindingProcessService>();
builder.Services.AddScoped<SlittingProcessService>();
builder.Services.AddScoped<PrintingProcessService>();
builder.Services.AddScoped<CuttingProcessService>();
builder.Services.AddScoped<GrainMixingProcessService>();
builder.Services.AddScoped<GrainMixingBlowingProcessService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("AppDbConnection"),
        sqlOptions => sqlOptions.CommandTimeout(30)
    )
);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .SetIsOriginAllowed(_ => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// Thêm Controllers với Filter
builder.Services.AddControllers(options =>
{
    // Thêm filter để kiểm tra Access Token JTI
    options.Filters.Add<ValidateAccessTokenJtiFilter>();
});

var app = builder.Build();

// Thêm Global Exception Handler (đặt ở đầu pipeline)
app.UseGlobalExceptionHandler();

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Thêm Authentication & Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Map controllers nếu bạn đã thêm
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();

app.Run();

