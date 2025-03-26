namespace Eventos.Models
{
    public class Participante
    {
        public int ParticipanteID { get; set; }
        public string Nome { get; set; }
        public int EventoID { get; set; }
        public Evento Evento { get; set; }

        // Novas informações
        public DateTime DataNascimento { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string CPF { get; set; }

    }
}
