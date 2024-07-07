using ServiceCreditValidation.Services;
using ServiceCreditValidation.Validators;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddSingleton<ICreditService, CreditService>();
builder.Services.AddTransient<CreditRequestValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.MapControllers();

app.Run();
