using Lofty.SchudleTask;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Models;
using Reposatiory;
using System;
using System.IO;
using System.Text;
using WebApis.Filters;
namespace WebApis
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 
        /// </summary>
        public static void Main()
        {
            WebApplicationBuilder appbuilder = WebApplication.CreateBuilder();
            appbuilder.WebHost.ConfigureKestrel((context, options) =>
            {
                options.Limits.MaxRequestBodySize = 157286400;
            });
            #region APP Configurations
            appbuilder.Services.AddDbContext<LoftyContext>(
             options =>
             {
                 options.UseLazyLoadingProxies().UseSqlServer(
                     appbuilder.Configuration.GetConnectionString("LoftyProject"));
             }
            );
            appbuilder.Services.AddIdentity<User, IdentityRole>(
            i =>
            {
                i.User.RequireUniqueEmail = true;
                i.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
            }).AddEntityFrameworkStores<LoftyContext>().AddDefaultTokenProviders();
            appbuilder.Services.AddScoped(typeof(PropertyManager));
            appbuilder.Services.AddScoped(typeof(UserManager));
            appbuilder.Services.AddScoped(typeof(GovernorateManager));
            appbuilder.Services.AddScoped(typeof(CityManager));
            appbuilder.Services.AddScoped(typeof(PropertyImageManager));
            appbuilder.Services.AddScoped(typeof(FacilityManager));
            appbuilder.Services.AddScoped(typeof(UnitOfWork));
            appbuilder.Services.AddScoped(typeof(PropertyFacilityManager));
            appbuilder.Services.AddScoped(typeof(BuyTrackerManager));
            appbuilder.Services.AddScoped(typeof(OrderManager));
            appbuilder.Services.AddScoped(typeof(PropertyUnitPriceManager));
            appbuilder.Services.AddScoped(typeof(PropertyRentalYieldManager));
            appbuilder.Services.AddScoped(typeof(AboutQismaManager));
            appbuilder.Services.AddScoped(typeof(PropertyStatusManager));
            appbuilder.Services.AddScoped(typeof(FAQManager));
            appbuilder.Services.AddScoped(typeof(BlogManager));
            appbuilder.Services.AddScoped(typeof(TeamMeamberManager));
            //appbuilder.Services.AddHostedService<OrderConfirmationBackgroundService>();
            appbuilder.Services.AddControllers(
                con => con.Filters.Add<ExceptionFilter>()
                ).AddNewtonsoftJson(option =>
                {
                    option.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                    option.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });
            appbuilder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
            {
                option.SaveToken = true;
                option.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(
                    appbuilder.Configuration["JWT:Key"]!
                    )),
                    ValidateAudience = false,
                    ValidateIssuer = false,
                };
            });
            appbuilder.Services.AddHttpClient();
            appbuilder.Services.AddCors(option =>
            option.AddPolicy("CorsPolicy", P => P.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
            //appbuilder.Services.AddCors(options =>
            //{
            //    options.AddPolicy("CorsPolicy", builder =>
            //        builder
            //            .WithOrigins("https://www.ph-sportsfest.com")
            //            .AllowAnyHeader()
            //            .AllowAnyMethod()
            //            .AllowCredentials());
            //});
            appbuilder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Lofty APIs", Version = "v1" });
                var xmlFile = "Lofty.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "Enter your token here",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
               {
                 new OpenApiSecurityScheme
                 {
                      Reference = new OpenApiReference
                      {
                          Type = ReferenceType.SecurityScheme,
                          Id = "Bearer"
                      }
                 }, new string[] {}
               }
                });
            });
            #endregion
            #region Middlewares
            WebApplication appstarter = appbuilder.Build();
            var env = appstarter.Environment;
            appstarter.MapDefaultControllerRoute();
            appstarter.UseCors("CorsPolicy");
            appstarter.UseSwagger();
            appstarter.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Lofty APIs V1");
                c.RoutePrefix = string.Empty;
            });
            appstarter.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "Content")),
                RequestPath = "/Content"
            });
            appstarter.UseRouting();
            appstarter.UseAuthentication();
            appstarter.UseAuthorization();
            appstarter.Run();
            #endregion
        }
    }
}