using MongoAPI.Data;
using MongoAPI.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace MongoAPI.Repositories
{
    public class MarcaRepository : IMarcaRepository
    {
        private readonly AppDbContext _context;

        public MarcaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Marca> GetAsync(string id)
        {
            return await (await _context.Marcas.FindAsync(new BsonDocument("_id", new ObjectId(id)))).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Marca>> GetAsync()
        {
            return await (await _context.Marcas.FindAsync(new BsonDocument())).ToListAsync();
        }
    }
}
