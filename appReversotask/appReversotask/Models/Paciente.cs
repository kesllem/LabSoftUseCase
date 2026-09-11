using System;
using System.Collections.Generic;

namespace appReversotask.Models;

public partial class Paciente
{
    public int Codigo { get; set; }

    public string Nome { get; set; } = null!;

    public string Cpf { get; set; } = null!;

    public string Telefone { get; set; } = null!;

    public DateOnly DataNascimento { get; set; }

    public virtual ICollection<Consultum> Consulta { get; set; } = new List<Consultum>();
}
