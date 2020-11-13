using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Supply.Repositories;
using Supply.Models;

namespace Supply.Controllers
{
    public class MercadoriaController : Controller
    {
        private readonly IMercadoriaRepository _repo;

        public MercadoriaController(IMercadoriaRepository repo)
        {
            _repo = repo;
        }

        // GET: Mercadoria
        public IActionResult Index()
        {
            return View(_repo.GetAll());
        }

        // GET: Mercadoria/Details/5
        public ActionResult Details(int id)
        {
            var mercadoria = _repo.Find(id);
            if (mercadoria == null)
            {
                return NotFound();
            }

            return View(mercadoria);
        }

        // GET: Mercadoria/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Mercadoria/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Nome,Descricao,Tipo,Fabricante")] Mercadoria mercadoria)
        {
            if (ModelState.IsValid)
            {
                _repo.Add(mercadoria);
                return RedirectToAction(nameof(Index));
            }
            return View(mercadoria);
        }

        // GET: Mercadoria/Edit/5
        public IActionResult Edit(int id)
        {
            var mercadoria = _repo.Find(id);
            if (mercadoria == null)
            {
                return NotFound();
            }
            return View(mercadoria);
        }

        // POST: Mercadoria/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Nome,Descricao,Tipo,Fabricante")] Mercadoria mercadoria)
        {
            if (id != mercadoria.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _repo.Update(mercadoria);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_repo.MercadoriaExists(mercadoria.Id))
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
            else
            {
                return BadRequest();
            }
          //  return View(mercadoria);
        }

        // GET: Mercadoria/Delete/5
        public IActionResult Delete(int id)
        {
            var mercadoria = _repo.Find(id);
            if (mercadoria == null)
            {
                return NotFound();
            }

            return View(mercadoria);
        }

        // POST: Mercadoria/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var mercadoria = _repo.Find(id);
            if (mercadoria == null)
            {
                return NotFound();
            }
            _repo.Remove(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
