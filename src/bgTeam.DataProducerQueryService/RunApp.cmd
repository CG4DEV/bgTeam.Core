@ECHO OFF

ECHO ##########################################################
ECHO #######                 IMPORTANT!                 #######
ECHO To start the application using this script
ECHO use the environment variable specified in appsettings.json
ECHO Or run the script RunApp.Debug.cmd
ECHO ##########################################################

START "DataProducerQueryService" dotnet bgTeam.DataProducerQueryService.dll