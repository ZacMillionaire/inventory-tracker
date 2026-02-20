var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.InventorySystem>("inventorysystem");

builder.Build().Run();
