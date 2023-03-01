﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace avanphamaceuticalsmanagement.Shared.Models;

[Table("StockTable")]
public partial class StockTable
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Name { get; set; }

    public int? Quanity { get; set; }

    public double? Price { get; set; }

    public int? Category { get; set; }

    [ForeignKey("Category")]
    [InverseProperty("StockTables")]
    public virtual StockCategoryTable CategoryNavigation { get; set; }
}