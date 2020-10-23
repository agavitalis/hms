using HMS.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HMS.Extensions
{
    public static class JwtRegistrationExtension
    {
        private static string sectionName = "JwtOptions";
        public static void AddJwt(this IServiceCollection services)
        {
            IConfiguration configuration;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }

            var section = configuration.GetSection(sectionName);
            var options = configuration.GetOptions<JwtOptions>(sectionName);
            services.Configure<JwtOptions>(section);
            services.AddSingleton(options);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(opt =>
              {
                  opt.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuer = true,
                      ValidateAudience = options.ValidateAudience,
                      ValidateLifetime = options.ValidateLifetime,
                      ValidateIssuerSigningKey = true,
                      ValidIssuer = options.Issuer,
                      ValidAudience = options.Audience,
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Key))
                  };
              });

        }
    }
}
