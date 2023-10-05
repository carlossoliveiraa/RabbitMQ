using ImplementandoRabiitMQ.Application.Domain;
using ImplementandoRabiitMQ.Application.Interface;
using ImplementandoRabiitMQ.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ImplementandoRabiitMQ.Data.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        private readonly B3DbContext _db;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(B3DbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _dbSet = _db.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AddAsync(TEntity entity)
        {
            _dbSet.Add(entity);
            await SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            await SaveChangesAsync();
        }

        public async Task RemoveAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await SaveChangesAsync();
            }
        }

        private async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Lidar com exceções de concorrência, se necessário.
                throw;
            }
            catch (DbUpdateException)
            {
                // Lidar com outras exceções do Entity Framework, se necessário.
                throw;
            }
        }
    }
}
