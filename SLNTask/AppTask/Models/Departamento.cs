using System;
using System.Collections.Generic;

namespace AppTask.Models;

public class Departamento
{
    public int Codigo { get; set; }
    public string Nome { get; set; } = null!;
    public string Sigla { get; set; } = null!;
    public bool Ativo { get; set; }

    public virtual ICollection<Funcionario> Funcionario { get; set; }
        = new List<Funcionario>();
}

