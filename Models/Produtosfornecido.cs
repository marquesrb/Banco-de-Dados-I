using System;
using System.Collections.Generic;

namespace prjGura.Models;

public partial class Produtosfornecido
{
    public int Idproduto { get; set; }

    public string Idfornecedor { get; set; } = null!;

    public decimal? PrecoCusto { get; set; }

    public int? Quantidade { get; set; }

    public DateOnly Data { get; set; }

    public virtual Fornecedor IdfornecedorNavigation { get; set; } = null!;

    public virtual Produto IdprodutoNavigation { get; set; } = null!;
}
