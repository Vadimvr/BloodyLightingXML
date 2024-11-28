using Microsoft.EntityFrameworkCore;
using Models;
using mouse_lighting.Services.db;

namespace mouse_lighting.Services.DataService.Repository
{
    public interface IRepository<T> where T : Entity, new()
    {
        IQueryable<T> Items { get; }
        T? Get(int id);
        IEnumerable<T> GetAll();
        T Add(T entity, bool autoSeve = true);
        void Update(int id, T entityNew, bool autoSeve = true);
        void Delete(int id, bool autoSeve = true);
        void Save();

        Task<T?> GetAsync(int id, CancellationToken cancel = default);
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancel = default);
        Task<T> AddAsync(T entity, bool autoSeve = true, CancellationToken cancel = default);
        Task DeleteAsync(int id, bool autoSeve = true, CancellationToken cancel = default);
        Task UpdateAsync(int id, T entityNew, bool autoSeve = true, CancellationToken cancel = default);
        Task SaveAsync(CancellationToken cancel = default);

        void Clear();
    }

    public class Repository<T> : IRepository<T> where T : Entity, new()
    {
        private readonly DbContext _Db;
        private readonly DbSet<T> _Set;
        public Repository(ApplContextSqLite db)
        {
            _Db = db;
            _Set = db.Set<T>();
        }

        public virtual IQueryable<T> Items => _Set;

        public IEnumerable<T> GetAll() => Items.ToList();

        public T? Get(int id) => Items.SingleOrDefault(i => i.Id == id);
        public T Add(T entity, bool autoSeve = true)
        {
            if (entity is null) { throw new ArgumentNullException(nameof(entity)); }
            _Db.Entry(entity).State = EntityState.Added;
            if (autoSeve) _Db.SaveChanges();
            return entity;
        }
        public void Delete(int id, bool autoSeve = true)
        {
            var x = Get(id);
            if (x != null)
            {
                _Db.Entry(x).State = EntityState.Deleted;
                if (autoSeve) { _Db.SaveChanges(); }
            }
        }
        public void Update(int id, T entityNew, bool autoSeve = true)
        {
            if (entityNew is null) { throw new ArgumentNullException($"{nameof(entityNew)} is null"); }
            T? entity = Get(id);
            if (entity is null) { throw new ArgumentNullException($"{nameof(entity)}.Id{nameof(id)} is null"); }
            entity.Update(entityNew);
            _Db.Entry(entity).State = EntityState.Modified;
            if (autoSeve) { _Db.SaveChanges(); }
        }
        public void Save() => _Db.SaveChanges();

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancel = default) => await Items.ToListAsync(cancel);

        public async Task<T?> GetAsync(int id, CancellationToken cancel = default) => await Items.SingleOrDefaultAsync(i => i.Id == id, cancel).ConfigureAwait(false);

        public async Task<T> AddAsync(T entity, bool autoSeve = true, CancellationToken cancel = default)
        {
            if (entity is null) { throw new ArgumentNullException(nameof(entity)); }
            _Db.Entry(entity).State = EntityState.Added;
            if (autoSeve) await _Db.SaveChangesAsync(cancel).ConfigureAwait(false);
            return entity;
        }

        public async Task DeleteAsync(int id, bool autoSeve = true, CancellationToken cancel = default)
        {
            _Db.Remove(new T { Id = id });
            if (autoSeve) { await _Db.SaveChangesAsync(cancel).ConfigureAwait(false); }
        }

        public async Task UpdateAsync(int id, T entityNew, bool autoSeve = true, CancellationToken cancel = default)
        {
            if (entityNew is null) { throw new ArgumentNullException($"{nameof(entityNew)} is null"); }
            T? entity = await GetAsync(id, cancel).ConfigureAwait(false);
            if (entity is null) { throw new ArgumentNullException($"{nameof(entity)}.Id{nameof(id)} is null"); }
            entity.Update(entityNew);
            _Db.Entry(entity).State = EntityState.Modified;
            if (autoSeve) { await _Db.SaveChangesAsync(cancel); }
        }
        public async Task SaveAsync(CancellationToken cancel = default) => await _Db.SaveChangesAsync(cancel).ConfigureAwait(false);

        public void Clear() => _Db.ChangeTracker.Clear();
    }
}
