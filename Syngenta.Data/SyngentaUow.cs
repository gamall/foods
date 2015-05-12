using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syngenta.Core;
using Syngenta.Core.Contracts.Data;
using System.Data.Entity;
using Syngenta.Data;

namespace Syngenta.Data
{
    public class SyngentaUow: ISyngentaUow ,IDisposable
    {
        public SyngentaUow(IRepositoryProvider repositoryProvider)
        {
            CreateDbContext();

            repositoryProvider.DbContext = DbContext;
            RepositoryProvider = repositoryProvider;
        }

        public IUserRepository Users { get { return GetRepo<IUserRepository>(); } }
        public IUserBookRepository UserBooks { get { return GetRepo<IUserBookRepository>(); } }
        public IUserBookPageRepository UserBookPages { get { return GetRepo<IUserBookPageRepository>(); } }
        public IFailedLoginAttemptRepository FailedLoginAttempts { get { return GetRepo<IFailedLoginAttemptRepository>(); } }
        public IUserModificationLogRepository UserModificationLogs { get { return GetRepo<IUserModificationLogRepository>(); } }

        protected IRepositoryProvider RepositoryProvider { get; set; }

        private T GetRepo<T>() where T : class
        {
            return RepositoryProvider.GetRepository<T>();
        }

        private IRepository<T> GetStandardRepo<T>() where T : class
        {
            return RepositoryProvider.GetRepositoryForEntityType<T>();
        }

        /// <summary>
        /// Save pending changes to the database
        /// </summary>
        public void Commit()
        {            
            DbContext.SaveChanges();
        }

        protected void CreateDbContext()
        {
            DbContext = new SyngentaDbContext();

            // Do NOT enable proxied entities, else serialization fails
            DbContext.Configuration.ProxyCreationEnabled = true;

            // Load navigation properties explicitly (avoid serialization trouble)
            DbContext.Configuration.LazyLoadingEnabled = true;

            // Because Web API will perform validation, we don't need/want EF to do so
            DbContext.Configuration.ValidateOnSaveEnabled = false;

            //DbContext.Configuration.AutoDetectChangesEnabled = false;
            // We won't use this performance tweak because we don't need 
            // the extra performance and, when autodetect is false,
            // we'd have to be careful. We're not being that careful.
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private SyngentaDbContext DbContext { get; set; }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (DbContext != null)
                {
                    DbContext.Dispose();
                }
            }
        }

    }
}
