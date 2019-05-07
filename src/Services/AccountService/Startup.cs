using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AccountService.Domain.Write.Repositories;
using AccountService.Domain.Write.Services;
using AccountService.Infrastructure.Helpers;
using AccountService.Infrastructure.Persistence;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AccountService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc();
            services.AddDbContext<DataContext>(x => x.UseInMemoryDatabase("TestDb"));
            services.AddAutoMapper();
            
            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            
            
            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
                {
                    x.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                            var userId = Guid.Parse(context.Principal.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault());
                            var user = userService.GetById(userId);
                            if (user == null)
                            {
                                // return unauthorized if user no longer exists
                                context.Fail("Unauthorized");
                            }
                            return Task.CompletedTask;
                        }
                    };
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            
            // configure DI for application services
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddSingleton<RabbitListener>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

           /* app.UseRouter(routes =>
            {
                routes.MapControllers();
            });*/

            app.UseAuthentication();
            
            
            app.UseRabbitListener();


            app.UseMvc();
        }
    }
        
        public static class ApplicationBuilderExtentions
        {
            //the simplest way to store a single long-living object, just for example.
            private static RabbitListener _listener { get; set; }

            public static IApplicationBuilder UseRabbitListener(this IApplicationBuilder app)
            {
                _listener = app.ApplicationServices.GetService<RabbitListener>();

                var lifetime = app.ApplicationServices.GetService<IApplicationLifetime>();

                lifetime.ApplicationStarted.Register(OnStarted);

                //press Ctrl+C to reproduce if your app runs in Kestrel as a console app
                lifetime.ApplicationStopping.Register(OnStopping);

                return app;
            }

            private static void OnStarted()
            {
                _listener.Register();
            }

            private static void OnStopping()
            {
                _listener.Deregister();    
            }
        }
        
        public class RabbitListener
        {
            /*ConnectionFactory factory { get; set; }
            IConnection connection { get; set; }
            IModel channel { get; set; }

            public RabbitListener()
            {
                this.factory = new ConnectionFactory() { HostName = "localhost" };
                this.connection = factory.CreateConnection();
                this.channel = connection.CreateModel();
            }*/
            
            public void Register()
            {
                for (int i = 0; i < int.MaxValue; i++)
                {
                    Console.WriteLine(i);
                }
                
                /*channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    int m = 0;
                };
                channel.BasicConsume(queue: "hello", autoAck: true, consumer: consumer);*/
            }

            public void Deregister()
            {
                Console.WriteLine("ACABOU");
                /*this.connection.Close();*/
            }
        }
}
