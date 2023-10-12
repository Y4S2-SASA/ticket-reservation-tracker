using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using TRT.Domain.Entities;

namespace TRT.Infrastructure.Data
{
    public class TRTContextInitialiser
    {
        private readonly TRTContext _context;
        private readonly ILogger<TRTContextInitialiser> _logger;
        public TRTContextInitialiser(TRTContext _context, ILogger<TRTContextInitialiser> logger)
        {
            this._context = _context; 
            this._logger = logger;
        }

        public async Task SeedAsync()
        {
            try
            {
                await SeedUsersAsync();
                await SeedStationsAsync();
                await SeedTicketPriceAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initialising the database.");
                throw;
            }
        }

        private async Task SeedUsersAsync()
        {
            if(!_context.Users.Find(p => true).Any())
            {
                var backOffice = new User()
                {
                    NIC = "1111",
                    FirstName = "Super",
                    LastName = "Admin",
                    UserName = "superadmin",
                    MobileNumber = "0703375581",
                    Email = "admin@trt.com",
                    Role = Domain.Enums.Role.BackOffice,
                    Status = Domain.Enums.Status.Activated,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("123"),
                };

                await _context.Users.InsertOneAsync(backOffice);

                var travelAgent = new User()
                {
                    NIC = "2222",
                    FirstName = "Travel",
                    LastName = "Agent",
                    UserName = "travelagent",
                    MobileNumber = "0703375580",
                    Email = "travelgent@trt.com",
                    Role = Domain.Enums.Role.TravelAgent,
                    Status = Domain.Enums.Status.Activated,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("123"),
                };

                await _context.Users.InsertOneAsync(travelAgent);

                var traveler = new User()
                {
                    NIC = "3333",
                    FirstName = "Traveler",
                    LastName = string.Empty,
                    UserName = "traveler",
                    MobileNumber = "0703375580",
                    Email = "traveler@trt.com",
                    Role = Domain.Enums.Role.Traveler,
                    Status = Domain.Enums.Status.Activated,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("123"),
                };

                await _context.Users.InsertOneAsync(traveler);
            }
        }
        private async Task SeedStationsAsync()
        {
            if(!_context.Stations.Find(p => true).Any())
            {
                var data = new List<Station>
                {
                    new Station { Name = "Maradana", Code = "MDA", City = "Colombo", Elevation = 5.46M, Distance = 2.08M, Line = Domain.Enums.Line.MainLine },
                    new Station { Name = "Dematagoda", Code = "DAG", City = "Colombo", Elevation = 3.05M, Distance = 4.54M, Line = Domain.Enums.Line.MainLine },
                    new Station { Name = "Kelaniya", Code = "KLA", City = "Gampaha", Elevation = 3.96M, Distance = 7.72M, Line = Domain.Enums.Line.MainLine },
                    new Station { Name = "Wanawasala", Code = "WSL", City = "Gampaha", Elevation = 3.25M, Distance = 9.42M, Line = Domain.Enums.Line.MainLine },
                    new Station { Name = "Hunupitiya", Code = "HUN", City = "Gampaha", Elevation = 3.04M, Distance = 10.84M, Line = Domain.Enums.Line.MainLine },
                    new Station { Name = "Ederamulla", Code = "EDM", City = "Gampaha", Elevation = 3.18M, Distance = 12.58M, Line = Domain.Enums.Line.MainLine },
                    new Station { Name = "Horape", Code = "HRP", City = "Gampaha", Elevation = 3.52M, Distance = 14.98M, Line = Domain.Enums.Line.MainLine },
                    new Station { Name = "Ragama Junction", Code = "RGM", City = "Gampaha", Elevation = 3.65M, Distance = 16.42M, Line = Domain.Enums.Line.MainLine },
                    new Station { Name = "Walpola", Code = "WPA", City = "Gampaha", Elevation = 4.25M, Distance = 19M, Line = Domain.Enums.Line.MainLine },
                    new Station { Name = "Batuwaththa", Code = "BTU", City = "Gampaha", Elevation = 4.3M, Distance = 20.08M, Line = Domain.Enums.Line.MainLine },
                    new Station { Name = "Bulugahagoda", Code = "BGU", City = "Gampaha", Elevation = 8.05M, Distance = 21.64M, Line = Domain.Enums.Line.MainLine },
                    new Station { Name = "Ganemulla", Code = "GAN", City = "Gampaha", Elevation = 9.45M, Distance = 23.44M, Line = Domain.Enums.Line.MainLine },
                    new Station { Name = "Yagoda", Code = "YGD", City = "Gampaha", Elevation = 9.99M, Distance = 25.28M, Line = Domain.Enums.Line.MainLine },
                    new Station { Name = "Gampaha", Code = "GPH", City = "Gampaha", Elevation = 10.97M, Distance = 28.4M, Line = Domain.Enums.Line.MainLine },
                    new Station { Name = "Daraluwa", Code = "DRL", City = "Gampaha", Elevation = 11.05M, Distance = 30.8M, Line = Domain.Enums.Line.MainLine },
                    new Station { Name = "Bemmulla", Code = "BEM", City = "Gampaha", Elevation = 12.25M, Distance = 32.78M, Line = Domain.Enums.Line.MainLine },
                    new Station { Name = "Magalegoda", Code = "MGG", City = "Gampaha", Elevation = 14.75M, Distance = 35.03M, Line = Domain.Enums.Line.MainLine },
                    new Station { Name = "Heendeniya Pattiyagoda", Code = "HDP", City = "Gampaha", Elevation = 14.89M, Distance = 36.5M, Line = Domain.Enums.Line.MainLine },
                    new Station { Name = "Veyangoda", Code = "VGD", City = "Gampaha", Elevation = 18.59M, Distance = 38.3M, Line = Domain.Enums.Line.MainLine },
                    new Station { Name = "Wadurawa", Code = "WRW", City = "Gampaha", Elevation = 19.31M, Distance = 40.14M, Line = Domain.Enums.Line.MainLine },
                    new Station { Name = "Keenawala", Code = "KEN", City = "Gampaha", Elevation = 20.75M, Distance = 42.36M, Line = Domain.Enums.Line.MainLine },
                    new Station { Name = "Pallewela", Code = "PLL", City = "Gampaha", Elevation = 27.42M, Distance = 44.7M, Line = Domain.Enums.Line.MainLine },
                    new Station { Name = "Ganegoda", Code = "GND", City = "Gampaha", Elevation = 27.43M, Distance = 46.98M, Line = Domain.Enums.Line.MainLine },
                    new Station { Name = "Wijaya Rajadahana", Code = "WRD", City = "Gampaha", Elevation = 30.83M, Distance = 49.32M, Line = Domain.Enums.Line.MainLine },
                    new Station { Name = "Meerigama", Code = "MIG", City = "Gampaha", Elevation = 50M, Distance = 51.04M, Line = Domain.Enums.Line.MainLine },
                    new Station { Name = "Wilwatta", Code = "WWT", City = "Gampaha", Elevation = 53.69M, Distance = 52.86M, Line = Domain.Enums.Line.MainLine },
                    new Station { Name = "Botale", Code = "BTL", City = "Gampaha", Elevation = 51.37M, Distance = 54.9M, Line = Domain.Enums.Line.MainLine },
                    new Station { Name = "Ambepussa", Code = "APS", City = "Gampaha", Elevation = 55.48M, Distance = 56.98M, Line = Domain.Enums.Line.MainLine },
                    new Station { Name = "Yaththalgoda", Code = "YTG", City = "Kurunegala", Elevation = 56.37M, Distance = 60.58M, Line = Domain.Enums.Line.MainLine },
                    new Station { Name = "Bujjomuwa", Code = "BJM", City = "Kurunegala", Elevation = 56.98M, Distance = 62.66M, Line = Domain.Enums.Line.MainLine },
                    new Station { Name = "Alawwa", Code = "ALW", City = "Kurunegala", Elevation = 57.92M, Distance = 66.48M, Line = Domain.Enums.Line.MainLine },
                    new Station { Name = "Walakumbura", Code = "WKA", City = "Kurunegala", Elevation = 56.52M, Distance = 70.52M, Line = Domain.Enums.Line.MainLine },
                    new Station { Name = "Polgahawela Junction", Code = "PLG", City = "Kurunegala", Elevation = 74.39M, Distance = 73.92M, Line = Domain.Enums.Line.MainLine },
                    new Station { Name = "Panaliya", Code = "PNL", City = "Kurunegala", Elevation = 74.11M, Distance = 77.5M, Line = Domain.Enums.Line.MainLine },
                    new Station { Name = "Tismalpola", Code = "TSM", City = "Kegalle", Elevation = 76.71M, Distance = 79.74M, Line = Domain.Enums.Line.MainLine },
                    new Station { Name = "Korossa", Code = "KSA", City = "Kegalle", Elevation = 0M, Distance = 80.5M, Line = Domain.Enums.Line.MainLine },
                    new Station { Name = "Yatagama", Code = "YTM", City = "Kegalle", Elevation = 77.06M, Distance = 82.02M, Line = Domain.Enums.Line.MainLine },
                    new Station { Name = "Rambukkana", Code = "RBK", City = "Kegalle", Elevation = 88.42M, Distance = 85.14M, Line = Domain.Enums.Line.MainLine },
                    new Station { Name = "Kadigamuwa", Code = "KMA", City = "Kegalle", Elevation = 194.81M, Distance = 90.14M, Line = Domain.Enums.Line.MainLine },
                    new Station { Name = "Hali-Ela", Code = "HEA", City = "Badulla", Elevation = 732.71M, Distance = 285.92M, Line = Domain.Enums.Line.MainLine },
                    new Station { Name = "Badulla", Code = "BAD", City = "Badulla", Elevation = 652.43M, Distance = 291.6M, Line = Domain.Enums.Line.MainLine },

                    new Station { Name = "Peradeniya Junction", Code = "PDA", City = "Kandy", Elevation = 473.47M, Distance = 115.34M, Line = Domain.Enums.Line.MathaleLine },
                    new Station { Name = "Sarasavi Uyana", Code = "SUA", City = "Kandy", Elevation = 479.26M, Distance = 117M, Line = Domain.Enums.Line.MathaleLine },
                    new Station { Name = "Rajawatte", Code = "RJT", City = "Kandy", Line = Domain.Enums.Line.MathaleLine }, 
                    new Station { Name = "Randles Hill", Code = "RHL", City = "Kandy", Line = Domain.Enums.Line.MathaleLine },
                    new Station { Name = "Suduhumpola", Code = "SDH", City = "Kandy", Line = Domain.Enums.Line.MathaleLine }, 
                    new Station { Name = "Kandy", Code = "KDT", City = "Kandy", Elevation = 488.41M, Distance = 119.5M, Line = Domain.Enums.Line.MathaleLine },
                    new Station { Name = "Asgirya", Code = "ASG", City = "Kandy", Line = Domain.Enums.Line.MathaleLine }, 
                    new Station { Name = "Mahiyawa", Code = "MYA", City = "Kandy", Elevation = 526.21M, Distance = 121.16M, Line = Domain.Enums.Line.MathaleLine },
                    new Station { Name = "Katugastota Road", Code = "KTR", City = "Kandy", Line = Domain.Enums.Line.MathaleLine }, 
                    new Station { Name = "Mavilmada", Code = "MVM", City = "Kandy", Line = Domain.Enums.Line.MathaleLine }, 
                    new Station { Name = "Katugastota", Code = "KTG", City = "Kandy", Elevation = 467.68M, Distance = 125.02M, Line = Domain.Enums.Line.MathaleLine },
                    new Station { Name = "Pallethalawinna", Code = "PLW", City = "Kandy", Line = Domain.Enums.Line.MathaleLine }, 
                    new Station { Name = "Udathalawinna", Code = "UDL", City = "Kandy", Line = Domain.Enums.Line.MathaleLine }, 
                    new Station { Name = "Meegammana", Code = "MGM", City = "Kandy", Line = Domain.Enums.Line.MathaleLine }, 
                    new Station { Name = "Yatirawana", Code = "YRW", City = "Kandy", Line = Domain.Enums.Line.MathaleLine }, 
                    new Station { Name = "Wattegama", Code = "WGA", City = "Kandy", Elevation = 493.9M, Distance = 131.6M, Line = Domain.Enums.Line.MathaleLine },
                    new Station { Name = "Yatawara", Code = "YTA", City = "Kandy", Line = Domain.Enums.Line.MathaleLine }, 
                    new Station { Name = "Pathanpaha", Code = "PTP", City = "Matale", Line = Domain.Enums.Line.MathaleLine }, 
                    new Station { Name = "Marukona", Code = "MRK", City = "Matale", Line = Domain.Enums.Line.MathaleLine }, 
                    new Station { Name = "Udaththawala", Code = "UDW", City = "Matale", Line = Domain.Enums.Line.MathaleLine }, 
                    new Station { Name = "Ukuwela", Code = "UKL", City = "Matale", Elevation = 393.9M, Distance = 141.6M, Line = Domain.Enums.Line.MathaleLine },
                    new Station { Name = "Tawalankoya", Code = "TWY", City = "Matale", Line = Domain.Enums.Line.MathaleLine }, 
                    new Station { Name = "Elwala", Code = "ELW", City = "Matale", Line = Domain.Enums.Line.MathaleLine }, 
                    new Station { Name = "Kohobiliwala", Code = "KHB", City = "Matale", Line = Domain.Enums.Line.MathaleLine }, 
                    new Station { Name = "Matale", Code = "MTL", City = "Matale", Elevation = 351.21M, Distance = 147.14M, Line = Domain.Enums.Line.MathaleLine },

                    new Station { Name = "Ragama Junction", Code = "RGM", City = "Gampaha", Elevation = 3.65m, Distance = 16.42m, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Peralanda", Code = "PRL", City = "Gampaha", Elevation = 0, Distance = 17, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Kandana", Code = "KAN", City = "Gampaha", Elevation = 5.79m, Distance = 17.42m, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Kapuwatta", Code = "KAW", City = "Gampaha", Elevation = 0, Distance = 19.44m, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Ja-Ela", Code = "JLA", City = "Gampaha", Elevation = 5.18m, Distance = 21.04m, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Thudella", Code = "TUD", City = "Gampaha", Elevation = 0, Distance = 22.8m, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Kudahakkapola", Code = "KUD", City = "Gampaha", Elevation = 0, Distance = 23.92m, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Alawathupitiya", Code = "AWP", City = "Gampaha", Elevation = 0, Distance = 25.38m, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Seeduwa", Code = "SED", City = "Gampaha", Elevation = 4.87m, Distance = 26.92m, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Liyanagemulla", Code = "LGM", City = "Gampaha", Elevation = 0, Distance = 29.02m, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Inventment Promotion Zone", Code = "IPZ", City = "Gampaha", Elevation = 0, Distance = 30.4m, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Colombo Airport Katunayake", Code = "CAK", City = "Gampaha", Elevation = 0, Distance = 31.80m, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Katunayaka", Code = "KTK", City = "Gampaha", Elevation = 5.79m, Distance = 32.3m, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Kurana", Code = "KUR", City = "Gampaha", Elevation = 0, Distance = 34.14m, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Negombo", Code = "NGB", City = "Gampaha", Elevation = 2.18m, Distance = 37.64m, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Kattuwa", Code = "KAT", City = "Gampaha", Elevation = 0, Distance = 40.94m, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Kochchikade", Code = "KCH", City = "Gampaha", Elevation = 8.84m, Distance = 43.84m, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Waikkala", Code = "WKL", City = "Puttalam", Elevation = 0, Distance = 45.42m, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Bolawatta", Code = "BLT", City = "Puttalam", Elevation = 7.62m, Distance = 47.64m, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Boralessa", Code = "BSA", City = "Puttalam", Elevation = 0, Distance = 50.14m, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Lunuwila", Code = "LWL", City = "Puttalam", Elevation = 7.01m, Distance = 53.08m, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Thummodara", Code = "TDR", City = "Puttalam", Elevation = 0, Distance = 58m, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Nattandiya", Code = "NAT", City = "Puttalam", Elevation = 5.18m, Distance = 60.08m, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Walahapitiya", Code = "WHP", City = "Puttalam", Elevation = 0, Distance = 63.92m, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Kudawewa", Code = "KWW", City = "Puttalam", Elevation = 8.84m, Distance = 66.46m, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Nelumpokuna", Code = "NPK", City = "Puttalam", Elevation = 0, Distance = 67.21m, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Madampe", Code = "MDP", City = "Puttalam", Elevation = 6.09m, Distance = 70.64m, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Kakkapalliya", Code = "KYA", City = "Puttalam", Elevation = 0, Distance = 75.04m, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Sawarana", Code = "SWR", City = "Puttalam", Elevation = 0, Distance = 78.24m, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Chilaw", Code = "CHL", City = "Puttalam", Elevation = 0, Distance = 81.04m, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Manuwangama", Code = "MNG", City = "Puttalam", Elevation = 0, Distance = 86.4m, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Bangadeniya", Code = "BGY", City = "Puttalam", Elevation = 0, Distance = 89.6m, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Arachchikattuwa", Code = "AKT", City = "Puttalam", Elevation = 0, Distance = 92.8m, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Anawilundawa", Code = "", City = "Puttalam", Elevation = 0, Distance = 0, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Battuluoya", Code = "BOA", City = "Puttalam", Elevation = 4.57m, Distance = 99.2m, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Pulichchikulam", Code = "PCK", City = "Puttalam", Elevation = 0, Distance = 103.7m, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Mundel", Code = "MNL", City = "Puttalam", Elevation = 4.57m, Distance = 108.24m, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Mangala Eliya", Code = "MGE", City = "Puttalam", Elevation = 0, Distance = 115.14m, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Madurankuli", Code = "MKI", City = "Puttalam", Elevation = 2.74m, Distance = 120m, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Erukkalampiddy", Code = "", City = "Puttalam", Elevation = 0, Distance = 0, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Palavi", Code = "PVI", City = "Puttalam", Elevation = 3.04m, Distance = 128.62m, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Thillayadi", Code = "", City = "Puttalam", Elevation = 0, Distance = 0, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Puttalam", Code = "PTM", City = "Puttalam", Elevation = 2.74m, Distance = 133.24m, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Noor Nagar", Code = "NOR", City = "Puttalam", Elevation = 0, Distance = 134.4m, Line = Domain.Enums.Line.PuttalamLine },
                    new Station { Name = "Karadipooval", Code = "KPL", City = "Puttalam", Elevation = 0, Distance = 0, Line = Domain.Enums.Line.PuttalamLine },

                    new Station { Name = "Kirulapone", Code = "KPE", City = "Colombo", Elevation = 0, Distance = 7.26m, Line = Domain.Enums.Line.KelaniValleyLine },
                    new Station { Name = "Nugegoda", Code = "NUG", City = "Colombo", Elevation = 3.96m, Distance = 9.04m, Line = Domain.Enums.Line.KelaniValleyLine },
                    new Station { Name = "Pengiriwatte", Code = "", City = "Colombo", Elevation = 0, Distance = 0, Line = Domain.Enums.Line.KelaniValleyLine },
                    new Station { Name = "Udahamulla", Code = "UHM", City = "Colombo", Elevation = 0, Distance = 11.46m, Line = Domain.Enums.Line.KelaniValleyLine },
                    new Station { Name = "Nawinna", Code = "NWN", City = "Colombo", Elevation = 10.67m, Distance = 13.14m, Line = Domain.Enums.Line.KelaniValleyLine },
                    new Station { Name = "Kiriwadala", Code = "", City = "Colombo", Elevation = 0, Distance = 57.24m, Line = Domain.Enums.Line.KelaniValleyLine },
                    new Station { Name = "Avissawella", Code = "AVS", City = "Colombo", Elevation = 39.63m, Distance = 58.98m, Line = Domain.Enums.Line.KelaniValleyLine }


                };

                await _context.Stations.InsertManyAsync(data);
            }
        }
        private async Task SeedTicketPriceAsync()
        {
            if (!_context.TrainTicketPrices.Find(p => true).Any())
            {
                var firstClassPrice = new TrainTicketPrice()
                {
                    PassengerClass = Domain.Enums.PassengerClass.FirstClass,
                    Price = 300,
                };

                await _context.TrainTicketPrices.InsertOneAsync(firstClassPrice);

                var secondClassPrice = new TrainTicketPrice()
                {
                    PassengerClass = Domain.Enums.PassengerClass.SecondClass,
                    Price = 100,
                };

                await _context.TrainTicketPrices.InsertOneAsync(secondClassPrice);

                var thirdClassPrice = new TrainTicketPrice()
                {
                    PassengerClass = Domain.Enums.PassengerClass.ThirdClass,
                    Price = 50,
                };

                await _context.TrainTicketPrices.InsertOneAsync(thirdClassPrice);
            }
               
        }
    }
}
