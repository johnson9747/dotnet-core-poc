using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Models;
using EmployeeManagement.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagement
{
	public class Startup
	{
		private IConfiguration _config;

		public Startup(IConfiguration config)
		{
			_config = config;
		}
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContextPool<AppDbContext>(
				options => options.UseSqlServer(_config.GetConnectionString("EmployeeDbConnection")));
			services.AddIdentity<ApplicationUser, IdentityRole>(options =>
			{
				options.Password.RequiredLength = 5;
				options.Password.RequiredUniqueChars = 3;
				options.Password.RequireNonAlphanumeric = false;
			})
			.AddEntityFrameworkStores<AppDbContext>();
			services.AddMvc().AddXmlSerializerFormatters();
			services.AddAuthorization(options =>
			{
				options.AddPolicy("DeleteRolePolicy",
					policy => policy.RequireClaim("Delete Role"));
			});
			services.ConfigureApplicationCookie(options =>
			{
				options.AccessDeniedPath = new PathString("/Administration/AccessDenied");
			});
			services.AddSingleton<ITest, RealTest>();
			services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				app.UseStatusCodePagesWithReExecute("/Error/{0}");
			}
			app.UseStaticFiles();
			//app.UseMvcWithDefaultRoute();
			//app.Run(async (context) =>
			//{
			//await context.Response.WriteAsync(System.Diagnostics.Process.GetCurrentProcess().ProcessName + _config["APIKey"]);
			//});
			app.UseAuthentication();
			app.UseMvc(routes =>
			{
				routes.MapRoute("default", "{controller=Home}/{action=All}/{id?}");
			});
		}
	}
}
