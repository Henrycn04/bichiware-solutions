using backend.Application;
using backend.Infrastructure;
using backend.Queries;
using backend.Domain;
using backend.Services;
using Microsoft.Extensions.Configuration;
using backend.Commands;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin();
                          policy.AllowAnyMethod();
                          policy.AllowAnyHeader();
                      });
});

// Adding Mail service
builder.Services.Configure<MailConfiguration>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IMailService, MailService>();

// Adding cleanup service to mantain database free of expiered deliverys
builder.Services.AddHostedService<DeliveryCleanUpService>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    return new DeliveryCleanUpService(
        provider.GetRequiredService<ILogger<DeliveryCleanUpService>>(),
        configuration.GetConnectionString("BichiwareSolutionsContext")
    );
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<SearchDeliveryHandler>();
builder.Services.AddScoped<SearchDeliveryQuery>();
builder.Services.AddScoped<UpdateDeliveryHandler>();
builder.Services.AddScoped<SearchProductQuery>();
builder.Services.AddScoped<SearchProductHandler>();
builder.Services.AddScoped<UpdateProductHandler>();
builder.Services.AddTransient<UpdateDeliveryCommand>();
builder.Services.AddTransient<UpdateProductCommand>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// CORS allowed
app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
