using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TRT.Application.Common.Interfaces;
using TRT.Domain.Entities;
using TRT.Infrastructure.Common.Constants;
using TRT.Infrastructure.Data.Configuration;
/*
 * File: TRTContext.cs
 * Author: Dunusinghe A.V/IT20025526
*/
namespace TRT.Infrastructure.Data
{
    public class TRTContext : ITRTContext
    {
        private readonly IMongoDatabase _database;

        public TRTContext(IOptions<DataSourceCongifuration> options)
        {
           
            var client = new MongoClient(options.Value.DefaultConnection);
            _database = client.GetDatabase(options.Value.Database);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>(CollectionConstant.User);

        public IMongoCollection<Schedule> Schedules => _database.GetCollection<Schedule>(CollectionConstant.Schedule);

        public IMongoCollection<Train> Trains => _database.GetCollection<Train>(CollectionConstant.Train);

        public IMongoCollection<Station> Stations => _database.GetCollection<Station>(CollectionConstant.Station);

        public IMongoCollection<TrainTicketPrice> TrainTicketPrices => _database.GetCollection<TrainTicketPrice>(CollectionConstant.TrainTicketPrice);

        public IMongoCollection<Reservation> Reservations => _database.GetCollection<Reservation>(CollectionConstant.Reservation);
    }

}
   

