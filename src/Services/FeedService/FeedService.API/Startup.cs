using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;
using FeedService.Domain.CommandHandlers;
using FeedService.Domain.Repositories;
using FeedService.Infrastructure;
using FeedService.Infrastructure.Broker;
using FeedService.Infrastructure.CQRS;
using FeedService.Infrastructure.InfraServices;
using FeedService.Infrastructure.Persistence;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NHibernate;

namespace FeedService
{
    [ExcludeFromCodeCoverage]
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
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            services.AddControllers().AddNewtonsoftJson(options => {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            services.AddCors();
//            services.AddMvc()

            // configure jwt authentication
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
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
                    OnTokenValidated = context =>  Task.CompletedTask
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
            services.AddMediatR(typeof(Startup));
            services.AddSingleton(x => new NHibernateFactory(appSettings.ConnectionString).CreateSessionFactory());
            services.AddScoped<IUnitOfWork, UnitOfWork>(x => new UnitOfWork(x.GetService<ISessionFactory>().OpenSession()));
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IMediatorHandler, InMemoryBus>();
            services.AddScoped<PostCommandHandler>();
            services.AddScoped<PersonCommandHandler>();
            services.AddHostedService<ConsumeRabbitListenerService>();
            services.AddHttpContextAccessor();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRabbitListenerService, RabbitListenerService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

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

            app.UseRouting();
            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            
//            app.UseRabbitListener();

//            app.UseMvc();
        }
    }
}
