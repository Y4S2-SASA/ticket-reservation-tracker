using TRT.Domain.Entities;
using TRT.Domain.Repositories.Query;
using TRT.Infrastructure.Data;
using TRT.Infrastructure.Repositories.Query.Base;
/*
 * File: ScheduleQueryRepository.cs
 * Author: Perera M.S.D/IT20020262
*/
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
