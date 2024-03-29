﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace avanphamaceuticalsmanagement.Shared.Models;

[Table("StockCategoryTable")]
public partial class StockCategoryTable
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string StockCategoryName { get; set; }

    [InverseProperty("StockCategory")]
    public virtual ICollection<PharmacyTransactionsTable> PharmacyTransactionsTables { get; } = new List<PharmacyTransactionsTable>();

    [InverseProperty("RequestCategory")]
    public virtual ICollection<RestockRequestsTable> RestockRequestsTables { get; } = new List<RestockRequestsTable>();

    [InverseProperty("CategoryNavigation")]
    public virtual ICollection<StockTable> StockTables { get; } = new List<StockTable>();
}