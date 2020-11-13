using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Supply.Models;
using System.Collections.Generic;
using System.Linq;

namespace Supply.Repositories
{
    public class MovimentacaoRepository : IMovimentacaoRepository
    {
        private readonly SupplyDBContext _context;
        public MovimentacaoRepository(SupplyDBContext ctx)
        {
            _context = ctx;
        }
        public void Add(Movimentacao movimentacao)
        {
            _context.Movimentacoes.Add(movimentacao);
            _context.SaveChanges();
        }

        public Movimentacao Find(int id)
        {
            var result = _context.Movimentacoes
                .Include(i => i.Mercadoria)
                .FirstOrDefault(i => i.Id == id);
            return result;
        }

        public IEnumerable<Movimentacao> GetAll()
        {
            var p = _context.Movimentacoes
                .Include(m => m.Mercadoria)
                .OrderByDescending(i => i.DataHora)
                .ToList();
            return (IEnumerable<Movimentacao>)p;
        }

        public void Remove(int id)
        {
            var entity = _context.Movimentacoes.First(i => i.Id == id);
            _context.Movimentacoes.Remove(entity);
            _context.SaveChanges();
        }

        public void Update(Movimentacao movimentacao)
        {
            _context.Movimentacoes.Update(movimentacao);
            _context.SaveChanges();
        }
        public bool MovimentacaoExists(int id)
        {
            return _context.Movimentacoes.Any(e => e.Id == id);
        }

        public IEnumerable<object> GetByMonth(int idMercadoria, int ano, bool tipoEntrada)
        {
            var _qry = (from m in _context.Movimentacoes
                       where m.MovimentacaoEntrada == tipoEntrada
                       && m.IdMercadoria == idMercadoria
                       group m by new
                       { m.Mercadoria.Id, m.Mercadoria.Nome, 
                           ano = m.DataHora.Year,
                           mes = m.DataHora.Month } into qry
                       select new
                       {
                           IdMercadoria = qry.Key.Id,
                           NomeMercadoria = qry.Key.Nome,
                           Mes = qry.Key.mes,
                           Qtd = qry.Sum(x => x.Quantidade)
                       }).OrderBy(at => at.Mes).ToList();
            return (IEnumerable<object>)_qry;
        }
    }
}
