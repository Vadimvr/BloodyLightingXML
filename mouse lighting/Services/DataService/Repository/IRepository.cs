using System.ComponentModel.DataAnnotations;

namespace mouse_lighting.Services.DataService.Repository
{
    public interface IRepository<T, I> where T : IEntity<I> where I : struct
    {
        T? Get(I id);
        IEnumerable<T> GetAll(int skip = 0, int take = int.MaxValue);
        I Add(T entity);
        void Update(I id, T entity);
        bool Delete(I id);
    }
    public interface IRepositoryAsync<T, I> where T : IEntity<I> where I : struct
    {
        Task<T?> GetAsync(I id);
        Task<IEnumerable<T>> GetAllAsync(int skip = 0, int take = int.MaxValue);
        Task<I> CreateAsync(T entity);
        Task UpdateAsync(I id, T entity);
        Task<bool> DeleteAsync(I id);
    }
    public interface IEntity<I> where I : struct
    {
        [Key]
        I Id { get; set; }
    }
}
