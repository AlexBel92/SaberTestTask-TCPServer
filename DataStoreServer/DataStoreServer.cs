using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Net.Sockets;
using System.Threading.Tasks;
using DataStoreServer.Response;
using DataStore.Core.Exceptions;

namespace DataStoreServer
{
    public class DataStoreServer
    {
        private readonly IPAddress ipAddress;
        private readonly int port;
        private readonly RequestProcessingStrategy strategy;
        private readonly TcpListener listener;

        public DataStoreServer(TcpListener listener, RequestProcessingStrategy strategy)
        {            
            this.strategy = strategy;
            this.listener = listener;
            this.ipAddress = (listener.LocalEndpoint  as IPEndPoint).Address;
            this.port = (listener.LocalEndpoint as IPEndPoint).Port;
        }

        public async Task RunAsync()
        {
            listener.Start();
            Console.WriteLine($"TCP server ({GetStrategyTypeName()}) is now running  on {ipAddress}:{port}");            

            while (true)
            {
                await AcceptClientsAsync();
            }
        }

        private string GetStrategyTypeName()
        {
            return strategy.GetType().Name;
        }

        private async Task AcceptClientsAsync()
        {
            var tcpClient = await listener.AcceptTcpClientAsync();
            new Task(async () => await ProcessClientAsync(tcpClient)).Start();
        }

        private async Task ProcessClientAsync(TcpClient tcpClient)
        {
            StreamReader reader = null;
            StreamWriter writer = null;
            try
            {
                Console.WriteLine($"\n{GetStrategyTypeName()} - Received connection request from " + GetClientEndPoint(tcpClient));

                GetRWStreams(tcpClient, out reader, out writer);

                while (true)
                {
                    var request = await reader.ReadLineAsync();
                    Console.WriteLine($"{GetStrategyTypeName()} - Received request: \n" + request);

                    if (request is null)
                    {
                        await SendErrorResponse(writer, "Request cannot be null.");
                        break;
                    }

                    var response = await ProcessRequestAsync(request);

                    await SendSuccessResponse(writer, response);
                }
            }
            catch (Exception e) when (e is PersonValidationException or DuplicateKeyException or PersonNotFoundException)
            {
                await SendErrorResponse(writer, e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (tcpClient.Connected)                
                    await SendErrorResponse(writer, $"Exception then executing: {GetStrategyTypeName()} for client: {GetClientEndPoint(tcpClient)}");                
            }
            finally
            {
                if (tcpClient.Connected)
                    tcpClient.Close();
            }
        }

        private static void GetRWStreams(TcpClient tcpClient, out StreamReader reader, out StreamWriter writer)
        {
            var networkStream = tcpClient.GetStream();
            reader = new StreamReader(networkStream);
            writer = new StreamWriter(networkStream)
            {
                AutoFlush = true
            };
        }

        private static string GetClientEndPoint(TcpClient tcpClient)
        {
            return tcpClient.Client.RemoteEndPoint.ToString();
        }

        private async Task<string> ProcessRequestAsync(string request)
        {
            var response = await strategy.ExecuteAsync(request);
            Console.WriteLine($"{GetStrategyTypeName()} - Computed response is: \n" + response);

            return response;
        }

        private static async Task SendErrorResponse(StreamWriter writer, string error)
        {
            var response = new DataStoreServerResponse(false, string.Empty, error);
            await writer.WriteLineAsync(JsonSerializer.Serialize(response));
        }

        private static async Task SendSuccessResponse(StreamWriter writer, string response)
        {
            try
            {
                var serverResponse = new DataStoreServerResponse(true, response, string.Empty);
                await writer.WriteLineAsync(JsonSerializer.Serialize(serverResponse));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}