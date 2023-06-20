using MongoAPI.Models;

namespace MongoAPI.Repositories
{
    public interface IMarcaRepository
    {
        Task<Marca> GetAsync(string id);
        Task<IEnumerable<Marca>> GetAsync();
    }
}
