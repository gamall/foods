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
    public class UserBookPageRepository : EFRepository<UserBookPage>, IUserBookPageRepository
    {
        public UserBookPageRepository(DbContext context)
            : base(context)
        {
        }

        public UserBookPage IsBookExist(int bookId)
        {
            return DbSet.FirstOrDefault(o => o.UserBookId == bookId);
        }

        public IEnumerable<UserBookPage> GetUserBookPageByUserBookId(int userBookId) 
        {
            return DbSet.Where(o => o.UserBookId == userBookId);
        }

        public IEnumerable<UserBookPage> GetUserBookPageId(int userBookId, int pageNumber, bool isIntro)
        {
            return DbSet.Where(o => o.UserBookId == userBookId).Where(o => o.PageNumber == pageNumber).Where(o => o.IsIntroductory == isIntro);
        }
    }
}
