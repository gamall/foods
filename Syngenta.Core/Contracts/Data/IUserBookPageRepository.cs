using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syngenta.Core.Contracts.Data
{
    public interface IUserBookPageRepository : IRepository<UserBookPage>
    {
        UserBookPage IsBookExist(int bookId);
        IEnumerable<UserBookPage> GetUserBookPageByUserBookId(int userBookId);
        IEnumerable<UserBookPage> GetUserBookPageId(int userBookId, int pageNumber, bool isIntro);
    }
}
