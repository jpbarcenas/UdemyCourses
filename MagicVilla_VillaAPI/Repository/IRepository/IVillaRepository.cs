using MagicVilla_VillaAPI.Models;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
	public interface IVillaRepository
	{
		Task Create(Villa entity);
		Task<List<Villa>> ReadAll(Expression<Func<Villa, bool>> filter = null);
		Task<Villa> Read(Expression<Func<Villa, bool>> filter = null, bool tracked = true);
		Task Update(Villa entity);
		Task Delete(Villa entity);
		Task Save();
	}
}
