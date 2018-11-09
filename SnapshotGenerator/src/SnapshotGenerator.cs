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
        const string workerType = "LifeWorker";

        static int Main(string[] arguments)
        {
            Console.WriteLine(String.Format("Generating snapshot file {0}", snapshotPath));
            Assembly.Load("GeneratedCode");

            // https://docs.improbable.io/reference/13.3/csharpsdk/using/snapshots

            // Construct a SnapshotOutputStream to write a snapshot to a file at the string path.
            SnapshotOutputStream sos = new SnapshotOutputStream(snapshotPath);

            var id = 0;

            //Create a 10x10 grid of entities
            for (int i = 0; i < 10; i++)
            {
                for(int j = 0; j < 10; j++)
                {
                    id++;
                    var entityId = new EntityId(id + 1);
                    var entity = createEntity(workerType, i, j);
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

        private static Entity createEntity(string workerType, double X, double Y)
        {
            var entity = new Entity();
            const string entityType = "Cell";
            // Defines worker attribute requirements for workers that can read a component.
            // workers with an attribute of "client" OR workerType will have read access
            var readRequirementSet = new WorkerRequirementSet(
                new Improbable.Collections.List<WorkerAttributeSet>
                {
                    new WorkerAttributeSet(new Improbable.Collections.List<string> {workerType}),
                    new WorkerAttributeSet(new Improbable.Collections.List<string> {"client"}),
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
                {Position.ComponentId, workerWriteRequirementSet}
            };

            entity.Add(new EntityAcl.Data(readRequirementSet, writeAcl));
            // Needed for the entity to be persisted in snapshots.
            entity.Add(new Persistence.Data());
            entity.Add(new Metadata.Data(entityType));
            entity.Add(new Position.Data(new Coordinates(X, Y, 0)));
            entity.Add(new Demo.Life.Data(bool false)); //Need to add Life component
            return entity;
        }
    }
}
