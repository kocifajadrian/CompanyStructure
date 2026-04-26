namespace CompanyStructure.Application.Interfaces.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
