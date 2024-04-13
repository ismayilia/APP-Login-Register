using Microsoft.Extensions.DependencyInjection;
using Repository.Repositories.Interfaces;
using Repository.Repositories;

namespace Service
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddRepositoryLayer(this IServiceCollection services)
		{
			services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>)); // generic-in inject olmasi
			services.AddScoped<IEmployeeRepository, EmployeeRepository>();
			return services;
		}
	}
}
