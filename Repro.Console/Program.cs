using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Repro.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var functionCaller = new Caller("https://functionrepro.azurewebsites.net/api/function");

            System.Console.WriteLine("How many runners?");
            var runnerCount = int.Parse(System.Console.ReadLine());

            System.Console.WriteLine($"Starting with {runnerCount} runners.");

            for (var i = 0; i <= runnerCount; i++)
                Task.Run(() => Call(functionCaller));
            

            System.Console.WriteLine("Press any key to stop...");
            while (!System.Console.KeyAvailable || functionCaller.ErrorList.Count < 10)
                Print(functionCaller);

            if (functionCaller.ErrorList.Any())
            {
                System.Console.WriteLine("Function has errors");
                foreach (var line in functionCaller.ErrorList)
                    System.Console.WriteLine(line);
            }
            System.Console.WriteLine("Press any key to exit...");
            System.Console.ReadLine();
        }

        private static void Print(Caller functionCaller)
        {
            System.Console.Clear();
            System.Console.ForegroundColor = functionCaller.Failed ? ConsoleColor.Red : ConsoleColor.Green;
            System.Console.WriteLine("==========================");
            System.Console.WriteLine($"Function Requests: {functionCaller.RequestCount}");
            System.Console.WriteLine($"Function Errors: {functionCaller.ErrorList.Count}");
            System.Console.WriteLine("==========================");
            Thread.Sleep(200);
        }

        private static async void Call(Caller caller)
        {
            while (caller.ErrorList.Count < 10)
            {
                await caller.Call();
            }
        }
    }

    public class Caller
    {
        private readonly HttpClient _httpClient;

        public int RequestCount { get; set; }

        public List<string> ErrorList { get; set; }

        public bool Failed { get; set; }

        public Caller(string urlToCall)
        {
            RequestCount = 0;
            ErrorList = new List<string>();
            _httpClient = new HttpClient {BaseAddress = new Uri(urlToCall)};
        }

        public async Task<string> Call()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "");
            
            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != HttpStatusCode.OK 
                && !string.IsNullOrEmpty(content)
                && !content.StartsWith("<!"))
            {
                Failed = true;
                ErrorList.Add(content);
            }

            RequestCount++;

            return response.StatusCode == HttpStatusCode.OK ? "" : content;
        }
    }
}
