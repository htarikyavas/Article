using Article.Data;
using Article.Data.Repositories;
using Article.Services.Articles;
using Article.Services.Authors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Xml;

namespace Article
{
    public class Startup
    {
        private readonly IConfiguration _config;
        private SecurityKey _key;
        public Startup(IConfiguration config)
        {
            _config = config;
        }

        public static RSAParameters LoadKey(string xmlString)
        {
            RSAParameters parameters = new RSAParameters();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            if (xmlDoc.DocumentElement.Name.Equals("RSAKeyValue"))
            {
                foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "Modulus": parameters.Modulus = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "Exponent": parameters.Exponent = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "P": parameters.P = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "Q": parameters.Q = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "DP": parameters.DP = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "DQ": parameters.DQ = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "InverseQ": parameters.InverseQ = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "D": parameters.D = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                    }
                }
            }
            else
            {
                throw new Exception("Invalid XML RSA key.");
            }

            return parameters;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            _key = new RsaSecurityKey(LoadKey(_config["jwt"]));
            services.AddSingleton(_key);
            services.AddSingleton(new SigningCredentials(_key, SecurityAlgorithms.RsaSha256Signature));

            services
              .AddDbContext<ArticleContext>(options =>
                  options.UseSqlServer(_config["database"], builder => builder.MigrationsAssembly(typeof(ArticleContext).Assembly.FullName))
              )
                .AddScoped(typeof(IRepository<>), typeof(Repository<>))
                .AddScoped<IArticleService, ArticleService>()
                .AddScoped<IAuthorService, AuthorService>()
                .AddMemoryCache()
                .AddAuthenticationCore()
                .AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters();

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .Use(async (context, next) =>
                {
                    try
                    {
                        await next();
                    }
                    catch (Exception e)
                    {
                        context.Response.StatusCode = e is NotImplementedException ? 404 : e is UnauthorizedAccessException || e is SecurityTokenValidationException ? 401 : e is ArgumentException ? 400 : 500;
                        context.Response.ContentType = "application/json; charset=utf-8";

                        string message = "";

                        Exception x = e;
                        do
                        {
                            message += x.Message + "\r\n\r\n";
                        } while ((x = x.InnerException) != null);

                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                        {
                            message = message.Substring(0, message.Length - 4),
                            stacktrace = e.StackTrace
                        }));
                    }
                })
                .Use(async (context, next) =>
                {
                    string authorization = context.Request.Headers["Authorization"];
                    if (authorization != null)
                    {
                        if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                        {
                            var token = authorization.Substring("Bearer ".Length).Trim();

                            var tokenHandler = new JwtSecurityTokenHandler();

                            try
                            {
                                var claims = tokenHandler.ValidateToken(token, new TokenValidationParameters
                                {
                                    ValidateAudience = true,
                                    ValidIssuer = "Article",
                                    ValidAudience = "Article Users",
                                    IssuerSigningKey = _key
                                }, out SecurityToken _);

                                context.User = claims;
                            }
                            catch (Exception e)
                            {
                                throw new Exception("Failed to verify the token.", e);
                            }
                        }
                        else
                        {
                            throw new Exception("invalid token type!");
                        }
                    }

                    await next();
                })
                .UseMvc();
        }
    }
}
