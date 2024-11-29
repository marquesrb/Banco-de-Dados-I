using System;
using System.Collections.Generic;

namespace prjGura.Models;

public partial class Pet
{
    public int Idpet { get; set; }

    public string Nome { get; set; } = null!;

    public string? Porte { get; set; }

    public string? Raca { get; set; }

    public int? Idade { get; set; }

    public string? Idcliente { get; set; }

    public virtual Cliente? IdclienteNavigation { get; set; }

    public virtual ICollection<Servico> Servicos { get; set; } = new List<Servico>();
}
