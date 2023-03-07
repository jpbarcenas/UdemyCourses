using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using MagicVilla_VillaAPI.Repository.IRepository;
using MagicVilla_VillaAPI.Data;

namespace MagicT_TAPI.Repository
{
	public class Repository<T> : IRepository<T> where T : class
	{
		private readonly ApplicationDbContext _dbo;
		internal DbSet<T> _dbSet;

		public Repository(ApplicationDbContext dbo)
		{
			_dbo = dbo;
			this._dbSet = _dbo.Set<T>();
		}
		public async Task CreateAsync(T entity)
		{
			await _dbSet.AddAsync(entity);
			await SaveAsync();
		}

		public async Task<T> ReadAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true)
		{
			IQueryable<T> query = _dbSet;

			if (!tracked)
			{
				query = query.AsNoTracking();
			}

			if (filter != null)
			{
				query = query.Where(filter);
			}

			return await query.FirstOrDefaultAsync();
		}

		public async Task<List<T>> ReadAllAsync(Expression<Func<T, bool>>? filter = null)
		{
			IQueryable<T> query = _dbSet;

			if (filter != null)
			{
				query = query.Where(filter);
			}

			return await query.ToListAsync();
		}

		public async Task DeleteAsync(T entity)
		{
			_dbSet.Remove(entity);
			await SaveAsync();
		}

		public async Task SaveAsync()
		{
			await _dbo.SaveChangesAsync();
		}
	}
}
