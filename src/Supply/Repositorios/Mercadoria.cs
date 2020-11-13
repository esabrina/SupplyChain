using Supply.Models;
using System.Collections.Generic;
using System.Linq;

namespace Supply.Repositories
{
    public class MercadoriaRepository : IMercadoriaRepository
    {
        private readonly SupplyDBContext _context;
        public MercadoriaRepository(SupplyDBContext ctx)
        {
            _context = ctx;
        }
        public void Add(Mercadoria mercadoria)
        {
            _context.Mercadorias.Add(mercadoria);
            _context.SaveChanges();
        }

        public Mercadoria Find(int id)
        {
            var result = _context.Mercadorias
                .FirstOrDefault(i => i.Id == id);
            return result;
        }

        public IEnumerable<Mercadoria> GetAll()
        {
            var p = _context.Mercadorias.OrderBy(i => i.Nome).ToList();
            return (IEnumerable<Mercadoria>)p;
        }

        public void Remove(int id)
        {
            var entity = _context.Mercadorias.First(i => i.Id == id);
            _context.Mercadorias.Remove(entity);
            _context.SaveChanges();
        }

        public void Update(Mercadoria mercadoria)
        {
            _context.Mercadorias.Update(mercadoria);
            _context.SaveChanges();
        }
        public bool MercadoriaExists(int id)
        {
            return _context.Mercadorias.Any(e => e.Id == id);
        }
    }
}
