using System;
using System.Configuration;
using Akka.Actor;
using Akka.Configuration;
using Akka.Configuration.Hocon;

using Cluster.Common.Actors;
using System.Threading;

namespace Samples.Cluster.Simple
{
    class Program
    {
        private static void Main(string[] args)
        {
            StartUp(args.Length == 0 ? new String[] { "8888" } : args);
            //StartUp(args.Length == 0 ? new String[] { "2553" } : args);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        public static void StartUp(string[] ports)
        {
            var section = (AkkaConfigurationSection)ConfigurationManager.GetSection("akka");

            //Override the configuration of the port
            var config =
                ConfigurationFactory.ParseString("akka.remote.helios.tcp.port=" + ports[0])
                    .WithFallback(section.AkkaConfig);

            //create an Akka system
            var system = ActorSystem.Create("ClusterSystem", config);

            //create an actor that send message to another
            system.ActorOf(Props.Create(typeof(SendMessageActor)), "messageActor");
        }
    }
}

