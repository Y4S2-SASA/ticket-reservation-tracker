using MongoDB.Driver;
using TRT.Domain.Entities;

namespace TRT.Application.Common.Interfaces
{
    public interface ITRTContext
    {
        IMongoCollection<User> Users { get; }
        IMongoCollection<Schedule> Schedules { get; }
        IMongoCollection<Train> Trains { get; }
        IMongoCollection<Station> Stations { get; }

    }
}
