#!/bin/bash

_ok=0
_not_ok=1

function _help_necessary() {
  local args_empty=$([ $# -eq 0 ] && echo 'true' || echo 'false')
  if [ $args_empty = 'true' ]; then
    return $_not_ok
  fi

  [ "$1" = '--help' ] \
    || [ "$1" = '-h' ] \
    || [ "$1" = '-?' ] \
    || [ "$1" = '/?' ] \
    || [ "$1" = '/h' ]
}

function _print_help() {
  echo '
  	Usage:
  		./run.sh [--help] [...]
  		# Any parameter passed to run is passed down as-is to dotnet run
  		
  		DOTNET_ENVIRONMENT=Offline ./run.sh
  		# Runs the application in offline mode (Defaults to 'Development')
  	'
}

_script_path=$(readlink -f "$0")
_script_path=$(dirname "$_script_path")
_project_path="$_script_path/PresenceMonitor"

if _help_necessary $@; then
  _print_help
  exit 99
fi

export DOTNET_ENVIRONMENT=${DOTNET_ENVIRONMENT:=Development}

# See README.md for hints on configuration
#dotnet run --project "$_project_path" $@
