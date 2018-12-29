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

        /// <summary>
        /// Connect the Worker to the Runtime
        /// </summary>
        /// <param name="workerId"></param>
        /// <param name="hostName"></param>
        /// <param name="port"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Run a time-based tick loop to change the life state
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="view"></param>
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
                UpdateEntities(view, connection);
                stopwatch.Stop();
                var waitFor = maxWait.Subtract(stopwatch.Elapsed);
                System.Threading.Thread.Sleep(waitFor.Milliseconds > 0 ? waitFor : System.TimeSpan.Zero);
            }
        }

        /// <summary>
        /// Runs through the current view and sends updates to the entities it has write authority over
        /// </summary>
        /// <param name="view"></param>
        private static void UpdateEntities(View view, Connection connection)
        {
            int liveNeighborCount = 0;
            bool hasNext = true;
            bool canGetAllNeighbors = false;

            //Run through every id/entity pair in the view
            var enumerator = view.Entities.GetEnumerator();
            while(hasNext)
            {
                var pair = enumerator.Current;
                EntityId id = pair.Key;
                Entity e = pair.Value;

                //Only do this update if this worker has write access to the entity
                //Is there a way to determine that this worker has write access to an entity?

                //Get the number of live neighbors
                canGetAllNeighbors = TryGetLiveNeighbors(e, view, out liveNeighborCount);
                //Update the entity based on the live neighbor count
                if(canGetAllNeighbors)
                {
                    UpdateEntity(id, e, liveNeighborCount, connection);
                }
                hasNext = enumerator.MoveNext();
            }
        }

        /// <summary>
        /// Get a count of live neighbors by running through the list of neighbors from the entity
        /// </summary>
        /// <param name="e"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        private static bool TryGetLiveNeighbors(Entity e, View view, out int LiveNeighborCount)
        {
            Entity neighbor;
            bool canGetLiveNeighbors = true;
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
                    //This should only be for outer edge items, which this worker should only have read and not write
                    //Although there may be an issue of "View Completeness" in the beginning... so just say no and don't update the entity yet.
                    canGetLiveNeighbors = false;
                }
                
                if(IsCellAlive(seqId, neighbor))
                {
                    //What if that cell isn't at that time sequence ID yet?
                    liveNeighbors++;
                }
            }

            LiveNeighborCount = liveNeighbors;
            return canGetLiveNeighbors;
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
        private static void UpdateEntity(EntityId id, Entity e, int LiveNeighborCount, Connection connection)
        {
            //Get Current life state of the entity
            bool isAlive = IsCellAlive(GetLatestSequenceId(e), e);
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
            //If it doesn't meet the above criteria, then the previous state persists to the next time step
            else
            {
                isAliveNextState = isAlive;
            }

            //Set new current state
            UpdateLifeComponent(connection, id, e, isAliveNextState);
        }

        /// <summary>
        /// Update the Life component to the provided next state
        /// </summary>
        /// <param name="e"></param>
        /// <param name="isAliveNextState"></param>
        private static void UpdateLifeComponent(Connection connection, EntityId id, Entity e, bool isAliveNextState)
        {
            ulong curSequenceId = GetLatestSequenceId(e);
            bool curIsAlive = IsCellAlive(curSequenceId, e);

            //Create new component update object
            Life.Update lu = new Life.Update();

            //set prev state and count id to the current state and time id
            lu.SetPrevSequenceId(curSequenceId);
            lu.SetPrevIsAlive(curIsAlive);

            //Set new current state
            lu.SetCurIsAlive(isAliveNextState);
            lu.SetCurSequenceId(curSequenceId + 1);

            //Send the update
            connection.SendComponentUpdate<Life>(id, lu);
        }
    }
}
