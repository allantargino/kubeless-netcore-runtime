namespace Kubeless.WebAPI
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Kubeless.Core.Interfaces;
    using Kubeless.WebAPI.Utils;
    using Kubeless.Core.Models;

    public sealed class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            this.Configuration = configuration;

            if (!env.IsDevelopment())
            {
                // TODO: Get latest available version.
                Environment.SetEnvironmentVariable("DOTNETCORESHAREDREF_VERSION", "2.0.5");
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            // Compile Function.
            IFunction function = FunctionFactory.BuildFunction(this.Configuration);
            ICompiler compiler = new DefaultCompiler(new DefaultParser(), new DefaultReferencesManager());

            if (!function.IsCompiled())
            {
                compiler.Compile(function);
            }

            services.AddSingleton<IFunction>(function);
            services.AddSingleton<IFunctionSettings>(serviceProvider => serviceProvider.GetService<IFunction>().FunctionSettings);
            services.AddSingleton<IInvoker, DefaultInvoker>();
            services.AddSingleton<ReportBuilder>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
