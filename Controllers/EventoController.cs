using Microsoft.AspNetCore.Mvc;
using Eventos.Models;
using Microsoft.EntityFrameworkCore;
using X.PagedList.Mvc.Core;
using X.PagedList;
using X.PagedList.Extensions;

namespace Eventos.Controllers
{
    public class EventoController : Controller
    {
        private readonly Context _context;

        public EventoController(Context context)
        {
            _context = context;
        }

        public IActionResult Index(int pagina = 1)
        {
            return View(_context.Eventos
                .AsQueryable()
                .ToPagedList(pagina, 5));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Evento evento)
        {
           
          
                _context.Add(evento);
                _context.SaveChanges();
                return RedirectToAction("Index");
            
            
        }

        public IActionResult Details(int id, string searchString)
        {
            var evento = _context.Eventos
                .Include(e => e.Participantes) // Inclui os participantes do evento
                .FirstOrDefault(e => e.EventoID == id);

            if (evento == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                evento.Participantes = evento.Participantes
                    .Where(p => p.Nome.Contains(searchString))
                    .ToList();
            }

            return View(evento);
        }



        public IActionResult Edit(int id)
        {
            var evento = _context.Eventos.Find(id);
            if (evento == null)
            {
                return NotFound();
            }
            return View(evento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Evento evento)
        {
            
            
                _context.Update(evento);
                _context.SaveChanges();
                return RedirectToAction("Index");
            
           
        }

        public IActionResult Delete(int id)
        {
            var evento = _context.Eventos.Find(id);
            if (evento == null)
            {
                return NotFound();
            }
            return View(evento);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var evento = _context.Eventos.Find(id);
            if (evento != null)
            {
                _context.Eventos.Remove(evento);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
