using MagicT_TAPI.Repository;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace MagicVilla_VillaAPI.Repository
{
	public class VillaRepository : Repository<Villa>, IVillaRepository
	{
		private readonly ApplicationDbContext _dbo;

        public VillaRepository(ApplicationDbContext dbo)
			: base(dbo)
        {
			_dbo = dbo;
        }

		public async Task<Villa> UpdateAsync(Villa entity)
		{
			entity.UpdateDate = DateTime.Now;
			_dbo.Villas.Update(entity);
			await _dbo.SaveChangesAsync();

			return entity;
		}
	}
}
