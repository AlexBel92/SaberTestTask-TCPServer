using DataStoreServer.Factories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DataStoreServer
{
    class Program
    {
        static async Task Main(string[] args)
        { 
            try
            {
                var configuration = BuildConfig();

                var serverSettings = configuration.GetSection(ServerSettings.Position).Get<ServerSettings>();
                var ipAddress = serverSettings.GetIPAddress();
                var repositoryFactory = GetRepositoryFactory(configuration);

                var endPoints = new List<DataStoreServer>
                {
                    new DataStoreServer(new TcpListener(ipAddress, serverSettings.AddPortNumber), new AddPersonStrategy(repositoryFactory)),
                    new DataStoreServer(new TcpListener(ipAddress, serverSettings.UpdatePortNumber), new UpdatePersonStrategy(repositoryFactory)),
                    new DataStoreServer(new TcpListener(ipAddress, serverSettings.GetPortNumber), new GetPersonsStrategy(repositoryFactory))
                };

                foreach (var endPoint in endPoints)
                    endPoint.RunAsync();

                Console.WriteLine("Press <enter> to close...\n");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }

        private static IRepositoryFactory GetRepositoryFactory(IConfiguration configuration)
        {
            var inMemoryFlag = configuration.GetSection("InMemory").Get<bool>();
            IStorageContextFactory contextFactory =
                inMemoryFlag == false ?
                new MySqlContextFactory(configuration.GetConnectionString("MySql")) :
                new InMemoryContextFactory();
            return new EfRepositoryFactory(contextFactory);
        }

        static IConfiguration BuildConfig()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true);
            return builder.Build();
        }
    }
}
