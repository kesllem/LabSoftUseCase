using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AppTask.Models;

public partial class Funcionario
{
    public int Codigo { get; set; }

    public string Nome { get; set; } = null!;

    public string Cargo { get; set; } = null!;

    public int DepartamentoId { get; set; }

    [ForeignKey("DepartamentoId")]
    [ValidateNever]
    public virtual Departamento Departamento { get; set; } = null!;

    [ValidateNever]
    public virtual ICollection<Tarefa> Tarefas { get; set; } = new List<Tarefa>();
}