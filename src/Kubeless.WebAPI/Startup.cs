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
    using System.Diagnostics;

    public sealed class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            // Compile Function.
            string requirementsPath = this.Configuration["Compiler:RequirementsPath"];
            IFunction function = FunctionFactory.BuildFunction(this.Configuration);
            ICompiler compiler = new DefaultCompiler(new DefaultParser(), new DefaultReferencesManager(function.FunctionSettings));

            if (!function.IsCompiled())
            {
                compiler.Compile(function);
            }

            services.AddSingleton<IFunction>(function);
            services.AddSingleton<IFunctionSettings>(serviceProvider => serviceProvider.GetService<IFunction>().FunctionSettings);
            services.AddSingleton<IInvoker>(new DefaultInvoker(requirementsPath));
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
