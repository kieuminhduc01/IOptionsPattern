using IOptionsPattern;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//IOption read
builder.Services.Configure<ApplicationOptions>(
    builder.Configuration.GetSection(nameof(ApplicationOptions)));


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

// IOptions: return value of appsetting at the time app combine that mean when app running and change value inside appsetting.json the new value dont apply
//           because it config as a Singleton an it save in caches in life time of application
// IOptionsSnapshot: return lastest value of appsetting.json even if we change value of appsetting.json when application running. because it config as a scope service
//                   it going to be resoled once inside of current application scope
// IOptionsMonitor: return lastest value of appsetting.json even if we change value of appsetting.json when application running. because it config as a Singleton 
//                  but the value is alway read lastest confign value
        
app.MapGet("options", (IOptions<ApplicationOptions> options,
                       IOptionsSnapshot<ApplicationOptions> optionsSnapshot,
                       IOptionsMonitor<ApplicationOptions> optionsMonitor) =>
{
    var resopnse = new
    {
        OpionsValue = options.Value.ExampleValue,
        SnapshotValue = optionsSnapshot.Value.ExampleValue,
        MonitorValue = optionsMonitor.CurrentValue.ExampleValue
    };
    return Results.Ok(resopnse);
});

app.Run();
