# Zeltik.Mongo "Extension"

This extension is packaged for my own personal use. It is not recommended for general use unless you want to have a headache and lost some time.

## Description

This extension provides additional functionality for interacting with MongoDB. It includes some utilities that I find useful for me in my own projects. However, please note that this extension may not have undergone extensive testing and may not be suitable for production environments.

## Features

- Extension method for `IServiceCollection` to simplify MongoDB integration
- Integration of MongoDB context and options using `MongoDbContext` (specifically, inheriting it)
- Simplified usage of `MongoDB.Driver` library (I guess)

## Installation

You can install the Zeltik.Mongo Extension as a NuGet package. Follow the steps below:

1. Open your project in Visual Studio.
2. Right-click on your project in the Solution Explorer and select "Manage NuGet Packages".
3. In the NuGet Package Manager, search for "Zeltik.Mongo" and select the package in the search results.
4. Click on the "Install" button to install the package into your project.

After installing the NuGet package, the necessary dependencies and files will be added to your project.

## Usage

After installing the Zeltik.Mongo Extension, you can start using its utilities in your code. Here's an example of how to integrate MongoDB using the provided code:

```csharp
//Import necessary namespaces
using Microsoft.Extensions.Options;
using Example.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Zeltik.Mongo;

namespace Example.Data
{
    //Inherit from MongoDbContext
    public class AppDbContext : MongoDbContext
    {
        //This constructor is for dependency injection for one MongoOptions
        public AppDbContext(IOptions<MongoOptions> options) : base(options) { }
        //If you want to have multiple contexts and options, you'll need to use the next constructor but before, delete the first constructor
        public AppDbContext(IOptionsMonitor<MongoOptions> manager) : base(manager.Get("instanceName")) { }

        //Use GetCollection as a get accessor for your Context properties
        public IMongoCollection<Product> Products => GetCollection<Product>("Products");

        //Use this method for Configure your entities
        protected override void OnConfiguring()
        {
            ConfigureProductMap();
        }

        private static void ConfigureProductMap()
        {
            BsonClassMap.RegisterClassMap<Product>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(false);
                cm.MapIdMember(x => x.Id).SetIdGenerator(StringObjectIdGenerator.Instance)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId)).SetIgnoreIfDefault(true);
                cm.SetIdMember(cm.GetMemberMap(x => x.Id));
            });
        }
    }
}

//Then, add your context to your IServiceCollection
    builder.Services.AddMongoContext<AppDbContext>(options => options.SetDatabaseName("your_databaseName").SetConnectionString("your_connection"));

//And finally (I guess), inject it to your service or repository (basically, wherever you want)
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
    
        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Product> GetAsync(string id)
        {
            return await (await _context.Products.FindAsync(new BsonDocument("_id", new ObjectId(id)))).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetAsync()
        {
            return await (await _context.Products.FindAsync(new BsonDocument())).ToListAsync();
        }
    }
```
Please note that the above example is just a basic usage demonstration. Refer to the documentation or the source code for more advanced usage scenarios (if casually there are).

## Disclaimer

**Warning:** This Zeltik.Mongo "Extension" is provided "as is" and is intended for personal use only (or at least it's recommended just for that). It may contain bugs, limitations, or other issues that could cause headaches or unexpected behavior. Use it at your own risk.

## Contributing

Contributions to this Zeltik.Mongo "Extension" are not currently accepted, as it is developed and maintained solely for personal use. However, if you find any bugs or have suggestions, feel free to open an issue in the repository.

## Licensing

This extension (Zeltik.Mongo) is released under the **[MIT License](https://github.com/Jorivan678/Zeltik.Mongo/blob/main/LICENSE)**