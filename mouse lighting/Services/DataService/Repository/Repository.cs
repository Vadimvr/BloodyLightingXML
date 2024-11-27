namespace mouse_lighting.Services.DataService.Repository
{
    //internal abstract class Repository<T, I> : IRepository<T, I> where T : Entity<I> where I : struct
    //{
    //    protected List<T> Items = new List<T>();
    //    public abstract I Add(T entity);

    //    public bool Delete(I id)
    //    {
    //        T? item = Items.FirstOrDefault(c => Equals(c.Id, id));
    //        if (item == null) { return false; }
    //        Items.Remove(item);
    //        return true;
    //    }

    //    public T? Get(I id)
    //    {
    //        return Items.FirstOrDefault(I => Equals(I.Id, id));
    //    }

    //    public IEnumerable<T> GetAll(int skip = 0, int take = int.MaxValue)
    //    {
    //        return Items.GetRange(skip, Items.Count < take ? Items.Count : take);
    //    }

    //    public virtual void Update(I id, T newEntity)
    //    {
    //        T? old = Items.FirstOrDefault(c => Equals(c.Id, id));
    //        if (old == null) { throw new ArgumentNullException(nameof(old)); }

    //        Update(old, newEntity);
    //    }

    //    protected abstract void Update(T old, T newEntity);
    //}
}
