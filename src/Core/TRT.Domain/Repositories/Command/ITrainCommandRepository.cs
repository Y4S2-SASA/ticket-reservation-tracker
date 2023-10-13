using TRT.Domain.Entities;
using TRT.Domain.Repositories.Command.Base;
/*
 * File: ITrainCommandRepository.cs
 * Author:Jayathilake S.M.D.A.R/IT20037338
*/
namespace TRT.Domain.Repositories.Command
{
    public interface ITrainCommandRepository : ICommandRepository<Train>
    {
    }
}
