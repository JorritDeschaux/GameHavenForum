using Application.Persistence.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
	public static class StartupSetup
	{
		public static void AddDbContext(this IServiceCollection services, string connectionString) =>
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseMySQL(connectionString)); // will be created in web project root

		public static void AddScoped(this IServiceCollection services)
		{
			services.AddMediatR(Assembly.GetExecutingAssembly());
		}

	}
}
