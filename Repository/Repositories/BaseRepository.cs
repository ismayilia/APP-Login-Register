using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Exceptions;
using Repository.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
	public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
	{
		protected readonly AppDbContext _context;
		private readonly DbSet<T> _enities;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
			_enities = _context.Set<T>();
        }
        public async Task CreateAsync(T entity)
		{
			ArgumentNullException.ThrowIfNull(entity);
			await _enities.AddAsync(entity);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(T entity)
		{
			//ArgumentNullException.ThrowIfNull(entity);
			//if(entity is null) throw new NotFoundException("Data not found");
			_enities.Remove(entity);
			await _context.SaveChangesAsync();
		}

		public async Task<List<T>> GetAllAsync()
		{
			return await _enities.ToListAsync();
		}

		public async Task<T> GetAsync(int? id)
		{
			ArgumentNullException.ThrowIfNull(id);

			//var data = await _enities.FirstOrDefaultAsync(m => m.Id == id);
			//if (data is null) throw new NullReferenceException();
			//return data;
			// ?? => entity null-disa exceptionu ishlet, null deyilse ozun ishlet....??=> null-a gore yoxlayir
			return await _enities.FirstOrDefaultAsync(m => m.Id == id);/*?? throw new NotFoundException("Data not found");*/
		}

		public async Task UpdateAsync(T entity)
		{
			ArgumentNullException.ThrowIfNull(entity);
			_enities.Update(entity);
			await _context.SaveChangesAsync();
		}
	}
}
