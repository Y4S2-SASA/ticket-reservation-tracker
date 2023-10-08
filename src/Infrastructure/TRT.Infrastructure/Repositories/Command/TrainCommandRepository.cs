using MongoDB.Bson;
using MongoDB.Driver;
using TRT.Domain.Entities;
using TRT.Domain.Repositories.Command;
using TRT.Infrastructure.Data;
using TRT.Infrastructure.Repositories.Command.Base;

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
