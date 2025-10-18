﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Shared.DDD;

namespace Shared.Data.Interceptors
{
    public class AuditableEntityInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {

            UpdateEntities(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
                                                                              InterceptionResult<int> result,
                                                                              CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }



        private static void UpdateEntities(DbContext? context)
        {
            if (context is null)
            {
                return;
            }

            foreach (var item in context.ChangeTracker.Entries<IEntity>())
            {

                if (item.State == EntityState.Added)
                {
                    item.Entity.CreatedBy = "neven";
                    item.Entity.CreatedAt = DateTime.UtcNow;
                }

                if (item.State == EntityState.Added || item.State == EntityState.Modified || Extensions.HasChangedOwnEntities(item))
                {
                    item.Entity.LastModifiedBy = "neven";
                    item.Entity.LastModified = DateTime.UtcNow;
                }
            }
        }
    }



    public static class Extensions
    {
        public static bool HasChangedOwnEntities(this EntityEntry entry) => entry.References.Any(r =>
        r.TargetEntry != null &&
        r.TargetEntry.Metadata.IsOwned() &&
        (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
    }
}
