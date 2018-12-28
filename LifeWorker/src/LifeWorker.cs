// Copyright (c) Improbable Worlds Ltd, All Rights Reserved

using System;
using System.Reflection;
using Improbable.Worker;
using Improbable.Collections;
using Improbable;

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
            int liveNeighborCount = 0;

            //Iterate through every entity in the view
            foreach(Entity e in view.Entities.Values)
            {
                //Only do this update if this worker has write access to the entity
                

                //Get the number of live neighbors
                liveNeighborCount = GetLiveNeighbors(e, view);
                //Update the entity based on the live neighbor count
                UpdateEntity(e, liveNeighborCount);
            }
        }

        /// <summary>
        /// Get a count of live neighbors by running through the list of neighbors from the entity
        /// </summary>
        /// <param name="e"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        private static int GetLiveNeighbors(Entity e, View view)
        {
            Entity neighbor;
            bool hasNeighbor = false;
            int liveNeighbors = 0;

            //Get the current time sequence id
            ulong seqId = GetLatestSequenceId(e);

            //Get the list of neighbor entity ids
            var option = e.Get<Neighbors>();
            var neighbors = option.Value;
            var neighborsData = neighbors.Get();
            NeighborsData nd = neighborsData.Value;

            //Go through the entity IDs of the neighbors
            foreach(Improbable.EntityId neighborId in nd.neighborList)
            {
                //Get the life value for an entityID
                hasNeighbor = view.Entities.TryGetValue(neighborId, out neighbor);
                if(!hasNeighbor)
                {
                    //How to get the neighbor if it isn't in the view?
                    //This should only be for outer edge items, which this worker should only have read and not write, right?
                }
                
                if(IsCellAlive(seqId, neighbor))
                {
                    //What if that cell isn't at that time sequence ID yet?
                    liveNeighbors++;
                }
            }

            return liveNeighbors;
        }

        /// <summary>
        /// Get the latest time-based sequence id from an entity
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private static ulong GetLatestSequenceId(Entity e)
        {
            var option = e.Get<Life>();
            var life = option.Value;
            var lifeData = life.Get();
            LifeData ld = lifeData.Value;
            return ld.curSequenceId;
        }

        /// <summary>
        /// Determine if a cell is alive for the provided time sequence ID
        /// </summary>
        /// <param name="sequenceId"></param>
        /// <param name="cellEntity"></param>
        /// <returns></returns>
        private static bool IsCellAlive(ulong sequenceId, Entity cellEntity)
        {
            bool isAlive = false;

            var option = cellEntity.Get<Life>();
            var life = option.Value;
            var lifeData = life.Get();
            LifeData ld = lifeData.Value;

            if(ld.curSequenceId == sequenceId)
            {
                isAlive = ld.curIsAlive;
            }
            else if(ld.prevSequenceId == sequenceId)
            {
                isAlive = ld.prevIsAlive;
            }

            return isAlive;
        }

        /// <summary>
        /// Update an entity based on the rules of Conways Game of Life
        /// </summary>
        /// <param name="e">The entity to update</param>
        private static void UpdateEntity(Entity e, int LiveNeighborCount)
        {
            //Get Current life state of the entity
            bool isAlive = false;
            bool isAliveNextState = false;

            // If current state = is Alive && (If LiveNeighborCount < 2 OR LiveNeighborCount > 3) Then Update to next state = dead
            if(isAlive && (LiveNeighborCount < 2 || LiveNeighborCount > 3))
            {
                isAliveNextState = false;
            }
            //If current state = Dead(is not alive) &&  If LiveNeighborCount == 3 then Update to next state = alive
            else if ((!isAlive) && (LiveNeighborCount == 3))
            {
                isAliveNextState = true;
            }

            //set prev state and count id to the current state/id
            //Set new current state


        }
    }
}
