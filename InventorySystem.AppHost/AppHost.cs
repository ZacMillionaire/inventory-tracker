using InventorySystem.Core;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
	.WithDataVolume(isReadOnly: false)
	.ExcludeFromMcp();

var postgresdb = postgres.AddDatabase(EnvironmentKeys.PostgresDbEnvironmentKey)
	.ExcludeFromMcp();

builder.AddProject<Projects.InventorySystem>("inventorysystem")
	.WaitFor(postgresdb)
	.WithReference(postgresdb)
	.WithEnvironment(context =>
	{
		// Additional individual connection details as environment variables
		// TODO: here just incase I care, but 
		// context.EnvironmentVariables["POSTGRES_HOST"] = postgres.Resource.PrimaryEndpoint.Property(EndpointProperty.Host);
		// context.EnvironmentVariables["POSTGRES_PORT"] = postgres.Resource.PrimaryEndpoint.Property(EndpointProperty.Port);
		// context.EnvironmentVariables["POSTGRES_USER"] = postgres.Resource.UserNameParameter;
		// context.EnvironmentVariables["POSTGRES_PASSWORD"] = postgres.Resource.PasswordParameter;
		// context.EnvironmentVariables["POSTGRES_DATABASE"] = postgresdb.Resource.DatabaseName;
	})
	.ExcludeFromMcp();

