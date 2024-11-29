using System;
using System.Collections.Generic;

namespace prjGura.Models;

public partial class Fornecedor
{
    public string Cnpj { get; set; } = null!;

    public string RazaoSocial { get; set; } = null!;

    public string Telefone { get; set; } = null!;

    public string Endereco { get; set; } = null!;

    public virtual ICollection<Produtosfornecido> Produtosfornecidos { get; set; } = new List<Produtosfornecido>();
}
