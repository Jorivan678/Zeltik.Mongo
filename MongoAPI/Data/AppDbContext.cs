using Microsoft.Extensions.Options;
using MongoAPI.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Zeltik.Mongo;

namespace MongoAPI.Data
{
    public class AppDbContext : MongoDbContext
    {
        public AppDbContext(IOptions<MongoOptions> options) : base(options) { }

        //Aquí van las propiedades xd
        public IMongoCollection<Marca> Marcas => GetCollection<Marca>("Marcas");

        protected override void OnConfiguring()
        {
            ConfigureMarcaMap();
        }

        private static void ConfigureMarcaMap()
        {
            BsonClassMap.RegisterClassMap<Marca>(cm =>
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
