using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TRT.Application.Common.Interfaces;
using TRT.Domain.Entities;
using TRT.Infrastructure.Data.Configuration;

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

        public IMongoCollection<User> Users => _database.GetCollection<User>("User");

        public IMongoCollection<Schedule> Schedules => _database.GetCollection<Schedule>("Schedule");

        public IMongoCollection<Train> Trains => _database.GetCollection<Train>("Train");

        public IMongoCollection<Station> Stations => _database.GetCollection<Station>("Station");
    }

}
   

