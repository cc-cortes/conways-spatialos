// Copyright (c) Improbable Worlds Ltd, All Rights Reserved

using System;
using System.Reflection;
using Improbable.Worker;
using Improbable.Collections;
using Improbable;
using System.Diagnostics;
using System.Collections.Generic;

namespace Demo
{
    class LifeWorker //Change this to LifeWorker
    {
        // https://github.com/spatialos/FlexibleProjectExample/blob/master/HelloWorker/src/HelloWorker.cs

        private const string WorkerType = "LifeWorker";
        private const string LoggerName = "LifeWorker.cs";
        private const int ErrorExitStatus = 1;
        private const uint GetOpListTimeoutInMilliseconds = 100;
        //const int FramesPerSecond = 1;
        const int SecondsPerFrame = 2;
        private static bool IsConnected;
        private const string EntityTypeCellAlive = "Cell_Alive";
        private const string EntityTypeCellDead = "Cell_Dead";

        private static Connection WorkerConnection;
        private class ViewEntity
        {
            public bool hasAuthority;
            public Entity entity;
            public bool hasUpdate;
            public Life.Update lifeUpdate;
            public Metadata.Update metadataUpdate;
        }
        private static Dictionary<EntityId, ViewEntity> EntityView = new Dictionary<EntityId, ViewEntity>(); // THE ALMIGHTY VIEW
        private static System.Collections.Generic.List<ulong> sequenceIds = new System.Collections.Generic.List<ulong>(); //List of all current Sequence Ids, could probably split this out to a separate class with all the specific methods

        static int Main(string[] arguments)
        {
            //"${IMPROBABLE_RECEPTIONIST_HOST}", "${IMPROBABLE_RECEPTIONIST_PORT}", "${IMPROBABLE_WORKER_ID}" defined as passed in as part of config
            string hostName = arguments[0];
            ushort port = Convert.ToUInt16(arguments[1]);
            string workerId = arguments[2];

            Assembly.Load("GeneratedCode");

            using (var connection = ConnectWorker(workerId, hostName, port))
            {
                using (var dispatcher = new Dispatcher())
                {
                    IsConnected = true;

                    dispatcher.OnDisconnect(op =>
                    {
                        Console.Error.WriteLine("[disconnect] {0}", op.Reason);
                        IsConnected = false;
                    });

                    dispatcher.OnLogMessage(op =>
                    {
                        connection.SendLogMessage(op.Level, LoggerName, op.Message);
                        Console.WriteLine("Log Message: {0}", op.Message);
                        if (op.Level == LogLevel.Fatal)
                        {
                            Console.Error.WriteLine("Fatal error: {0}", op.Message);
                            Environment.Exit(ErrorExitStatus);
                        }
                    });

                    dispatcher.OnAuthorityChange<Life>(op =>
                    {
                        ViewEntity entity;
                        if (EntityView.TryGetValue(op.EntityId, out entity))
                        {
                            if(op.Authority == Authority.Authoritative)
                            {
                                entity.hasAuthority = true;
                            }
                            else if (op.Authority == Authority.NotAuthoritative)
                            {
                                entity.hasAuthority = false;
                            }
                            else if (op.Authority == Authority.AuthorityLossImminent)
                            {
                                entity.hasAuthority = false;
                            }
                        }
                    });

                    dispatcher.OnAddComponent<Life>(op =>
                    {
                        ViewEntity entity;
                        if (EntityView.TryGetValue(op.EntityId, out entity))
                        {
                            entity.entity.Add<Life>(op.Data);
                        }
                        else
                        {
                            ViewEntity newEntity = new ViewEntity();
                            EntityView.Add(op.EntityId, newEntity);
                            newEntity.entity.Add<Life>(op.Data);
                        }
                    });

                    dispatcher.OnComponentUpdate<Life>(op=>
                    {
                        ViewEntity entity;
                        if (EntityView.TryGetValue(op.EntityId, out entity))
                        {
                            var update = op.Update.Get();
                            entity.entity.Update<Life>(update);
                        }
                    });
                    dispatcher.OnAddComponent<Neighbors>(op =>
                    {
                        ViewEntity entity;
                        if (EntityView.TryGetValue(op.EntityId, out entity))
                        {
                            entity.entity.Add<Neighbors>(op.Data);
                        }
                        else
                        {
                            ViewEntity newEntity = new ViewEntity();
                            EntityView.Add(op.EntityId, newEntity);
                            newEntity.entity.Add<Neighbors>(op.Data);
                        }
                    });

                    dispatcher.OnComponentUpdate<Neighbors>(op =>
                    {
                        ViewEntity entity;
                        if (EntityView.TryGetValue(op.EntityId, out entity))
                        {
                            var update = op.Update.Get();
                            entity.entity.Update<Neighbors>(update);
                        }
                    });
                    dispatcher.OnAddEntity(op =>
                    {
                        //AddEntity will always be followed by OnAddComponent
                        ViewEntity newEntity = new ViewEntity();
                        newEntity.hasAuthority = true;
                        newEntity.entity = new Entity();
                        ViewEntity oldEntity;
                        if(!EntityView.TryGetValue(op.EntityId, out oldEntity))
                        {
                            EntityView.Add(op.EntityId, newEntity);
                        }
                    });
                    dispatcher.OnRemoveEntity(op =>
                    {
                        EntityView.Remove(op.EntityId);
                    });

                    WorkerConnection = connection;
                    WorkerConnection.SendLogMessage(LogLevel.Info, "LifeWorker", "Worker Connected");
                    RunEventLoop(connection, dispatcher);
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
        private static void RunEventLoop(Connection connection, Dispatcher dispatcher)
        {
            //var maxWait = System.TimeSpan.FromMilliseconds(1000f / FramesPerSecond);
            var maxWait = System.TimeSpan.FromMilliseconds(1000f * SecondsPerFrame);
            var stopwatch = new System.Diagnostics.Stopwatch();
            while (IsConnected)
            {
                stopwatch.Reset();
                stopwatch.Start();
                var opList = connection.GetOpList(0 /* non-blocking */);
                // Invoke callbacks.
                dispatcher.Process(opList);
                // Create the list of latest
                bool performUpdate = TryCreateEntitiesUpdatesAndSequenceIdList(EntityView, connection);
                if(performUpdate)
                {
                    SendAndClearEntitiesUpdates(connection);
                    ClearSequenceIdList();
                }
                else
                {
                    ClearEntitiesUpdates();
                    ClearSequenceIdList();
                }
                stopwatch.Stop();
                var waitFor = maxWait.Subtract(stopwatch.Elapsed);
                System.Threading.Thread.Sleep(waitFor.Milliseconds > 0 ? waitFor : System.TimeSpan.Zero);
            }
        }

        /// <summary>
        /// Runs through the View and if there is an entity then it is sent through the worker connection
        /// </summary>
        /// <param name="connection"></param>
        private static void SendAndClearEntitiesUpdates(Connection connection)
        {
            var seqIdList = GetSequenceIdsToUpdate();

            foreach (KeyValuePair<EntityId, ViewEntity> pair in EntityView)
            {
                if(pair.Value.hasUpdate)
                {
                    //Only update for a given set of current sequence ids
                    var currSeqId = GetLatestSequenceId(pair.Value.entity);
                    if(seqIdList.Contains(currSeqId))
                    {
                        var id = pair.Key;
                        var mu = pair.Value.metadataUpdate;
                        var lu = pair.Value.lifeUpdate;

                        //Send the updates
                        connection.SendComponentUpdate<Metadata>(id, mu);
                        connection.SendComponentUpdate<Life>(id, lu);
                    }
                    //Reset the bit to remove the update
                    ClearEntityUpdates(pair.Value);
                }
            }

            //TODO: Add list of sequence ids to this printout, as it currently just says "Sequence IDs" as the ToString output
            //WorkerConnection.SendLogMessage(LogLevel.Info, "LifeWorker", "Component Updates Sent for Sequence IDs " + seqIdList.ToString());
        }

        /// <summary>
        /// Clear all updates from the View
        /// </summary>
        private static void ClearEntitiesUpdates()
        {
            foreach (KeyValuePair<EntityId, ViewEntity> pair in EntityView)
            {
                if (pair.Value.hasUpdate)
                {
                    ClearEntityUpdates(pair.Value);
                }
            }
        }

        /// <summary>
        /// Gets the set of Sequence IDs to update
        /// If there are multiple current sequence ids, just perform the updates to get them all to the same id
        /// If there is one current sequence id, pass it through
        /// </summary>
        /// <returns></returns>
        private static System.Collections.Generic.List<ulong> GetSequenceIdsToUpdate()
        {
            System.Collections.Generic.List<ulong> retList = new System.Collections.Generic.List<ulong>();

            // Order the list
            sequenceIds.Sort();

            int count = sequenceIds.Count;
            if (count == 1) // If it is length of 1, return the one
            {
                retList.Add(sequenceIds[0]);
            }
            else if(count > 1) // If it is greater than one, return the lowest n - 1
            {
                for(int i = 0; i < (count - 1); i++)
                {
                    retList.Add(sequenceIds[i]);
                }
            }

            return retList;
        }

        /// <summary>
        /// Clear all updates from a single view entity
        /// </summary>
        /// <param name="viewEntity"></param>
        private static void ClearEntityUpdates(ViewEntity viewEntity)
        {
            viewEntity.hasUpdate = false;
            viewEntity.lifeUpdate = null;
            viewEntity.metadataUpdate = null;
        }

        /// <summary>
        /// If it isn't already present, add a sequence id to the list
        /// </summary>
        private static void AddSequenceIdToList(ulong id)
        {
            if (!sequenceIds.Contains(id))
            {
                sequenceIds.Add(id);
            }
        }

        /// <summary>
        /// Clear the list of all current sequence ids
        /// </summary>
        private static void ClearSequenceIdList()
        {
            sequenceIds.Clear();
        }

        /// <summary>
        /// Runs through the current view and creates updates for the entities it has write authority over
        /// Also fills in the list of all current sequence ids seen
        /// </summary>
        /// <param name="view"></param>
        /// <param name="connection">Pass the connection in to log messages, not to send updates</param>
        private static bool TryCreateEntitiesUpdatesAndSequenceIdList(Dictionary<EntityId, ViewEntity> view, Connection connection)
        {
            //WorkerConnection.SendLogMessage(LogLevel.Info, "LifeWorker", "Beginning Update Entities");

            int liveNeighborCount = 0;
            bool canGetAllNeighbors = false;

            //Run through every id/entity pair in the view
            foreach (var ve in view)
            {
                if(ve.Value.hasAuthority) //Only do this update if this worker has write access to the entity
                {
                    EntityId id = ve.Key;
                    Entity e = ve.Value.entity;

                    if (e == null)
                    {
                        throw new NullReferenceException("View Entities Enum has returned a null entity for EntityID " + ve.Key.ToString());
                    }

                    //Only do this if the entity is a Cell type (which should have a Life component)
                    var componentIdSet = e.GetComponentIds();
                    if (componentIdSet.Contains(Life.ComponentId))
                    {
                        // Add the current sequence id to the list of sequence ids for later selective updating
                        AddSequenceIdToList(GetLatestSequenceId(e));

                        //Get the number of live neighbors
                        canGetAllNeighbors = TryGetLiveNeighbors(id, e, view, out liveNeighborCount);
                        
                        //Update the entity based on the live neighbor count
                        if (canGetAllNeighbors)
                        {
                            CreateEntityUpdates(id, e, liveNeighborCount, connection);
                        }
                        else
                        {
                            return false; //If all neighbors can't be retrieved then the entity can't be updated. If one entity can't be updated then none can.
                        }
                    }
                    else
                    {
                        WorkerConnection.SendLogMessage(LogLevel.Info, "LifeWorker", "Life component not present on entity " + id.ToString() + ". update not done.");
                    }
                } 
            }

            return true; //All neighbors were successfully collected and updates successfully created
        }

        /// <summary>
        /// Get a count of live neighbors by running through the list of neighbors from the entity
        /// </summary>
        /// <param name="e"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        private static bool TryGetLiveNeighbors(EntityId id, Entity e, Dictionary<EntityId, ViewEntity> view, out int LiveNeighborCount)
        {
            ViewEntity neighbor;
            bool canGetLiveNeighbors = true;
            bool hasNeighbor = false;
            int liveNeighbors = 0;

            //Get the current time sequence id
            ulong seqId = GetLatestSequenceId(e);

            //Get the list of neighbor entity ids
            var option = e.Get<Neighbors>();

            if(!option.HasValue)
            {
                LiveNeighborCount = 0;
                WorkerConnection.SendLogMessage(LogLevel.Info, "LifeWorker", "Neighbors component not present on entity " + id.ToString() + ". update not done.");
                return canGetLiveNeighbors = false;
            }

            var neighbors = option.Value;
            var neighborsData = neighbors.Get();
            NeighborsData nd = neighborsData.Value;

            //Go through the entity IDs of the neighbors
            foreach(Improbable.EntityId neighborId in nd.neighborList)
            {
                //Get the life value for an entityID
                hasNeighbor = view.TryGetValue(neighborId, out neighbor);
                if(!hasNeighbor)
                {
                    //How to get the neighbor if it isn't in the view?
                    //This should only be for outer edge items, which this worker should only have read and not write permissions on
                    //Although there may be an issue of "View Completeness" in the beginning... so just say no and don't update the entity yet.
                    canGetLiveNeighbors = false;
                }
                else if(IsCellAlive(seqId, neighborId, neighbor.entity))
                {
                    //What if that cell isn't at that time sequence ID yet?
                    // TODO: Make TryGet and fail if the neighbor doesn't have the sequence id
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
            //Inout entity is null... why?
            Option<IComponentData<Life>> option;
            IComponentData<Life> life;
            Life.Data lifeData;
            LifeData ld;

            option = e.Get<Life>();
            life = option.Value;
            lifeData = life.Get();
            ld = lifeData.Value;
            return ld.curSequenceId;
        }

        /// <summary>
        /// Determine if a cell is alive for the provided time sequence ID
        /// </summary>
        /// <param name="sequenceId"></param>
        /// <param name="cellEntity"></param>
        /// <returns></returns>
        private static bool IsCellAlive(ulong sequenceId, EntityId cellId, Entity cellEntity)
        {
            bool isAlive = false;

            var option = cellEntity.Get<Life>();

            if(!(option.HasValue)) //Why is this happening? All Entities should have the Life component. Maybe there are entities that I'm picking up that aren't Cells?
            {
                WorkerConnection.SendLogMessage(LogLevel.Info, "LifeWorker", "IsCellAlive: option does not have Life component value for entity " + cellId.ToString());
                return isAlive = false;
            }

            var life = option.Value;

            var lifeData = life.Get();
            LifeData ld = lifeData.Value;

            if (ld.curSequenceId == sequenceId)
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
        private static void CreateEntityUpdates(EntityId id, Entity e, int LiveNeighborCount, Connection connection)
        {
            //Get Current life state of the entity
            bool isAlive = IsCellAlive(GetLatestSequenceId(e), id, e);
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
            CreateComponentUpdates(connection, id, e, isAliveNextState);
        }

        /// <summary>
        /// Create Updates for the Life and metadata components to the provided next state
        /// </summary>
        /// <param name="e"></param>
        /// <param name="isAliveNextState"></param>
        private static void CreateComponentUpdates(Connection connection, EntityId id, Entity e, bool isAliveNextState)
        {
            ulong curSequenceId = GetLatestSequenceId(e);
            bool curIsAlive = IsCellAlive(curSequenceId, id, e);

            //Update metadata for visualization in inspector
            Metadata.Update mu = new Metadata.Update();
            if(isAliveNextState)
            {
                mu.SetEntityType(EntityTypeCellAlive);
            }
            else
            {
                mu.SetEntityType(EntityTypeCellDead);
            }
            
            //Create new component update object
            Life.Update lu = new Life.Update();

            //set prev state and count id to the current state and time id
            lu.SetPrevSequenceId(curSequenceId);
            lu.SetPrevIsAlive(curIsAlive);

            //Set new current state
            lu.SetCurIsAlive(isAliveNextState);
            lu.SetCurSequenceId(curSequenceId + 1);

            //Add to the view
            ViewEntity viewEntity;
            if(EntityView.TryGetValue(id, out viewEntity))
            {
                viewEntity.hasUpdate = true;
                viewEntity.metadataUpdate = mu;
                viewEntity.lifeUpdate = lu;
            }
        }
    }
}
