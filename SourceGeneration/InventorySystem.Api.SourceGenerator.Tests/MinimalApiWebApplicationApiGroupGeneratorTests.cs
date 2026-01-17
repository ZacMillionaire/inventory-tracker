using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace InventorySystem.Api.SourceGenerator.Tests;

public class MinimalApiWebApplicationApiGroupGeneratorTests
{
	public MinimalApiWebApplicationApiGroupGeneratorTests()
	{
		VerifySourceGenerators.Initialize();
	}

	[Fact]
	public Task GeneratesApiExtensionsCorrectly()
	{
		// The source code to test
		var source = """
		             using InventorySystem.Core.Api;
		             namespace Tests
		             {

		                 [ApiGroup("items")]
		             	public class ItemApiRoutes
		             	{
		             		[ApiGet("/")]
		             		public void GetItems(ItemRepository repo) => repo.Get();
		             		
		             		[ApiPost("Create")]
		             public void CreateItem(ItemRepository repo, [FromBody] CreateItemRequestDto dto) => repo.Create(dto);
		             	}
		             }
		             """;

		// Pass the source code to our helper and snapshot test the output
		return TestHelper.Verify(source,"MinimalApiWebApplicationApiGroup");
	}
}

// public static class ModuleInitializer
// {
// 	[ModuleInitializer]
// 	public static void Init()
// 	{
// 		VerifySourceGenerators.Initialize();
// 		Verifier.UseProjectRelativeDirectory("TestVerifications");
// 	}
// }

public static class TestHelper
{
	public static Task Verify(string source, string verificationFolderName)
	{
		Verifier.UseProjectRelativeDirectory($"TestVerifications/{verificationFolderName}");
		// Parse the provided string into a C# syntax tree
		SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(source);

		IEnumerable<PortableExecutableReference> references = new[]
		{
			MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
		};

		// Create a Roslyn compilation for the syntax tree.
		CSharpCompilation compilation = CSharpCompilation.Create(
			assemblyName: "Tests",
			syntaxTrees: new[] { syntaxTree },
			references: references);


		// Create an instance of our EnumGenerator incremental source generator
		var generator = new MinimalApiWebApplicationApiGroupGenerator();

		// The GeneratorDriver is used to run our generator against a compilation
		GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

		// Run the source generator!
		driver = driver.RunGenerators(compilation);

		// Use verify to snapshot test the source generator output!
		return Verifier.Verify(driver);
	}
}