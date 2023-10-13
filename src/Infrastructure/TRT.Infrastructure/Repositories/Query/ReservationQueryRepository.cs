using TRT.Domain.Entities;
using TRT.Domain.Repositories.Query;
using TRT.Infrastructure.Data;
using TRT.Infrastructure.Repositories.Query.Base;
/*
 * File: ReservationQueryRepository.cs
 * Author:Bartholomeusz S.V/IT20274702
*/
namespace TRT.Infrastructure.Repositories.Query
{
    public class ReservationQueryRepository
        : QueryRepository<Reservation>, IReservationQueryRepository
    {
        public ReservationQueryRepository(TRTContext context)
            : base(context)
        {

        }
    }
}
