using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab4_5.Entity
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Review> Reviews { get; }
        IRepository<UserE> Users { get; }
        IRepository<Good> Goods { get; }

        Task<int> SaveAsync();
    }
    public class UnitOfWork : IUnitOfWork

    {
        private readonly CosmeticShopReviewsContext _context;
        public CosmeticShopReviewsContext Context => _context;
        public IRepository<Review> Reviews { get; private set; }
        public IRepository<UserE> Users { get; private set; }
        public IRepository<Good> Goods { get; private set; }

        public UnitOfWork(CosmeticShopReviewsContext context)
        {
            _context = context;
            Reviews = new Repository<Review>(context);
            Users = new Repository<UserE>(context); 
            Goods = new Repository<Good>(context);
        }

        public async Task<int> SaveAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }


}
