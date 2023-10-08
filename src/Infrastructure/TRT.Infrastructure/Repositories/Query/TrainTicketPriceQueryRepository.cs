using TRT.Domain.Entities;
using TRT.Domain.Repositories.Query;
using TRT.Infrastructure.Data;
using TRT.Infrastructure.Repositories.Query.Base;

namespace TRT.Infrastructure.Repositories.Query
{
    public class TrainTicketPriceQueryRepository : QueryRepository<TrainTicketPrice>, ITrainTicketPriceQueryRepository
    {
        public TrainTicketPriceQueryRepository(TRTContext context)
            : base(context)
        {

        }
    }
}
