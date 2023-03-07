using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace MagicVilla_VillaAPI.Repository
{
	public class VillaRepository : IVillaRepository
	{
		private readonly ApplicationDbContext _dbo;

        public VillaRepository(ApplicationDbContext dbo)
        {
			_dbo = dbo;
        }
        public async Task Create(Villa entity)
		{
			await _dbo.Villas.AddAsync(entity);
			await Save();
		}

		public async Task<Villa> Read(Expression<Func<Villa, bool>> filter = null, bool tracked = true)
		{
			IQueryable<Villa> query = _dbo.Villas;

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

		public async Task<List<Villa>> ReadAll(Expression<Func<Villa, bool>> filter = null)
		{
			IQueryable<Villa> query = _dbo.Villas;

			if (filter != null)
			{
				query = query.Where(filter);
			}

			return await query.ToListAsync();
		}

		public Task Update(Villa entity)
		{
			throw new NotImplementedException();
		}

		public async Task Delete(Villa entity)
		{
			_dbo.Villas.Remove(entity);
			await Save();
		}

		public async Task Save()
		{
			await _dbo.SaveChangesAsync();
		}
	}
}
