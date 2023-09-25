using Microsoft.Extensions.Configuration;
using TRT.Application.Common.Interfaces;
using TRT.Domain.Repositories.Command;
using TRT.Domain.Repositories.Command.Base;
using TRT.Domain.Repositories.Query;
using TRT.Domain.Repositories.Query.Base;
using TRT.Infrastructure.Data;
using TRT.Infrastructure.Data.Configuration;
using TRT.Infrastructure.Repositories.Command;
using TRT.Infrastructure.Repositories.Command.Base;
using TRT.Infrastructure.Repositories.Query;
using TRT.Infrastructure.Repositories.Query.Base;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            
            services.AddScoped<TRTContext>()
                    .Configure<DataSourceCongifuration>(options =>
                    {
                        options.DefaultConnection = configuration.GetSection("DatabaseSettings:DefaultConnection").Value;
                        options.Database = configuration.GetSection("DatabaseSettings:DatabaseName").Value;
                    });

            services.AddTransient<ITRTContext>(provider => provider.GetRequiredService<TRTContext>());
           
            services.AddScoped(typeof(IQueryRepository<>), typeof(QueryRepository<>));
            services.AddScoped(typeof(ICommandRepository<>), typeof(CommandRepository<>));

            services.AddTransient<IUserQueryRepository, UserQueryRepository>();
            services.AddTransient<IUserCommandRepository, UserCommandRepository>();

            return services;
        }
    }
}
