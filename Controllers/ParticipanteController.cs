    using Microsoft.AspNetCore.Mvc;
    using Eventos.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using X.PagedList.Mvc.Core;
    using X.PagedList;
    using X.PagedList.Extensions;

    namespace Eventos.Controllers
    {
        public class ParticipanteController : Controller
        {
            public Context context;

            public ParticipanteController(Context ctx)
            {
                context = ctx;
            }


        public IActionResult Index(int pagina = 1)
        {
            // Carregar os eventos da base de dados
            var eventos = context.Eventos.ToList();  // Aqui você carrega todos os eventos

            // Passar a lista de eventos para a ViewBag
            ViewBag.Eventos = eventos;

            // Carregar os participantes e paginar
            var participantes = context.Participantes
                .Include(p => p.Evento) // Carrega os eventos relacionados
                .OrderBy(p => p.Nome)
                .ToPagedList(pagina, 5);

            return View(participantes); // Aqui você retorna a lista de participantes para a View
        }



        public IActionResult Create()
            {
                // Utiliza uma Viewbag para gerar uma lista com os nomes dos eventos
                ViewBag.EventoID = new SelectList(context.Eventos
                    .OrderBy(e => e.Nome), "EventoID", "Nome");
                return View();
            }

        [HttpPost]
        public IActionResult Create(Participante participante)
        {
            
            
                // Adiciona o participante ao contexto de banco de dados
                context.Participantes.Add(participante);
                context.SaveChanges();
                return RedirectToAction("Index");
            

            // Se os dados forem inválidos, repassa o formulário com os erros
            return View(participante);
        }

        public IActionResult Details(int id)
            {
                var participante = context.Participantes
                    .Include(p => p.Evento)
                    .FirstOrDefault(p => p.ParticipanteID == id);
                return View(participante);
            }

            public IActionResult Edit(int id)
            {
                var participante = context.Participantes.Find(id);
                ViewBag.EventoID = new SelectList(context.Eventos.OrderBy(e => e.Nome), "EventoID", "Nome");
                return View(participante);
            }

            [HttpPost]
            public IActionResult Edit(Participante participante)
            {
                // Informa à EF que o registro será modificado
                context.Entry(participante).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            public IActionResult Delete(int id)
            {
                var participante = context.Participantes
                    .Include(p => p.Evento)
                    .FirstOrDefault(p => p.ParticipanteID == id);
                return View(participante);
            }

            [HttpPost]
            public IActionResult Delete(Participante participante)
            {
                context.Participantes.Remove(participante);
                context.SaveChanges();
                return RedirectToAction("Index");
        }

        public IActionResult Certificado(int id)
        {
            var participante = context.Participantes
                .Include(p => p.Evento)  // Inclui os dados do evento relacionado
                .FirstOrDefault(p => p.ParticipanteID == id);

            if (participante == null)
            {
                return NotFound();
            }

            return View(participante);
        }
        // Ação para editar o evento de um participante
        // Método para editar o evento do participante
        public IActionResult AlterarEvento(int id)
        {
            // Buscar o participante a ser editado
            var participante = context.Participantes
                .Include(p => p.Evento)  // Incluir evento associado
                .FirstOrDefault(p => p.ParticipanteID == id);

           
               // Caso o participante não exista
            

            // Passar os eventos para o dropdown
            ViewBag.EventoID = new SelectList(context.Eventos.OrderBy(e => e.Nome), "EventoID", "Nome", participante.EventoID);

            return View(participante);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AlterarEvento(Participante participante)
        {
            
            
                // Busca o participante existente
                var participanteExistente = context.Participantes
                    .FirstOrDefault(p => p.ParticipanteID == participante.ParticipanteID);

                
                     // Se o participante não for encontrado
                

                // Atualiza o evento do participante
                participanteExistente.EventoID = participante.EventoID;

                // Salva as mudanças no banco de dados
                context.SaveChanges();

                // Redireciona de volta para a página de listagem de participantes
                return RedirectToAction("Index");
            

            // Caso o ModelState não seja válido, ou seja, algum erro de validação,
            // repassa a lista de eventos novamente para o dropdown e retorna à view
            ViewBag.EventoID = new SelectList(context.Eventos.OrderBy(e => e.Nome), "EventoID", "Nome", participante.EventoID);
            return View(participante); // Retorna para a view com os erros de validação
        }

    }




}

