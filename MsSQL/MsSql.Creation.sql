USE master
IF EXISTS(select * from sys.databases where name='RaceDb')
	DROP DATABASE RaceDb

CREATE DATABASE RaceDb