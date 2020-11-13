using Supply.Models;
using System.Collections.Generic;

namespace Supply.Repositories
{
    public interface IMercadoriaRepository
    {
        void Add(Mercadoria mercadoria);
        IEnumerable<Mercadoria> GetAll();
        Mercadoria Find(int id);
        void Remove(int id);
        void Update(Mercadoria mercadoria);
        bool MercadoriaExists(int id);
    }
}
