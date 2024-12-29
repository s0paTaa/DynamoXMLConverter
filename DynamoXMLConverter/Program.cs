using DynamoXMLConverter.Infrastructure.Database.Extensions;
using DynamoXMLConverter.Infrastructure.Extensions;
using SimpleInjector;

// Initialize SimpleInjector container
Container container = new Container();

// Create app builder
var builder = WebApplication.CreateBuilder(args);

// Register services via custom extension
builder.Services.RegisterAppServices(container, builder.Configuration);

// Build the application
var app = builder.Build();

// Initialize migrations
await app.MigrateDatabase();

// Configure app via custom extension
app.ConfigureApp(container);

// Run app async
await app.RunAsync();
