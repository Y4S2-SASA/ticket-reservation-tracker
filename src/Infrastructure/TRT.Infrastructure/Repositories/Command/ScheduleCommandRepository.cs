using TRT.Domain.Entities;
using TRT.Domain.Repositories.Command;
using TRT.Infrastructure.Data;
using TRT.Infrastructure.Repositories.Command.Base;
/*
 * File: ScheduleCommandRepository.cs
 * Author: Perera M.S.D/IT20020262
*/

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
