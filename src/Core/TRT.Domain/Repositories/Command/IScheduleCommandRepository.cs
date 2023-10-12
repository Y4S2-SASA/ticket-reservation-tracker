using TRT.Domain.Entities;
using TRT.Domain.Repositories.Command.Base;

/*
 * File: IScheduleCommandRepository.cs
 * Author: Perera M.S.D/IT20020262
*/
namespace TRT.Domain.Repositories.Command
{
    public interface IScheduleCommandRepository : ICommandRepository<Schedule>
    {
    }
}
