using System;
using System.Collections.Generic;

namespace prjGura.Models;

public partial class Cliente
{
    public string Cpf { get; set; } = null!;

    public string Nome { get; set; } = null!;

    public string Telefone { get; set; } = null!;

    public int? QuantidadePets { get; set; }

    public virtual ICollection<Pet> Pets { get; set; } = new List<Pet>();

    public virtual ICollection<Vendum> Venda { get; set; } = new List<Vendum>();
}
