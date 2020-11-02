using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sequel.Demo.Business;
using Sequel.Infrastructure;
using Sequel.Infrastructure.Component;
using Sequel.Infrastructure.Operations;
using Sequel.Infrastructure.Repository;
using Sequel.Middlewares;
using Unity;
using Unity.Lifetime;

namespace Sequel
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.SetBasePath(env.ContentRootPath)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
                configurationBuilder.AddJsonFile("appsettings.Development.json");
            else
                configurationBuilder.AddJsonFile("appsettings.json");
            Configuration = configurationBuilder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IUnityContainer container = new UnityContainer();
            InitSettings(container);
            InitCore(container);

            services.AddSingleton(container);
            services.AddControllers();
            services.AddMvcCore()
                .AddMvcOptions(o =>
                {
                    o.Conventions.Add(new GenericRouteConvention(container));
                })
                .ConfigureApplicationPartManager(m =>
                {
                    m.FeatureProviders.Add(new GenericQueryControllerFeatureProvider(container));
                    m.FeatureProviders.Add(new GenericCommandControllerFeatureProvider(container));
                });

            services.AddCors(o => o.AddPolicy("DebugPolicy", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));

            services.AddOpenApiDocument(settings =>
            {
                settings.GenerateEnumMappingDescription = true;
            });
        }

        private void InitSettings(IUnityContainer container)
        {
            AppConfig config = new AppConfig();
            config.DefaultConnectionString = Configuration.GetConnectionString("Default");
            container.RegisterInstance(config, new SingletonLifetimeManager());
        }

        private void InitCore(IUnityContainer container)
        {
            container.RegisterSingleton<QueriesDefinitions>();
            container.RegisterSingleton<CommandsDefinitions>();

            ComponentManager moduleManager = new ComponentManager(container, AvailableComponents);
            moduleManager.LoadAllComponents();
            container.RegisterInstance(moduleManager, new SingletonLifetimeManager());
            container.RegisterType<IUnitOfWork, UnitOfWork>(new PerResolveLifetimeManager());
            container.RegisterType<DbContextFactory>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors("DebugPolicy");
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseOpenApi();
            app.UseSwaggerUi3();

        }

        private IEnumerable<Type> AvailableComponents
        {
            get
            {
                List<Type> componentsAvailables = new List<Type>();
                IEnumerable<Assembly> assemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies().Where(a => a.FullName.Contains("Sequel")).Select(a => Assembly.Load(a));
                foreach (Assembly assembly in assemblies)
                {
                    componentsAvailables.AddRange(assembly.GetTypes().Where(p => p != null && typeof(IComponent).IsAssignableFrom(p) && p.IsClass && p.IsAbstract == false));
                }
                return componentsAvailables;
            }
        }

        private void LinkerCanYouPleaseInclude()
        {
            DemoBusinessComponent c = new DemoBusinessComponent(null);
        }
    }
}
