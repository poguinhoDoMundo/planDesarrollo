namespace pln_v2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PDP.ESTADOS_PROY")]
    public partial class ESTADOS_PROY
    {
        
        [Key]
        public decimal ID_ESTADO { get; set; }

        [StringLength(50)]
        public string NOM_ESTADO { get; set; }
    }
}
