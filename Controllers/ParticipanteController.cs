    using Microsoft.AspNetCore.Mvc;
    using Eventos.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using X.PagedList.Mvc.Core;
    using X.PagedList;
    using X.PagedList.Extensions;
    using System.Drawing.Imaging;
    using System.Drawing;

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
               
                var eventos = context.Eventos.ToList();  

               
                ViewBag.Eventos = eventos;

                
                var participantes = context.Participantes
                    .Include(p => p.Evento) 
                    .OrderBy(p => p.Nome)
                    .ToPagedList(pagina, 5);

                return View(participantes); 
            }



            public IActionResult Create()
            {
                
                ViewBag.EventoID = new SelectList(context.Eventos
                    .OrderBy(e => e.Nome), "EventoID", "Nome");
                return View();
            }

            [HttpPost]
            public IActionResult Create(Participante participante)
            {


              
                context.Participantes.Add(participante);
                context.SaveChanges();
                return RedirectToAction("Index");


                
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
                .Include(p => p.Evento)  
                .FirstOrDefault(p => p.ParticipanteID == id);

            if (participante == null)
            {
                return NotFound();
            }

            
            return GerarCertificado(participante.Nome, participante.Evento.Nome);
        }

        private IActionResult GerarCertificado(string nomeParticipante, string nomeEvento)
        {
            
            string caminhoBase = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "celso.jpg");

            
            if (!System.IO.File.Exists(caminhoBase))
            {
                throw new FileNotFoundException($"Imagem base do certificado não encontrada em {caminhoBase}");
            }

          
            using (Bitmap imagem = new Bitmap(caminhoBase))
            {
                using (Graphics g = Graphics.FromImage(imagem))
                {
                   
                    Font fonte = new Font("Arial", 24, FontStyle.Bold);
                    Brush pincel = Brushes.Black;

                    
                    PointF pontoNome = new PointF(1050, 1550);  
                    PointF pontoEvento = new PointF(850, 1815); 

                    
                    g.DrawString(nomeParticipante, fonte, pincel, pontoNome);

                    
                    g.DrawString(nomeEvento, fonte, pincel, pontoEvento);
                }

               
                using (var ms = new MemoryStream())
                {
                   
                    imagem.Save(ms, ImageFormat.Jpeg);
                    ms.Seek(0, SeekOrigin.Begin);

                    
                    return File(ms.ToArray(), "image/jpeg");
                }
            }
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

