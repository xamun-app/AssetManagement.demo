using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetManagementProject.web.Entity
{
    public class BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public virtual int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        //[ConcurrencyCheck]
        //[Timestamp]
        public byte[] RowVersion { get; set; }


    }
}
