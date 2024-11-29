namespace prjGura.ViewModel
{
    public class PetsViewModel
    {
        public int Idpet { get; set; }
        public string Nome { get; set; } = null!;
        public string? NomeCliente { get; set; }
        public string? Porte { get; set; }
        public string? Raca { get; set; }
        public int? Idade { get; set; }
        public string? Idcliente { get; set; }
        public DateOnly? UltimaIda { get; set; }
    }
}