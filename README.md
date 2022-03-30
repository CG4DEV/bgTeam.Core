## bgTeam.Core  

[![Build status](https://ci.appveyor.com/api/projects/status/x34oo0dbvftcdcvv?svg=true)](https://ci.appveyor.com/project/bgTeamDev/bgteam-core) ![Coverage Status](https://murstv.github.io/coveradge-badge.svg "Coverage Status") [![License MIT](https://img.shields.io/badge/license-MIT-green.svg)](https://opensource.org/licenses/MIT) 

[![Join the chat at https://discord.gg/qYmj4Z7jDA](https://murstv.github.io/discord-badge.svg)](https://discord.gg/qYmj4Z7jDA)

### Description

bgTeam infrastructure for .NET projects

### Usage

These libraries are designed to build projects on microservice architecture. Assemblies allow you to quickly release the basic infrastructure layers, which allows you to concentrate on the development of functionality

### Generating new project by template

To generate new project you can run bgTeam.ProjectTemplate from visual studio or using console:
dotnet run --framework=[netcoreapp3.1 | net5.0] -company trcont -project ShopMonitoring -is-web true -is-app true -bg-team-version 5.0.28 (required only --framework parameter)
Solution will be generated in folder bin/result.

### Generating coverage report

Move to folder with project and run coverage.bat, it will generate report to file coverage/index.htm
After report will be generated it will try to send coverage result to gitlab (settings in wiki-generator/settings.json)

### Contents

Package  | Description | NuGet 
--------| -------- | -------- 
bgTeam.Core  | Base interface | [![NuGet version](https://badge.fury.io/nu/bgTeam.Core.svg)](https://badge.fury.io/nu/bgTeam.Core)
bgTeam.DataAccess  | Base interface for DataBase | [![NuGet version](https://badge.fury.io/nu/bgTeam.DataAccess.svg)](https://badge.fury.io/nu/bgTeam.DataAccess)
bgTeam.Queues  | Base interface for Queues | [![NuGet version](https://badge.fury.io/nu/bgTeam.Queues.svg)](https://badge.fury.io/nu/bgTeam.Queues)
bgTeam.Web  | Classes for REST web client  | [![NuGet version](https://badge.fury.io/nu/bgTeam.Web.svg)](https://badge.fury.io/nu/bgTeam.Web)
bgTeam.Extensions  | Extensions for lite coding  | [![NuGet version](https://badge.fury.io/nu/bgTeam.Extensions.svg)](https://badge.fury.io/nu/bgTeam.Extensions)

### Implementation

Package  |  NuGet 
--------| -------- 
bgTeam.Impl.Dapper | [![NuGet version](https://badge.fury.io/nu/bgTeam.Impl.Dapper.svg)](https://badge.fury.io/nu/bgTeam.Impl.Dapper)
bgTeam.Impl.Memory | [![NuGet version](https://badge.fury.io/nu/bgTeam.Impl.Memory.svg)](https://badge.fury.io/nu/bgTeam.Impl.Memory)
bgTeam.Impl.MongoDB | [![NuGet version](https://badge.fury.io/nu/bgTeam.Impl.MongoDB.svg)](https://badge.fury.io/nu/bgTeam.Impl.MongoDB)
bgTeam.Impl.MsSql | [![NuGet version](https://badge.fury.io/nu/bgTeam.Impl.MsSql.svg)](https://badge.fury.io/nu/bgTeam.Impl.MsSql)
bgTeam.Impl.Oracle | [![NuGet version](https://badge.fury.io/nu/bgTeam.Impl.Oracle.svg)](https://badge.fury.io/nu/bgTeam.Impl.Oracle)
bgTeam.Impl.PostgreSQL | [![NuGet version](https://badge.fury.io/nu/bgTeam.Impl.PostgreSQL.svg)](https://badge.fury.io/nu/bgTeam.Impl.PostgreSQL)
bgTeam.Impl.Sqlite | [![NuGet version](https://badge.fury.io/nu/bgTeam.Impl.Sqlite.svg)](https://badge.fury.io/nu/bgTeam.Impl.Sqlite)
bgTeam.Impl.Serilog | [![NuGet version](https://badge.fury.io/nu/bgTeam.Impl.Serilog.svg)](https://badge.fury.io/nu/bgTeam.Impl.Serilog)
bgTeam.Impl.ElasticSearch | [![NuGet version](https://badge.fury.io/nu/bgTeam.Impl.ElasticSearch.svg)](https://badge.fury.io/nu/bgTeam.Impl.ElasticSearch)
bgTeam.Impl.Quartz | [![NuGet version](https://badge.fury.io/nu/bgTeam.Impl.Quartz.svg)](https://badge.fury.io/nu/bgTeam.Impl.Quartz)
bgTeam.Impl.Rabbit | [![NuGet version](https://badge.fury.io/nu/bgTeam.Impl.Rabbit.svg)](https://badge.fury.io/nu/bgTeam.Impl.Rabbit)
bgTeam.Impl.Kafka | [![NuGet version](https://badge.fury.io/nu/bgTeam.Impl.Kafka.svg)](https://badge.fury.io/nu/bgTeam.Impl.Kafka)