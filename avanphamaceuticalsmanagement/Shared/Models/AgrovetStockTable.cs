﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace avanphamaceuticalsmanagement.Shared.Models;

[Table("AgrovetStockTable")]
public partial class AgrovetStockTable
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string AgrovetName { get; set; }

    public int? Quantity { get; set; }

    public double? Price { get; set; }

    [InverseProperty("Agrovet")]
    public virtual ICollection<PharmacyTransactionsTable> PharmacyTransactionsTables { get; } = new List<PharmacyTransactionsTable>();
}