using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
namespace Eventos.Models

{
    public class Context : DbContext

    {
        public Context (DbContextOptions<Context> options)
            : base(options)
        {
        }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Participante> Participantes { get; set; }  
    }
}
