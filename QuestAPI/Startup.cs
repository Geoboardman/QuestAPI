using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuestAPI.Models;

namespace QuestAPI
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<GameWorldContext>(opt =>
                opt.UseSqlServer("Server=sanctuaryrpg.cduqxrqhyqqv.us-west-2.rds.amazonaws.com;Database=GameWorld;User ID=Admin;Password=Passwordx;"));//UseInMemoryDatabase("MonsterList"));
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
        }
    }
}
