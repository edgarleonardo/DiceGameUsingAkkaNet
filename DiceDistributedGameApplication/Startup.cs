using Akka.Actor;
using DiceDistributedGame.Actors.Actors;
using DiceDistributedGame.Model.Games;
using DiceDistributedGameApplication.Dto;
using DiceDistributedGameApplication.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace DiceDistributedGameApplication
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
            services.AddMvc();
            services.AddSignalR();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Dice API", Version = "v1" });
            });
            var actorSystem = ActorSystem.Create("DiceGameSystem");
            services.AddSingleton(typeof(ActorSystem), (serviceProvider) => actorSystem);            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSignalR(routes =>
            {
                routes.MapHub<NotificationHub>("notifications");
            });
            app.UseMvc();

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<GameInfoForReport, GameInfoForReportDto>();
                cfg.CreateMap<GameInfoForReportDto, GameInfoForReport>();
            });
        }
    }
}
