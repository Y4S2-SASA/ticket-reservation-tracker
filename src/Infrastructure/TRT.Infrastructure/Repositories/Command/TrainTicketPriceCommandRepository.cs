using TRT.Domain.Entities;
using TRT.Domain.Repositories.Command;
using TRT.Infrastructure.Data;
using TRT.Infrastructure.Repositories.Command.Base;

namespace TRT.Infrastructure.Repositories.Command
{
    public class TrainTicketPriceCommandRepository 
        : CommandRepository<TrainTicketPrice>, ITrainTicketPriceCommandRepository
    {
        public TrainTicketPriceCommandRepository(TRTContext context)
            : base(context)
        {

        }

    }
}
