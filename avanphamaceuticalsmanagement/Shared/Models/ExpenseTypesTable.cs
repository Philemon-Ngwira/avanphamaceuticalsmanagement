﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace avanphamaceuticalsmanagement.Shared.Models;

[Table("ExpenseTypesTable")]
public partial class ExpenseTypesTable
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string ExpenseTypes { get; set; }

    [InverseProperty("ExpenseType")]
    public virtual ICollection<ExpensesTable> ExpensesTables { get; } = new List<ExpensesTable>();
}