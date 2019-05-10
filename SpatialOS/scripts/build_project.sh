#!/usr/bin/env bash

# This script builds the full project by running all other provided shell scripts in sequence

abort()
{
    echo >&2 '
***************
*** ABORTED ***
***************
'
    echo "An error occurred. Exiting..." >&2
    read -p "Press [Enter] key to exit..."
}

trap 'abort' 0

set -e -x
pushd "$( dirname "${BASH_SOURCE[0]}" )"
source ./utils.sh

./download_dependencies.sh
./generate_schema_descriptor.sh

# Build all workers in the project
for WORKER_DIR in "${WORKER_DIRS[@]}"; do
  pushd "${WORKER_DIR}"
  ./build.sh
  popd
done

echo "Build complete."

popd