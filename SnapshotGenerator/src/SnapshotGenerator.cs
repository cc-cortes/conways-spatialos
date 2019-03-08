// Copyright (c) Improbable Worlds Ltd, All Rights Reserved

using System;
using System.Reflection;
using Improbable.Worker;
using Improbable;

namespace Demo
{
    class SnapshotGenerator
    {
        const string snapshotName = "default.snapshot";
        const string snapshotPath = snapshotName;
        const string workerType = "life"; //Layer name not workerType?

        static int Main(string[] arguments)
        {
            Console.WriteLine(String.Format("Generating snapshot file {0}", snapshotPath));
            Assembly.Load("GeneratedCode");

            // https://docs.improbable.io/reference/13.3/csharpsdk/using/snapshots

            // Construct a SnapshotOutputStream to write a snapshot to a file at the string path.
            SnapshotOutputStream sos = new SnapshotOutputStream(snapshotPath);

            var id = 0;
            var maxRowCount = 50;  //dimensionsInWorldUnits z is 100
            var maxColumnCount = 50; //dimensionsInWorldUnits x is 100
            // TODO: Update to account for center being at (0.0)

            //Create a grid of entities
            for (double y = 0; y < maxRowCount; y++) //Starts at 0
            {
                for (double x = 0; x < maxColumnCount; x++) //starts at 0
                {
                    id++; //starts at 1
                    var entityId = new EntityId(id);
                    Improbable.Collections.List<EntityId> nList = getNeighbors(x, maxColumnCount, y, maxRowCount, id);
                    bool currIsAlive = RandomBool();
                    bool prevIsAlive = RandomBool();
                    var entity = createEntity(workerType, x, y, currIsAlive, 0, prevIsAlive, 0, nList);
                    var error = sos.WriteEntity(entityId, entity);
                    if (error.HasValue)
                    {
                        throw new System.SystemException("error saving: " + error.Value);
                    }
                }
            }

            // Writes the end of snapshot header and releases the resources of the SnapshotOutputStream.
            sos.Dispose();

            return 0;
        }

        private static bool RandomBool()
        {
            bool output = true;
            var rand = new Random();
            if(rand.Next(2) == 0)
            {
                output = false;
            }
            
            return output;
        }

        private static Improbable.Collections.List<EntityId> getNeighbors(double x, int maxColumnCount, double y, int maxRowCount, int id)
        {
            Improbable.Collections.List<EntityId> neighbors = new Improbable.Collections.List<EntityId>();

            int maxX = maxColumnCount - 1;
            int maxY = maxRowCount - 1;
            long nId;

            //Top-Left
            if( (x > 0) & (y < maxY))
            {
                nId = (long)((y + 1) * maxColumnCount + ((x + 1) - 1));
                neighbors.Add(new EntityId(nId));
            }

            //Top
            if(y < maxY)
            {
                nId = (long)((y + 1) * maxColumnCount + (x + 1));
                neighbors.Add(new EntityId(nId));
            }

            //Top-Right
            if((y < maxY) & (x < maxX))
            {
                nId = (long)((y + 1) * maxColumnCount + (x + 2));
                neighbors.Add(new EntityId(nId));
            }

            //Right
            if(x < maxX)
            {
                neighbors.Add(new EntityId(id + 1));
            }

            //Bottom-Right
            if((x < maxX) & (y > 0))
            {
                nId = (long)((y - 1) * maxColumnCount + (x + 1 + 1));
                neighbors.Add(new EntityId(nId));
            }

            //Bottom
            if(y > 0)
            {
                nId = (long)((y - 1) * maxColumnCount + (x + 1));
                neighbors.Add(new EntityId(nId));
            }

            //Bottom-Left
            if((y > 0) & (x > 0))
            {
                nId = (long)((y - 1) * maxColumnCount + ((x + 1) - 1));
                neighbors.Add(new EntityId(nId));
            }

            //Left
            if(x > 0)
            {
                neighbors.Add(new EntityId(id - 1));
            }

            return neighbors;
        }

        private static Entity createEntity(string workerType, double X, double Y, bool currentAlive, UInt64 currentSequenceId, bool previousAlive, UInt64 previousSequenceId, Improbable.Collections.List<EntityId> neighborsList)
        {
            var entity = new Entity();
            //const string entityType = "Cell";
            string entityType = "Cell";
            if(currentAlive)
            {
                entityType = "Cell_Alive";
            }
            else
            {
                entityType = "Cell_Dead";
            }

            // Defines worker attribute requirements for workers that can read a component.
            // workers with an attribute of "client" OR workerType will have read access
            var readRequirementSet = new WorkerRequirementSet(
                new Improbable.Collections.List<WorkerAttributeSet>
                {
                    new WorkerAttributeSet(new Improbable.Collections.List<string> {workerType})
                    //new WorkerAttributeSet(new Improbable.Collections.List<string> {"client"}),
                });

            // Defines worker attribute requirements for workers that can write to a component.
            // workers with an attribute of workerType will have write access
            var workerWriteRequirementSet = new WorkerRequirementSet(
                new Improbable.Collections.List<WorkerAttributeSet>
                {
                    new WorkerAttributeSet(new Improbable.Collections.List<string> {workerType}),
                });

            var writeAcl = new Improbable.Collections.Map<uint, WorkerRequirementSet>
            {
                {EntityAcl.ComponentId, workerWriteRequirementSet},
                {Position.ComponentId, workerWriteRequirementSet},
                {Metadata.ComponentId, workerWriteRequirementSet},
                {Life.ComponentId, workerWriteRequirementSet}
            };

            entity.Add(new EntityAcl.Data(readRequirementSet, writeAcl));
            // Needed for the entity to be persisted in snapshots.
            entity.Add(new Persistence.Data());
            entity.Add(new Metadata.Data(entityType)); //Can use metadata to do color setting for visualization in inspector
            entity.Add(new Position.Data(new Coordinates(X, 0, Y)));
            entity.Add(new Life.Data(currentAlive, currentSequenceId, previousAlive, previousSequenceId));
            entity.Add(new Neighbors.Data(neighborsList));
            return entity;
        }
    }
}
