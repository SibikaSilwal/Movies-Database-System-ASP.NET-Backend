
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.APIBehavior;
using MoviesAPI.Filters;
using MoviesAPI.Helpers;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace MoviesAPI;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), 
        sqlOptions => sqlOptions.UseNetTopologySuite()));

        services.AddAutoMapper(typeof(Startup));

        services.AddSingleton(provider => new MapperConfiguration(config =>
        {
            var geometryFactory = provider.GetRequiredService<GeometryFactory>();
            config.AddProfile(new AutoMapperProfiles(geometryFactory));
        }).CreateMapper());

        services.AddSingleton<GeometryFactory>(NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326));



        services.AddControllers(options =>
        {
            options.Filters.Add(typeof(MyExceptionFilter));
            options.Filters.Add(typeof(ParseBadRequest));
        }).ConfigureApiBehaviorOptions(BadRequestBehavior.Parse);

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new() { Title = "MoviesAPI", Version = "v1" });
        });

        services.AddCors(options =>
        {
            var frontendURL = Configuration.GetValue<string>("frontend_url");
            options.AddDefaultPolicy(builder =>
            {
                builder.WithOrigins(frontendURL).AllowAnyMethod().AllowAnyHeader()
                .WithExposedHeaders(new string[] { "totalAmountOfRecords" });
            });
        });

        services.AddScoped<IFileStorageService, AzureStorageService>(); 
    }


    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Configure the HTTP request pipeline.
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/Swagger/v1/swagger.json", "MoviesAPI v1"));
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseCors();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
