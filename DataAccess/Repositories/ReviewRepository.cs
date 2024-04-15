﻿using Domain.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Dtos;

namespace DataAccess.Repositories
{
    public class ReviewRepository(AppDbContext appDbContext) : IReviewRepository
    {
        public async Task AssignReviewAsync(Review review)
        {
            await appDbContext.AddAsync(review);
            await appDbContext.SaveChangesAsync();
        }

        public async Task<List<Review>> GetByReviewSearchDtoAsync(ReviewSearchDto dto, int reviewsPerPage)
        {
            var reviews = appDbContext.Reviews
                .Include(x => x.Content)
                .Where(x => x.UserId == dto.UserId)
                .Where(x => x.Text.Contains(dto.Search ?? ""));
            
            switch (dto.SortType)
            {
                case ReviewSortType.Rating:
                    reviews = reviews.OrderByDescending(x => x.Score);
                    break;
                case ReviewSortType.DateUpdated:
                    reviews = reviews.OrderByDescending(x => x.WrittenAt);
                    break;
                case null:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            return await reviews
                .Skip(dto.Page * reviewsPerPage)
                .Take(reviewsPerPage)
                .ToListAsync();
        }

        public async Task<int> GetPagesCountAsync(ReviewSearchDto dto, int reviewsPerPage)
        {
            var pages = Math.Ceiling(await appDbContext.Reviews
                .Where(x => x.UserId == dto.UserId)
                .Where(x => x.Text.Contains(dto.Search ?? ""))
                .CountAsync() / (double) reviewsPerPage);
            return (int)pages;
        }

        public async Task<int?> GetScoreByUserAsync(long userId, long contentId)
        {
            var result = await appDbContext.Reviews
                .Where(x => x.UserId == userId)
                .Where(x => x.ContentId == contentId)
                .FirstOrDefaultAsync();

            return result?.Score;
        }

        public async Task<List<Review>> GetReviewsByFilterAsync(Expression<Func<Review, bool>> filter) =>
            await appDbContext.Reviews.Where(filter)
                .Include(r => r.User)
                .Include(r => r.Comments)!
                    .ThenInclude(r => r.User)
                 .Include(r => r.Comments)!
                    .ThenInclude(r => r.ScoredByUsers)
                .Include(r => r.RatedByUsers)
                .ToListAsync();
    }
}
