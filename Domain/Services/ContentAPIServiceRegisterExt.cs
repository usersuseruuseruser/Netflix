﻿using Domain.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Services
{
    public static class ContentAPIServiceRegisterExt
    {
        public static IServiceCollection AddContentAPIServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IContentService, ContentService>();
            serviceCollection.AddScoped<IReviewService, ReviewService>();
            serviceCollection.AddScoped<IFavouriteService, FavouriteService>();
            serviceCollection.AddScoped<ICommentService, CommentService>();

            return serviceCollection;
        }
    }
}
