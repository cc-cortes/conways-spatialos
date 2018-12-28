// Copyright (c) Improbable Worlds Ltd, All Rights Reserved

using System;
using System.Reflection;
using Improbable.Worker;

namespace Demo
{
    class HelloWorker
    {
        // https://github.com/spatialos/FlexibleProjectExample/blob/master/HelloWorker/src/HelloWorker.cs

        private const string WorkerType = "LifeWorker";
        private const string LoggerName = "LifeWorker.cs";
        private const int ErrorExitStatus = 1;
        private const uint GetOpListTimeoutInMilliseconds = 100;
        const int FramesPerSecond = 1;
        private static bool IsConnected;

        static int Main(string[] arguments)
        {
            //"${IMPROBABLE_RECEPTIONIST_HOST}", "${IMPROBABLE_RECEPTIONIST_PORT}", "${IMPROBABLE_WORKER_ID}" defined as passed in as part of config
            string hostName = arguments[0];
            ushort port = Convert.ToUInt16(arguments[1]);
            string workerId = arguments[2];

            Assembly.Load("GeneratedCode");

            Console.WriteLine("Worker Starting...");
            using (var connection = ConnectWorker(workerId, hostName, port))
            {
                using (var view = new View())
                {
                    IsConnected = true;

                    view.OnDisconnect(op =>
                    {
                        Console.Error.WriteLine("[disconnect] {0}", op.Reason);
                        IsConnected = false;
                    });

                    view.OnLogMessage(op =>
                    {
                        connection.SendLogMessage(op.Level, LoggerName, op.Message);
                        Console.WriteLine("Log Message: {0}", op.Message);
                        if (op.Level == LogLevel.Fatal)
                        {
                            Console.Error.WriteLine("Fatal error: {0}", op.Message);
                            Environment.Exit(ErrorExitStatus);
                        }
                    });


                    RunEventLoop(connection, view);
                }
                
            }

            return 0;
        }

        private static Connection ConnectWorker(string workerId, string hostName, ushort port)
        {
            // https://docs.improbable.io/reference/13.3/csharpsdk/using/connecting

            var connectionParameters = new ConnectionParameters();
            connectionParameters.WorkerType = WorkerType;
            connectionParameters.Network.ConnectionType = NetworkConnectionType.Tcp;

            using (var future = Connection.ConnectAsync(hostName, port, workerId, connectionParameters))
            {
                return future.Get();
            }
        }

        private static void RunEventLoop(Connection connection, View view)
        {
            var maxWait = System.TimeSpan.FromMilliseconds(1000f / FramesPerSecond); //When does QoS hit, are there heartbeat ops that need to go out faster than a second?
            var stopwatch = new System.Diagnostics.Stopwatch();
            while (IsConnected)
            {
                stopwatch.Reset();
                stopwatch.Start();
                var opList = connection.GetOpList(0 /* non-blocking */);
                // Invoke callbacks.
                view.Process(opList);
                // Do other work here...
                UpdateEntities(view);
                stopwatch.Stop();
                var waitFor = maxWait.Subtract(stopwatch.Elapsed);
                System.Threading.Thread.Sleep(waitFor.Milliseconds > 0 ? waitFor : System.TimeSpan.Zero);
            }
        }

        /// <summary>
        /// Runs through the current view and sends updates to the entities it has write authority over
        /// </summary>
        /// <param name="view"></param>
        private static void UpdateEntities(View view)
        {
            //Iterate through every entity in the view
            foreach(Entity e in view.Entities.Values)
            {
                UpdateEntity(e);
            }
        }

        /// <summary>
        /// Update an entity based on the rules of Conways Game of Life
        /// </summary>
        /// <param name="e">The entity to update</param>
        private static void UpdateEntity(Entity e)
        {
            //GetAvailableNeighborCount... What if you don't have all neighbors?
            int availableNeighborCount = 0;

            //GetLiveNeighborCount 
            int liveNeighborCount = 0;

            //Get Current life state of the entity
            bool isAlive = false;

            // If current state = is Alive && (If LiveNeighborCount < 2 OR LiveNeighborCount > 3) Then Update to next state = dead
            if(isAlive && (liveNeighborCount < 2 || liveNeighborCount > 3))
            {
                //set prev state and count id to the current state/id
                //Set new current state
            }
            //If current state = Dead(is not alive) &&  If LiveNeighborCount == 3 then Update to next state = alive
            else if ((!isAlive) && (liveNeighborCount == 3))
            {

            }

            
        }
    }
}
