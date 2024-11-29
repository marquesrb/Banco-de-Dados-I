using System;
using System.Collections.Generic;

namespace prjGura.Models;

public partial class Vendadeproduto
{
    public int Idvenda { get; set; }

    public int Idproduto { get; set; }

    public decimal? Valortotalproduto { get; set; }

    public int? Quantidadevendida { get; set; }

    public virtual Produto IdprodutoNavigation { get; set; } = null!;

    public virtual Vendum IdvendaNavigation { get; set; } = null!;
}
