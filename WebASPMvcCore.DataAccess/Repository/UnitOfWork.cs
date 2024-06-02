using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using WebASPMvcCore.DataAccess.Data;
using WebASPMvcCore.Domain.Abstracts;

namespace WebASPMvcCore.DataAccess.Repository
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly ISQLQueryHandler _sqLQueryHandler;

        ApplicationDbContext _applicationDbContext;

        ICategoryRepository _categoryRepository;
        IProductRepository _productRepository;
        IOrderRepository _orderRepository;
        IOrderDetailRepository _orderDrtailRepository;

        IDbContextTransaction _dbContextTransaction;
        private bool disposedValue;

        public UnitOfWork(ApplicationDbContext applicationDbContext,ISQLQueryHandler sQLQueryHandler)
        {
            _applicationDbContext = applicationDbContext;
            _sqLQueryHandler = sQLQueryHandler;
        }

        public DbSet<T> Table<T>() where T : class => _applicationDbContext.Set<T>();

        public ICategoryRepository CategoryRepository => _categoryRepository ?? new CategoryRepository(_applicationDbContext, _sqLQueryHandler);
        public IProductRepository ProductRepository => _productRepository ?? new ProductRepository(_applicationDbContext, _sqLQueryHandler);
        public IOrderRepository OrderRepository => _orderRepository ?? new OrderRepository(_applicationDbContext, _sqLQueryHandler);
        public IOrderDetailRepository OrderDetailRepository => _orderDrtailRepository ?? new OrderDetailRepository(_applicationDbContext, _sqLQueryHandler);

        public async Task BeginTransaction()
        {
            _dbContextTransaction = await _applicationDbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _dbContextTransaction.CommitAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _dbContextTransaction.RollbackAsync();
        }

        public async Task SaveChangeAsync()
        {
            await _applicationDbContext.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _applicationDbContext.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
