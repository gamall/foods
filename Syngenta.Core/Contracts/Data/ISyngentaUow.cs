using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syngenta.Core.Contracts.Data
{
    public interface ISyngentaUow
    {
        void Commit();
        IUserRepository Users { get; } 
        IUserBookRepository UserBooks { get; }
        IUserBookPageRepository UserBookPages { get; }
        IFailedLoginAttemptRepository FailedLoginAttempts { get; }
        IUserModificationLogRepository UserModificationLogs { get; }
    }
}
