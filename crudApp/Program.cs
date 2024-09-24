using Microsoft.EntityFrameworkCore;
using crudApp.Persistence.Contexts;
using crudApp.Services.ProductService;
using crudApp.Services.AutomationService;


// create builder
var builder = WebApplication.CreateBuilder(args); 

// default services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// add database service with configuration
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// add service
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IAutomationService, AutomationService>();

// build app
var app = builder.Build(); 


// middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); 
app.UseAuthorization(); 
app.MapControllers();

// run app
app.Run(); 


