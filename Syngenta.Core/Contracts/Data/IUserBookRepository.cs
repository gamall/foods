using System;
using System.Linq;
using Syngenta.Core;

namespace Syngenta.Core.Contracts.Data
{
    public interface IUserBookRepository : IRepository<UserBook>
    {
        UserBook IsUserExist(int userId);
        UserBook GetFirstIncompletedUserBook(int userId);        
        UserBook GetFirstCompletedUserBook(int userId);
        UserBook GetLatestCompletedUserBook(int userId);
        UserBook GetLatestIncompletedUserBook(int userId);
    }
}
