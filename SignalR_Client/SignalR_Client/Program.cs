﻿using System;
using System.Threading;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client;

namespace SignalR_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting client  http://localhost:8089");
            
            var hubConnection = new HubConnection("http://localhost:8089/");
            //hubConnection.TraceLevel = TraceLevels.All;
            //hubConnection.TraceWriter = Console.Out;
            IHubProxy myHubProxy = hubConnection.CreateHubProxy("MyHub");

            myHubProxy.On<string, string>("addMessage", (name, message) => Console.Write("Recieved addMessage: " + name + ": " + message + "\n"));
            myHubProxy.On("heartbeat", () => Console.Write("Recieved heartbeat \n"));
 
            hubConnection.Start().Wait();

            while (true)
            {
                string key = Console.ReadLine();
                if (key.ToUpper() == "W")
                {
                    myHubProxy.Invoke("addMessage", "client message", " sent from console client").ContinueWith(task =>
                    {
                        if (task.IsFaulted)
                        {
                            Console.WriteLine("!!! There was an error opening the connection:{0} \n", task.Exception.GetBaseException());
                        }

                    }).Wait();
                    Console.WriteLine("Client Sending addMessage to server\n");
                }
                if (key.ToUpper() == "E")
                {
                    myHubProxy.Invoke("Heartbeat").ContinueWith(task =>
                    {
                        if (task.IsFaulted)
                        {
                            Console.WriteLine("There was an error opening the connection:{0}", task.Exception.GetBaseException());
                        }

                    }).Wait();
                    Console.WriteLine("client heartbeat sent to server\n");
                }
                if (key.ToUpper() == "C")
                {
                    break;
                }
            }

        }
    }
}