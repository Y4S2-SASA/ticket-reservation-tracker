using TRT.Domain.Entities;
using TRT.Domain.Repositories.Command;
using TRT.Infrastructure.Data;
using TRT.Infrastructure.Repositories.Command.Base;

namespace TRT.Infrastructure.Repositories.Command
{
    public class ScheduleCommandRepository
        : CommandRepository<Schedule>, IScheduleCommandRepository
    {
        public ScheduleCommandRepository(TRTContext context)
            : base(context)
        {

        }
    }
}
