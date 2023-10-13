using TRT.Domain.Entities;
using TRT.Domain.Repositories.Command.Base;
/*
 * File: IReservationCommandRepository.cs
 * Author:Bartholomeusz S.V/IT20274702
*/
namespace TRT.Domain.Repositories.Command
{
    public interface IReservationCommandRepository : ICommandRepository<Reservation>
    {
    }
    
}
