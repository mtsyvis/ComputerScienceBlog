using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ComputerScienceBlogBackEnd.DataAccess;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ComputerScienceBlogBackEnd.Helpers;
using ComputerScienceBlogBackEnd.Services.UserManagement;
using System.Text;
using ComputerScienceBlogBackEnd.Repositories;
using ComputerScienceBlogBackEnd.Services.ArticleManagement;
using ComputerScienceBlogBackEnd.Infrastructure.Filters;
using FluentValidation.AspNetCore;
using FluentValidation;
using ComputerScienceBlogBackEnd.DataAccess.Validators;

namespace ComputerScienceBlogBackEnd
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
            services.AddCustomMVC(Configuration)
                .AddSwager(Configuration)
                .AddJwtAuthentication(Configuration);

            services.Configure<ComputerScienceBlogDatabaseSettings>(Configuration.GetSection(nameof(ComputerScienceBlogDatabaseSettings)));
            services.AddSingleton<IComputerScienceBlogDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<ComputerScienceBlogDatabaseSettings>>().Value);

            // configure DI for application services
            services.AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<IArticleService, ArticleService>();
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

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                    c.RoutePrefix = string.Empty;
                });

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "swager");
            });
        }
    }

    public static class CustomExtensionMethods
    {

        public static IServiceCollection AddCustomMVC(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            })
                .AddJsonOptions(options => options.UseMemberCasing())
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddFluentValidation();

            services.AddTransient<IValidator<Article>, ArticleValidator>()
                .AddTransient<IValidator<Comment>, CommentValidator>();

            return services;
        }

        public static IServiceCollection AddSwager(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "Computer Science Blog", Version = "v1" });
            });

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            // configure strongly typed settings objects
            var appSettingsSection = configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
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

            return services;
        }
    }
}
