using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoListWebApi.Contracts;
using TodoListWebApi.Data_Access;
using TodoListWebApi.Services.Repositories;

namespace TodoListWebApi
{
    public class Startup
    {

        private readonly string myPolicy = "myPolicy";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors(options =>
            {
                options.AddPolicy(name: myPolicy,
                    builder =>
                    {
                        var clientSideAddres = Configuration.GetSection("ClientSideAddress").GetValue<string>("Client");
                        builder.WithOrigins(clientSideAddres)
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                    });
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TodoListWebApi", Version = "v1" });
            });

            services.AddScoped<ITodosRepository, TodosDbRepository>();

            services.AddDbContext<TodosDbContext>(option =>
                    option.UseSqlServer(
                        Configuration.GetConnectionString("TodosDb"))
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoListWebApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(myPolicy);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
