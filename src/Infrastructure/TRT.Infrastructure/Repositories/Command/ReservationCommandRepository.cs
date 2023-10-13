using TRT.Domain.Entities;
using TRT.Domain.Repositories.Command;
using TRT.Infrastructure.Data;
using TRT.Infrastructure.Repositories.Command.Base;
/*
 * File: ReservationCommandRepository.cs
 * Author:Bartholomeusz S.V/IT20274702
*/
namespace TRT.Infrastructure.Repositories.Command
{
    public class ReservationCommandRepository : CommandRepository<Reservation>, IReservationCommandRepository
    {
        public ReservationCommandRepository(TRTContext context)
            : base(context)
        {

        }
    }
}
