using Akka.Actor;
using Akka.Cluster;
using Akka.Event;
using Cluster.Common.Message;

namespace Cluster.Common.Actors
{
    public class ListenerActor : UntypedActor
    {
        protected ILoggingAdapter Log = Context.GetLogger();
        protected Akka.Cluster.Cluster Cluster = Akka.Cluster.Cluster.Get(Context.System);

        /// <summary>
        /// Need to subscribe to cluster changes
        /// </summary>
        protected override void PreStart()
        {
            Cluster.Subscribe(Self, ClusterEvent.InitialStateAsEvents, new []{ typeof(ClusterEvent.IMemberEvent), typeof(ClusterEvent.UnreachableMember) });
        }

        /// <summary>
        /// Re-subscribe on restart
        /// </summary>
        protected override void PostStop()
        {
            Cluster.Unsubscribe(Self);
        }

        protected override void OnReceive(object message)
        {
            if (message is HelloMessage)
            {
                System.Console.WriteLine("Message received from Actor");
                //Context.ActorSelection("akka.tcp://ClusterSystem@127.0.0.1:8888/user/messageActor").Tell(Kill.Instance);
                //Context.ActorSelection("akka.tcp://ClusterSystem@127.0.0.1:8888/user/messageActor").Tell(new HelloMessage("Shout down"));
            }

            var up = message as ClusterEvent.MemberUp;
            if (up != null)
            {
                var mem = up;
                Log.Info("Member is Up: {0}", mem.Member);
            } else if(message is ClusterEvent.UnreachableMember)
            {
                var unreachable = (ClusterEvent.UnreachableMember) message;
                Log.Info("Member detected as unreachable: {0}", unreachable.Member);
            }
            else if (message is ClusterEvent.MemberRemoved)
            {
                var removed = (ClusterEvent.MemberRemoved) message;
                Log.Info("Member is Removed: {0}", removed.Member);
            }
            else if (message is ClusterEvent.IMemberEvent)
            {
                //IGNORE                
            }
            else
            {
                Unhandled(message);
            }
        }
    }
}

