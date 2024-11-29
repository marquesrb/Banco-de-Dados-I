using System;
using System.Collections.Generic;

namespace prjGura.Models;

public partial class Vendum
{
    public int Id { get; set; }

    public decimal Valortotal { get; set; }

    public DateOnly Data { get; set; }

    public string? Idcliente { get; set; }

    public virtual Cliente? IdclienteNavigation { get; set; }

    public virtual ICollection<Servico> Servicos { get; set; } = new List<Servico>();

    public virtual ICollection<Vendadeproduto> Vendadeprodutos { get; set; } = new List<Vendadeproduto>();
}
