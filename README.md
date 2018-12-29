
# Conway's Game of Life using SpatialOS

## What is it?
This is a simple SpatialOS project with one C# Server Worker and a Snapshot Generator.

> **Note**: SpatialOS expects the workers to be pre-built, and only contains information on how to run them. This means the `spatial worker build` command is unavailable - you're responsible for building your workers (see [Building the project](#building-the-project) below).

#### Prerequisites

See the [SpatialOS documentation](https://docs.improbable.io/reference/latest) to set up the `spatial` CLI.

Run `spatial update` to ensure you have the latest version of the `spatial` command line tool which includes the features required for this example project.

Mac users: ensure that `xbuild` is on your PATH. `xbuild` is provided by Mono.

Windows users: ensure that `MSBuild.exe` is on your PATH. `MSBuild.exe` is provided by the .NET Framework.

Bash is required for running the build script, although as an alternative you can complete these steps manually. There are several options to install bash on Windows, although we recommend [GitBash](https://gitforwindows.org/).

## How do I use it?
The custom layout is enabled by the `spatial` tool detecting a [new-format spatialos.json](docs/reference/project-configuration.md) file. 

Both `spatial local launch` and `spatial cloud launch` have a new parameter, `--world`, which points to a world configuration file. This flag is required to launch a deployment, although by default a file named [`world.json`](docs/reference/world-configuration.md) in the current working directory will be used if you don't pass the flag in.

You can access this information at any time by using the `--help` flag in the `spatial` command line tool.

## Building the project
Run `./build.sh` to build the workers and compile the schema descriptor. You can complete these steps manually if required.

Workers and tools are built in their own bin directories:
* LifeWorker: `LifeoWorker/bin`
* SnapshotGenerator: `SnapshotGenerator/bin`

## Cleaning the project
Run `./clean.sh` to delete all build files, including worker binaries and any intermediate files generated during the build process.

## Running the project

To launch a local instance of SpatialOS running the project, run `spatial local launch --launch_config ./deployment.json --optimize_for_runtime_v2` from the SpatialOS directory (or from any location by specifying the `--main_config=\<path to spatialos.json\>` flag). This starts SpatialOS locally and runs the server workers `LifeWorker` using the latest version of the runtime.

Connect your client by opening a second terminal to run the binary directly (from inside the `ReleaseWindows` or `ReleaseMacOS` directories):
* Windows: `./Client.exe localhost 7777 <client_id>`
* macOS: `mono --arch=64 Client.exe localhost 7777 <client_id>`

## Reference documentation

[Main project configuration](docs/reference/project-configuration.md)

[World configuration](docs/reference/world-configuration.md)

[Server worker configuration](docs/reference/server-worker-configuration.md)

[Client worker configuration](docs/reference/client-worker-configuration.md)
