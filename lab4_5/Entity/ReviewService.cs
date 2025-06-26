using lab4_5.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace lab4_5
{
    public class ReviewService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReviewService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Review>> GetAllAsync()
        {
            var reviews = await _unitOfWork.Reviews.GetAllAsync(
                include: q => q.Include(r => r.User).Include(r => r.Product)
            );
            return reviews.ToList();
        }

        public async Task<Review?> GetByIdAsync(int id)
        {
            var reviews = await _unitOfWork.Reviews.GetAllAsync(
                r => r.ReviewId == id,
                include: q => q.Include(r => r.User).Include(r => r.Product)
            );
            return reviews.FirstOrDefault();
        }

        public async Task AddAsync(Review review)
        {
            await _unitOfWork.Reviews.AddAsync(review);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(Review updatedReview)
        {
            _unitOfWork.Reviews.Update(updatedReview);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var review = await _unitOfWork.Reviews.GetByIdAsync(id);
            if (review != null)
            {
                _unitOfWork.Reviews.Delete(review);
                await _unitOfWork.SaveAsync();
            }
        }

        public async Task<List<Review>> SearchAsync(string searchText)
        {
            var reviews = await _unitOfWork.Reviews.GetAllAsync(
                r => r.ReviewText.Contains(searchText) || r.User.Username.Contains(searchText),
                include: q => q.Include(r => r.User).Include(r => r.Product)
            );
            return reviews.ToList();
        }

        public async Task<List<Review>> FilterAsync(int? productId, int? rating)
        {
            Expression<Func<Review, bool>> filter = r => true;

            if (productId.HasValue)
                filter = r => r.ProductId == productId.Value;

            if (rating.HasValue)
                filter = r => filter.Compile().Invoke(r) && r.Rating == rating.Value;

            var reviews = await _unitOfWork.Reviews.GetAllAsync(
                filter,
                include: q => q.Include(r => r.User).Include(r => r.Product)
            );
            return reviews.ToList();
        }

        //public async Task<List<Review>> SortByDateDescAsync()
        //{
        //    var reviews = await _unitOfWork.Reviews.GetAllAsync(
        //        include: q => q.Include(r => r.User).Include(r => r.Product)
        //    );
        //    return reviews.OrderByDescending(r => r.ReviewDate).ToList();
        //}

        public async Task<List<Review>> GetByProductIdAsync(int productId)
        {
            var reviews = await _unitOfWork.Reviews.GetAllAsync(
                r => r.ProductId == productId,
                include: q => q.Include(r => r.User)
            );
            return reviews.OrderByDescending(r => r.ReviewDate).ToList();
        }


        public async Task AddWithTransactionAsync(Review review)
        {
            var context = (_unitOfWork as UnitOfWork)?.Context;
            if (context == null)
                throw new InvalidOperationException("Невозможно получить контекст из UnitOfWork");

            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                await _unitOfWork.Reviews.AddAsync(review);
                await _unitOfWork.SaveAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

}
