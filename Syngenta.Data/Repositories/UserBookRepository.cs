using Syngenta.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syngenta.Core.Contracts.Data;
using System.Data.Entity;

namespace Syngenta.Data.Repositories
{
    public class UserBookRepository : EFRepository<UserBook>, IUserBookRepository
    {
        public UserBookRepository(DbContext context)
            : base(context)
        {
        }
                
        public UserBook GetFirstIncompletedUserBook(int userId)
        {
            return DbSet.FirstOrDefault(o => !o.IsCompleted && o.UserId == userId);
        }

        public UserBook IsUserExist(int userId)
        {
            return DbSet.FirstOrDefault(o => o.UserId == userId);
        }

        public UserBook GetFirstCompletedUserBook(int userId)
        {
            return DbSet.FirstOrDefault(o => o.IsCompleted && o.UserId == userId);
        }

        public UserBook GetLatestCompletedUserBook(int userId)
        {
            return DbSet.OrderByDescending(o => o.Id).FirstOrDefault(o => o.IsCompleted && o.UserId == userId);
        }

        public UserBook GetLatestIncompletedUserBook(int userId)
        {
            return DbSet.OrderByDescending(o => o.Id).FirstOrDefault(o => !o.IsCompleted && o.UserId == userId);
        }
    }
}
