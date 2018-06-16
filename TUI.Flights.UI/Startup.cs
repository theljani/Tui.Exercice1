//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using TUI.Flights.Core.Services.FlightServices;
//using TUI.Flights.Infrastructure.Base;
//using TUI.Flights.Infrastructure;
//using Microsoft.EntityFrameworkCore;
//using TUI.Flights.Infrastructure.Data;
//using TUI.Flights.Core.Mappers;
//using TUI.Flights.Common.Entities;
//using TUI.Flights.Core.Services.AirportServices;
//using TUI.Flights.Core.Helpers;

//namespace TUI.Flights.UI
//{
//    public class Startup
//    {
//        public Startup(IConfiguration configuration)
//        {
//            Configuration = configuration;
//        }

//        public IConfiguration Configuration { get; }

//        // This method gets called by the runtime. Use this method to add services to the container.
//        public void ConfigureServices(IServiceCollection services)
//        {
//            services.AddMvc();

//            services.AddTransient<IFlightServices, FlightServices>();
//            services.AddTransient<IAirportServices, AirportServices>();
//            services.AddTransient<IAircraftServices, AircraftServices>();

//            services.AddTransient<IUnitOfWork, EFUnitOfWork>();

//            services.AddEntityFrameworkSqlServer();
//            services.AddDbContext<EFUnitOfWork>(
//            options =>
//            {
//                var connectionString = Configuration.GetSection("ConnectionString").Value;

//                options.UseSqlServer(connectionString,
//                    sqlServerOptionsAction: sqlOptions =>
//                    {
//                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
//                    });
//            });

//            services.AddTransient<IRepository<Flight>, Repository<Flight>>();
//            services.AddTransient<IRepository<Airport>, Repository<Airport>>();
//            services.AddTransient<IRepository<Aircraft>, Repository<Aircraft>>();

//            services.AddScoped<IDictionaryHelper, DictionaryHelper>();
//            services.AddSingleton<IMapperWrapper, MapperWrapper>();

//            var serviceProvider = services.BuildServiceProvider();
//            var dictionaryHelper = serviceProvider.GetService<IDictionaryHelper>();
//            var automapperConfig = new AutoMapper.MapperConfiguration(acfg =>
//            {
//                acfg.AddProfile(new AutoMapperConfigurations());
//                acfg.ForAllMaps((map, exp) => exp.ForAllOtherMembers(opt => opt.AllowNull()));
//            });

//            var mapper = automapperConfig.CreateMapper();
//            services.AddSingleton(mapper);
//        }

//        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
//        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
//        {
//            if (env.IsDevelopment())
//            {
//                app.UseDeveloperExceptionPage();
//                app.UseBrowserLink();
//            }
//            else
//            {
//                app.UseExceptionHandler("/Home/Error");
//            }

//            app.UseStaticFiles();

//            app.UseMvc(routes =>
//            {
//                routes.MapRoute(
//                    name: "default",
//                    template: "{controller=Flights}/{action=Index}/{id?}");
//            });

//            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
//            {
//                var dbContext = serviceScope.ServiceProvider.GetService<EFUnitOfWork>();

//                dbContext.Database.EnsureCreated();
//                dbContext.Database.Migrate();
//                DbSeeder.Seed(dbContext);
//            }
//        }
//    }
//}
