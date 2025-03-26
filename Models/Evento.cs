namespace Eventos.Models
{
    public class Evento
    {
        public int EventoID { get; set; }
        public string Nome { get; set; }

        public ICollection<Participante> Participantes { get; set; }





    }
}
