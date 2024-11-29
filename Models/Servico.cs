using System;
using System.Collections.Generic;

namespace prjGura.Models;

public partial class Servico
{
    public int Idservico { get; set; }

    public string Tipo { get; set; } = null!;

    public decimal Preco { get; set; }

    public DateOnly Data { get; set; }

    public TimeOnly Horario { get; set; }

    public bool? Status { get; set; }

    public string? Idcaixa { get; set; }

    public int? Idpet { get; set; }

    public string? Idbanhista { get; set; }

    public int? Idvenda { get; set; }

    public virtual Funcionario? IdbanhistaNavigation { get; set; }

    public virtual Funcionario? IdcaixaNavigation { get; set; }

    public virtual Pet? IdpetNavigation { get; set; }

    public virtual Vendum? IdvendaNavigation { get; set; }
}
