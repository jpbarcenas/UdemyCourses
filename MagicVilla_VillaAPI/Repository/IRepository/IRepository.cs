using MagicVilla_VillaAPI.Models;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
	public interface IRepository<T>	where T : class
	{
		Task CreateAsync(T entity);
		Task<List<T>> ReadAllAsync(Expression<Func<T, bool>>? filter = null);
		Task<T> ReadAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true);
		Task DeleteAsync(T entity);
		Task SaveAsync();
	}
}
