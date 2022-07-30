#!/bin/bash

_script_path=$(readlink -f "$0")
_script_path=$(dirname "$_script_path")
_project_path="$_script_path/PresenceMonitor"

# See README.md for hints on configuration
dotnet run --project "$_project_path" $@
