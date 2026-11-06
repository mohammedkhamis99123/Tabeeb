using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Tabeeb.Application.Common.Services;
using Tabeeb.Domain.Common;
using Tabeeb.Domain.Common.Tabeeb.Domain.Common;

namespace Tabeeb.Infrastructure.Interceptors
{       public class Auditingnterceptor : SaveChangesInterceptor
        {
            private readonly ICurrentUserService _currentUser;

            public Auditingnterceptor(ICurrentUserService currentUser)
            {
                _currentUser = currentUser;
            }

            public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
            {
                var entries = eventData.Context.ChangeTracker.Entries();

            // inside SavingChangesAsync
            string userId = _currentUser.UserId;
            //var tz = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time"); // السعودية
            //var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz);
            foreach (var entry in eventData.Context.ChangeTracker.Entries<IBaseAuditableEntity>())
            {
                  
                    var entity = entry.Entity;
                    if (!string.IsNullOrEmpty(userId))
                        {
                            if (entry.State == EntityState.Added)
                            {
                                entity.CreatedBy = userId;
                                entity.CreatedAt = DateTime.UtcNow;
                            }
                            else if (entry.State == EntityState.Modified)
                            {
                                entity.UpdatedBy = userId;
                                entity.UpdatedAt = DateTime.UtcNow;
                                if (entity.DeletedAt == null)
                                {
                                    entry.Property(nameof(entity.DeletedAt)).IsModified = false;
                                    entry.Property(nameof(entity.DeletedBy)).IsModified = false;

                                }
                                else
                                {
                                    entity.DeletedAt = DateTime.UtcNow;
                                    entity.DeletedBy = userId;
                                }
                                entry.Property(nameof(entity.CreatedAt)).IsModified = false;
                                entry.Property(nameof(entity.CreatedBy)).IsModified = false;

                            }
                            else if (entry.State == EntityState.Deleted)
                            {
                                entry.State = EntityState.Modified; // prevent physical deletion

                                entity.DeletedBy = userId;
                                entity.DeletedAt = DateTime.UtcNow;

                                entry.Property(nameof(entity.CreatedAt)).IsModified = false;
                                entry.Property(nameof(entity.UpdatedAt)).IsModified = false;
                                entry.Property(nameof(entity.CreatedBy)).IsModified = false;
                                entry.Property(nameof(entity.UpdatedBy)).IsModified = false;

                            }
                        }

                    }

                    //else if (entry.Entity is BaseAuditableEntity<long> longentity)
                    //{

                    //    if (!string.IsNullOrEmpty(userId))
                    //    {
                    //        if (entry.State == EntityState.Added)
                    //        {
                    //            longentity.CreatedBy = userId;
                    //            longentity.CreatedAt = DateTime.UtcNow;
                    //        }
                    //        else if (entry.State == EntityState.Modified)
                    //        {
                    //            longentity.UpdatedBy = userId;
                    //            longentity.UpdatedAt = DateTime.UtcNow;
                    //            entry.Property(nameof(entity.CreatedAt)).IsModified = false;
                    //            entry.Property(nameof(entity.CreatedBy)).IsModified = false;
                    //            entry.Property(nameof(entity.DeletedAt)).IsModified = false;
                    //            entry.Property(nameof(entity.DeletedBy)).IsModified = false;
                    //        }
                    //        else if (entry.State == EntityState.Deleted)
                    //        {
                    //            entry.State = EntityState.Modified; // prevent physical deletion

                    //            longentity.DeletedBy = userId;
                    //            longentity.DeletedAt = DateTime.UtcNow;

                    //            entry.Property(nameof(entity.CreatedAt)).IsModified = false;
                    //            entry.Property(nameof(entity.UpdatedAt)).IsModified = false;
                    //            entry.Property(nameof(entity.CreatedBy)).IsModified = false;
                    //            entry.Property(nameof(entity.UpdatedBy)).IsModified = false;

                    //        }
                    //    }

                    //}
                    //else if (entry.Entity is BaseAuditableEntity<short> shortEntity)
                    //{

                    //    if (!string.IsNullOrEmpty(userId))
                    //    {
                    //        if (entry.State == EntityState.Added)
                    //        {
                    //        shortEntity.CreatedBy = userId;
                    //        shortEntity.CreatedAt = DateTime.UtcNow;
                    //        }
                    //        else if (entry.State == EntityState.Modified)
                    //        {
                    //        shortEntity.UpdatedBy = userId;
                    //            shortEntity.UpdatedAt = DateTime.UtcNow;
                    //            entry.Property(nameof(entity.CreatedAt)).IsModified = false;
                    //            entry.Property(nameof(entity.CreatedBy)).IsModified = false;
                    //            entry.Property(nameof(entity.DeletedAt)).IsModified = false;
                    //            entry.Property(nameof(entity.DeletedBy)).IsModified = false;
                    //        }
                    //        else if (entry.State == EntityState.Deleted)
                    //        {
                    //            entry.State = EntityState.Modified; // prevent physical deletion

                    //        shortEntity.DeletedBy = userId;
                    //            shortEntity.DeletedAt = DateTime.UtcNow;

                    //            entry.Property(nameof(entity.CreatedAt)).IsModified = false;
                    //            entry.Property(nameof(entity.UpdatedAt)).IsModified = false;
                    //            entry.Property(nameof(entity.CreatedBy)).IsModified = false;
                    //            entry.Property(nameof(entity.UpdatedBy)).IsModified = false;

                    //        }
                    //    }

                    //}
      



                    //else if (entry.Entity is BaseAuditableEntity<byte> byteEntity)
                    //{

                    //    if (!string.IsNullOrEmpty(userId))
                    //    {
                    //        if (entry.State == EntityState.Added)
                    //        {
                    //            byteEntity.CreatedBy = userId;
                    //            byteEntity.CreatedAt = DateTime.UtcNow;
                    //        }
                    //        else if (entry.State == EntityState.Modified)
                    //        {
                    //            byteEntity.UpdatedBy = userId;
                    //            byteEntity.UpdatedAt = DateTime.UtcNow;
                    //            entry.Property(nameof(entity.CreatedAt)).IsModified = false;
                    //            entry.Property(nameof(entity.CreatedBy)).IsModified = false;
                    //            entry.Property(nameof(entity.DeletedAt)).IsModified = false;
                    //            entry.Property(nameof(entity.DeletedBy)).IsModified = false;
                    //        }
                    //        else if (entry.State == EntityState.Deleted)
                    //        {
                    //            entry.State = EntityState.Modified; // prevent physical deletion

                    //            byteEntity.DeletedBy = userId;
                    //            byteEntity.DeletedAt = DateTime.UtcNow;

                    //            entry.Property(nameof(entity.CreatedAt)).IsModified = false;
                    //            entry.Property(nameof(entity.UpdatedAt)).IsModified = false;
                    //            entry.Property(nameof(entity.CreatedBy)).IsModified = false;
                    //            entry.Property(nameof(entity.UpdatedBy)).IsModified = false;

                    //        }
                    //    }

                    //}

                
                return await base.SavingChangesAsync(eventData, result, cancellationToken);

            }
        }
    }





