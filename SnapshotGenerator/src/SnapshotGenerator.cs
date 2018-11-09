// Copyright (c) Improbable Worlds Ltd, All Rights Reserved

using System;
using System.Reflection;
using Improbable.Worker;

namespace Demo
{
    class SnapshotGenerator
    {
        const string snapshotName = "default.snapshot";
        const string snapshotPath = snapshotName;

        static int Main(string[] arguments)
        {
            // https://docs.improbable.io/reference/13.3/csharpsdk/using/snapshots

            // Construct a SnapshotOutputStream to write a snapshot to a file at the string path.
            SnapshotOutputStream sos = new SnapshotOutputStream(snapshotPath);


            //Create a 10x10 grid of entities

            // Writes the (EntityId entityId, Entity entity) pair to the snapshot.
            // Returns an Optional string error message if an error occurs during writing.
            sos.WriteEntity(EntityId entityId, Entity entity)

            // Writes the end of snapshot header and releases the resources of the SnapshotOutputStream.
            public void Dispose()


            return 0;
        }
    }
}
