// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace avanphamaceuticalsmanagement.Shared.Models;

[Index("ConsumedTime", Name = "IX_PersistedGrants_ConsumedTime")]
[Index("Expiration", Name = "IX_PersistedGrants_Expiration")]
[Index("SubjectId", "ClientId", "Type", Name = "IX_PersistedGrants_SubjectId_ClientId_Type")]
[Index("SubjectId", "SessionId", "Type", Name = "IX_PersistedGrants_SubjectId_SessionId_Type")]
public partial class PersistedGrant
{
    [Key]
    [StringLength(200)]
    public string Key { get; set; }

    [Required]
    [StringLength(50)]
    public string Type { get; set; }

    [StringLength(200)]
    public string SubjectId { get; set; }

    [StringLength(100)]
    public string SessionId { get; set; }

    [Required]
    [StringLength(200)]
    public string ClientId { get; set; }

    [StringLength(200)]
    public string Description { get; set; }

    public DateTime CreationTime { get; set; }

    public DateTime? Expiration { get; set; }

    public DateTime? ConsumedTime { get; set; }

    [Required]
    public string Data { get; set; }
}