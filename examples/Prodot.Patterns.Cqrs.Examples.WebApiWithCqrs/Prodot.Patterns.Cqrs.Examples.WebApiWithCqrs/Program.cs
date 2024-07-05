using Prodot.Patterns.Cqrs.Examples.WebApiWithCqrs.Business;
using Prodot.Patterns.Cqrs.Examples.WebApiWithCqrs.DataLayer;
using Prodot.Patterns.Cqrs.MicrosoftExtensionsDependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// register CQRS services, query handlers and pipelines
builder.Services.AddProdotPatternsCqrs(options =>
    options
        .WithQueryHandlersFrom(
            typeof(BusinessLayerPipelineProfile).Assembly,
            typeof(DataLayerPipelineProfile).Assembly
        )
        .WithQueryHandlerPipelineConfiguration(config =>
            config
                .WithPipelineAutoRegistration() // using this, we can skip registration if there is an unambiguous mapping between query and handler
                .AddProfiles(
                    typeof(BusinessLayerPipelineProfile).Assembly,
                    typeof(DataLayerPipelineProfile).Assembly
                )
        )
);

// register other services
builder.Services.AddControllers();

// further application setup
var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
