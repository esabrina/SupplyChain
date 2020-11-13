using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Supply.Models;
using Supply.Repositories;

namespace Supply.Controllers
{
    public class MovimentacaoController : Controller
    {
        private readonly IMovimentacaoRepository _repo;
        private readonly IMercadoriaRepository _repoMercadoria;

        public MovimentacaoController(IMovimentacaoRepository repo,  IMercadoriaRepository repoMercadoria)
        {
            _repo = repo;
            _repoMercadoria = repoMercadoria;
        }

        // GET: Movimentacao
        public IActionResult Index()
        {
            return View(_repo.GetAll());
        }

        // GET: Movimentacao/Details/5
        public IActionResult Details(int id)
        {
            var movimentacao = _repo.Find(id);
            if (movimentacao == null)
            {
                return NotFound();
            }

            return View(movimentacao);
        }

        // GET: Movimentacao/Create
        public IActionResult Create()
        {
            ViewData["IdMercadoria"] = new SelectList(_repoMercadoria.GetAll(), "Id", "Nome");
            return View();
        }

        // POST: Movimentacao/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("DataHora,MovimentacaoEntrada,Quantidade,Local,IdMercadoria")] Movimentacao movimentacao)
        {
            if (ModelState.IsValid)
            {
                movimentacao.DataHora = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                _repo.Add(movimentacao);
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdMercadoria"] = new SelectList(_repoMercadoria.GetAll(), "Id", "Nome");
            return View(movimentacao);
        }

        // GET: Movimentacao/Edit/5
        public IActionResult Edit(int id)
        {
            var movimentacao = _repo.Find(id);
            if (movimentacao == null)
            {
                return NotFound();
            }
            ViewData["IdMercadoria"] = new SelectList(_repoMercadoria.GetAll(), "Id", "Nome");
            return View(movimentacao);
        }

        // POST: Movimentacao/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,DataHora,MovimentacaoEntrada,Quantidade,Local,IdMercadoria")] Movimentacao movimentacao)
        {
            if (id != movimentacao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _repo.Update(movimentacao);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_repo.MovimentacaoExists(movimentacao.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdMercadoria"] = new SelectList(_repoMercadoria.GetAll(), "Id", "Nome");
            return View(movimentacao);
        }

        // GET: Movimentacao/Delete/5
        public IActionResult Delete(int id)
        {
            var movimentacao = _repo.Find(id);
            if (movimentacao == null)
            {
                return NotFound();
            }

            return View(movimentacao);
        }

        // POST: Movimentacao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var movimentacao = _repo.Find(id);
            if (movimentacao == null)
            {
                return NotFound();
            }
            _repo.Remove(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Grafico
        public IActionResult Grafico()
        {
            string ano = DateTime.Now.Year.ToString();
            var mercadorias = _repoMercadoria.GetAll();
            TempData["grafico_ano"] = ano;
            TempData["grafico_series_entrada"] = GetSeries(ano, true, mercadorias);
            TempData["grafico_series_saida"] = GetSeries(ano, false, mercadorias);
            return View();
        }

        // GET: Estoque
        public IActionResult Estoque()
        {
            var data = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            var mercadorias = _repoMercadoria.GetAll();
            var movimentacoes = _repo.GetAll();
            List<Movimentacao> ListMovimentacao = new List<Movimentacao>();

            foreach (Mercadoria merc in mercadorias)
            {
                var mov = new Movimentacao { DataHora = data, Local = "", MovimentacaoEntrada = true, 
                    IdMercadoria = merc.Id, Mercadoria = merc };
                mov.Quantidade = CalculateQtdMercadoria(merc.Id, movimentacoes);
                ListMovimentacao.Add(mov);
            }
            return View(ListMovimentacao);
        }

        private string GetSeries(string ano, bool tipoEntrada, IEnumerable<Mercadoria> mercadorias)
        {
            List<int> meses = new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12};
            string _series = "[";

            foreach (Mercadoria merc in mercadorias)
            {
                _series += " {name: '" + merc.Nome + "', data: [";
                var valores = _repo.GetByMonth(merc.Id, Convert.ToInt32(ano), tipoEntrada);

                if (valores.Count() == 0)
                {
                    _series += "0,0,0,0,0,0,0,0,0,0,0,0,";
                }
                else {
                    foreach (int mes in meses)
                    {
                        bool achou = false;
                        foreach (dynamic item in valores)
                        {
                            if (mes == item.Mes)
                            {
                                _series += item.Qtd.ToString();
                                achou = true;
                                break;
                            }
                        }
                        if (achou == false)
                        {
                            _series += "0";
                        }
                        _series += ",";
                    }
                }
                _series = _series.TrimEnd(',') + "]},";
            }
            _series = _series.TrimEnd(',') + "]";
            return _series;
        }

        private int CalculateQtdMercadoria(int idMercadoria, IEnumerable<Movimentacao> movimentacoes)
        {
            int soma_entrada = CalculateByTypeQtdMercadoria(idMercadoria, movimentacoes, true);
            int soma_saida = CalculateByTypeQtdMercadoria(idMercadoria, movimentacoes, false);
            return soma_entrada - soma_saida;
        }

        private int CalculateByTypeQtdMercadoria(int idMercadoria, IEnumerable<Movimentacao> movimentacoes, bool tipoEntrada)
        {
            int soma = 0;
            var _qry = (from m in movimentacoes
                                where m.MovimentacaoEntrada == tipoEntrada
                                && m.IdMercadoria == idMercadoria
                                group m by new { m.Mercadoria.Id } into qry
                                select new
                                {
                                    IdMercadoria = qry.Key.Id,
                                    Qtd = qry.Sum(x => x.Quantidade)
                                }).ToList();

            foreach (dynamic item in _qry)
            {
                soma += item.Qtd;
            }
            return soma;
        }
    }
}
