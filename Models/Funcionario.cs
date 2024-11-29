using System;
using System.Collections.Generic;

namespace prjGura.Models;

public partial class Funcionario
{
    public string Cpf { get; set; } = null!;

    public string Nome { get; set; } = null!;

    public string Endereco { get; set; } = null!;

    public string Telefone { get; set; } = null!;

    public string? Turno { get; set; }

    public decimal? Salario { get; set; }

    public string? Cargo { get; set; }

    public string? Cpfsupervisiona { get; set; }

    public virtual ICollection<Servico> ServicoIdbanhistaNavigations { get; set; } = new List<Servico>();

    public virtual ICollection<Servico> ServicoIdcaixaNavigations { get; set; } = new List<Servico>();
}
