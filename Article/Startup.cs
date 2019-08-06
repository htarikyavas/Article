using Article.Data;
using Article.Data.Repositories;
using Article.Services.Articles;
using Article.Services.Authors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Article
{
    public class Startup
    {
        private readonly IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
              .AddDbContext<ArticleContext>(options =>
                  options.UseSqlServer(_config["database"], builder => builder.MigrationsAssembly(typeof(ArticleContext).Assembly.FullName))
              )
                .AddScoped(typeof(IRepository<>), typeof(Repository<>))
                .AddScoped<IArticleService, ArticleService>()
                .AddScoped<IAuthorService, AuthorService>()
                .AddMemoryCache()
                .AddMvcCore()
                .AddJsonFormatters();

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
