using TRT.Domain.Entities;
using TRT.Domain.Repositories.Query;
using TRT.Infrastructure.Data;
using TRT.Infrastructure.Repositories.Query.Base;

namespace TRT.Infrastructure.Repositories.Query
{
    public class UserQueryRepository : QueryRepository<User>, IUserQueryRepository
    {
        public UserQueryRepository(TRTContext context)
            : base(context)
        {

        }
    }
}
