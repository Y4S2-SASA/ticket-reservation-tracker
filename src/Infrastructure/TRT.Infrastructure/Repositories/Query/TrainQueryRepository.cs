using TRT.Domain.Entities;
using TRT.Domain.Repositories.Query;
using TRT.Infrastructure.Data;
using TRT.Infrastructure.Repositories.Query.Base;

/*
 * File: TrainQueryRepository.cs
 * Author: Perera M.S.D/IT20020262
*/
namespace TRT.Infrastructure.Repositories.Query
{
    public class TrainQueryRepository : QueryRepository<Train>, ITrainQueryRepository
    {
        public TrainQueryRepository(TRTContext context)
            : base(context)
        {

        }
    }
}
