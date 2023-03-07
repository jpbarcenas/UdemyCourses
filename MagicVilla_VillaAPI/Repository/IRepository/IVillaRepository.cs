using MagicVilla_VillaAPI.Models;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
	public interface IVillaRepository
	{
		Task CreateAsync(Villa entity);
		Task<List<Villa>> ReadAllAsync(Expression<Func<Villa, bool>> filter = null);
		Task<Villa> ReadAsync(Expression<Func<Villa, bool>> filter = null, bool tracked = true);
		Task UpdateAsync(Villa entity);
		Task DeleteAsync(Villa entity);
		Task SaveAsync();
	}
}
