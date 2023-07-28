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

// Configure app via custom extension
app.ConfigureApp(container);

// Initialize migrations
await app.MigrateDatabase();

// Run app async
await app.RunAsync();
