using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Eventos.Models
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<Context>();

                // Aplica migrações automaticamente (caso ainda não aplicadas)
                context.Database.Migrate();

                // Verifica se já existem eventos no banco
                if (context.Eventos.Any())
                {
                    return; // Banco já populado, não há necessidade de inserir novamente
                }

                // Criação de eventos iniciais
                var evento1 = new Evento { Nome = "Conferência de Tecnologia" };
                var evento2 = new Evento { Nome = "Workshop de Programação" };

                context.Eventos.AddRange(evento1, evento2);
                context.SaveChanges();

                // Criando participantes para os eventos
                var participantes = new[]
                {
                    new Participante { Nome = "Alice", EventoID = evento1.EventoID },
                    new Participante { Nome = "Carlos", EventoID = evento1.EventoID },
                    new Participante { Nome = "Bruno", EventoID = evento2.EventoID },
                    new Participante { Nome = "Mariana", EventoID = evento2.EventoID }
                };

                context.Participantes.AddRange(participantes);
                context.SaveChanges();
            }
        }
    }
}
