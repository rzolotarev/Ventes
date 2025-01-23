// See https://aka.ms/new-console-template for more information
using Dapper.FluentMap;
using DataExporter;
using DataImporter;
using Persistence;
using System;
using System.IO;
using System.Linq;

FluentMapper.Initialize(config => config.AddMap(new VenteMap()));
var connectionString = "Server=localhost\\SQLEXPRESS;Database=master;Trusted_Connection=True;";
var repo = new VenteRepository(connectionString);
var importer = new Importer(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VENTES.xlsx"), repo);
await importer.ToDb();

IExporter exporter = new ParettoExporter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VENTES.xlsx"), new ParettoHandler(), repo);
await exporter.Export();

exporter = new PercentExporter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VENTES.xlsx"), new PercentHandler(80m), repo);
await exporter.Export();


var collection = await repo.GetAll();
Console.WriteLine(collection.First().ClientId);

