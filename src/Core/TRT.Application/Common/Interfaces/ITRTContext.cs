using MongoDB.Driver;
using TRT.Domain.Entities;
/*
 * File: ITRTContext.cs
 * Author: Dunusinghe A.V/IT20025526
*/
namespace TRT.Application.Common.Interfaces
{
    public interface ITRTContext
    {
        IMongoCollection<User> Users { get; }
        IMongoCollection<Schedule> Schedules { get; }
        IMongoCollection<Train> Trains { get; }
        IMongoCollection<Station> Stations { get; }
        IMongoCollection<TrainTicketPrice> TrainTicketPrices { get; }
        IMongoCollection<Reservation> Reservations { get; }

    }
}
