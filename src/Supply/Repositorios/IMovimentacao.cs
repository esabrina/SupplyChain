using Microsoft.EntityFrameworkCore.Query.Internal;
using Supply.Models;
using System.Collections.Generic;

namespace Supply.Repositories
{
    public interface IMovimentacaoRepository
    {
        void Add(Movimentacao movimentacao);
        IEnumerable<Movimentacao> GetAll();
        Movimentacao Find(int id);
        void Remove(int id);
        void Update(Movimentacao movimentacao);
        bool MovimentacaoExists(int id);
        /// <summary>
        /// Movimenta��o consolidada por m�s da mercadoria
        /// </summary>
        /// <param name="idMercadoria">Id da Mercadoria</param>
        /// <param name="ano">Ano para a consolida��o mensal</param>
        /// <param name="tipoEntrada">Entrada (true) ou Sa�da (false)</param>
        /// <returns>Lista de objetos contendo IdMercadoria, NomeMercadoria, Mes e Qtd</returns>
        IEnumerable<object> GetByMonth(int idMercadoria, int ano, bool tipoEntrada);
    }
}
