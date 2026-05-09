using InventorySystem.Core;

var ports = new
{
	Backend = 9999,
	Frontend = 5173
};

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
	.WithDataVolume(isReadOnly: false)
	.ExcludeFromMcp();

var postgresdb = postgres.AddDatabase(EnvironmentKeys.PostgresDbEnvironmentKey)
	.ExcludeFromMcp();

var api = builder.AddProject<Projects.InventorySystem>("inventorysystem", launchProfileName: null)
	.WithEndpoint("http", endpoint =>
	{
		// here to remove the need for the first profile in launchSettings.json (that we set null above)
		endpoint.Port = ports.Backend;
		endpoint.IsProxied = false;
		endpoint.UriScheme = "http";
	})
	//.WaitFor(postgresdb)
	.WithReference(postgresdb)
	.WithEnvironment(context =>
	{
		// Originally would've come in with the launch profile but doesn't so we set it here
		context.EnvironmentVariables["ASPNETCORE_ENVIRONMENT"] = "Development";
		// As always, setting ports for ASP is fuckin weird where you actually set the URL.
		// Yeah it makes sense if you want to maybe have it listen on a real domain name but who actually does that?
		context.EnvironmentVariables["ASPNETCORE_URLS"] = $"http://*:{ports.Backend}";

		// Additional individual connection details as environment variables
		// TODO: here just incase I care, but 
		// context.EnvironmentVariables["POSTGRES_HOST"] = postgres.Resource.PrimaryEndpoint.Property(EndpointProperty.Host);
		// context.EnvironmentVariables["POSTGRES_PORT"] = postgres.Resource.PrimaryEndpoint.Property(EndpointProperty.Port);
		// context.EnvironmentVariables["POSTGRES_USER"] = postgres.Resource.UserNameParameter;
		// context.EnvironmentVariables["POSTGRES_PASSWORD"] = postgres.Resource.PasswordParameter;
		// context.EnvironmentVariables["POSTGRES_DATABASE"] = postgresdb.Resource.DatabaseName;
	})
	.ExcludeFromMcp();

builder.AddViteApp("frontend", "../InventorySystem.Web.Vue/inventory-system-ui", "dev")
	.WithEndpoint("http", (endpointAnnotation) =>
	{
		endpointAnnotation.Port = ports.Frontend;
		endpointAnnotation.IsProxied = false;
	})
	// This doesn't actually control the args passed to the vite app that sets the port because uhhhhh don't
	// ask questions thanks
	.WithEnvironment(ctx =>
	{
		ctx.EnvironmentVariables["VITE_API_URL"] = api.GetEndpoint("http");
	})
	.WithReference(api);

builder.Build().Run();