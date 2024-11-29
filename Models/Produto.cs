using System;
using System.Collections.Generic;

namespace prjGura.Models;

public partial class Produto
{
    public int Id { get; set; }

    public string? Nome { get; set; }

    public decimal? PrecoVenda { get; set; }

    public virtual ICollection<Produtosfornecido> Produtosfornecidos { get; set; } = new List<Produtosfornecido>();

    public virtual ICollection<Vendadeproduto> Vendadeprodutos { get; set; } = new List<Vendadeproduto>();
}
