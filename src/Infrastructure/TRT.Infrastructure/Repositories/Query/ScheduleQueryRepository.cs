using TRT.Domain.Entities;
using TRT.Domain.Repositories.Query;
using TRT.Infrastructure.Data;
using TRT.Infrastructure.Repositories.Query.Base;

namespace TRT.Infrastructure.Repositories.Query
{
    public class ScheduleQueryRepository : QueryRepository<Schedule>, IScheduleQueryRepository
    {
        public ScheduleQueryRepository(TRTContext context)
            : base(context)
        {

        }
    }
}
