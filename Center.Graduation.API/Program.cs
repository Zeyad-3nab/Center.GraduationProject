
using Center.Graduation.API.Extension;
using Center.Graduation.API.Helper;
using Center.Graduation.Repository.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Center.Graduation.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

            var builder = WebApplication.CreateBuilder(args);

            //Allow all people
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.WithOrigins("http://localhost:3000" , "https://tumortraker12.runasp.net/" , "http://tumortraker12.runasp.net/");
                                      policy.AllowAnyMethod();
                                      policy.AllowAnyHeader();
                                      policy.AllowCredentials();
                                  });
            });

            // Add services to the container.
            builder.Services.AddSignalR();

            builder.Services.AddControllers()
                  .AddJsonOptions(options =>
                  {
                      options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); // ·ﬁ—«¡… «·‹ Enum ﬂ‹ String/Number
                      options.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
                  });


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));   //Scoped object ber request
            });


            builder.Services.AddApplicationServices(builder.Configuration);

            //Add Swagger Extention
            builder.Services.AddSwaggerGenJwtAuth();

            //Add Custom Extention
            builder.Services.AddCustomJwtAuth(builder.Configuration);


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
                app.UseSwagger();
                app.UseSwaggerUI();
            //}

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
