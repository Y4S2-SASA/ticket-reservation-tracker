using TRT.Domain.Entities;
using TRT.Domain.Repositories.Command;
using TRT.Infrastructure.Data;
using TRT.Infrastructure.Repositories.Command.Base;

/*
 * File: TrainCommandRepository.cs
 * Author:Jayathilake S.M.D.A.R/IT20037338
*/

namespace TRT.Infrastructure.Repositories.Command
{
    public class TrainCommandRepository
        : CommandRepository<Train>, ITrainCommandRepository
    {
        public TrainCommandRepository(TRTContext context)
            : base(context)
        {

        }

    }
}
