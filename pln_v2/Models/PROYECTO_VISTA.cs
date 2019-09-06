namespace pln_v2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PDP.PROYECTO_VISTA")]
    public partial class PROYECTO_VISTA
    {
        [Key]
        public decimal ID_VISTA { get; set; }

        public decimal? ID_PROYECTO { get; set; }

        public DateTime? FECHA { get; set; }

        [StringLength(1)]
        public string ESTADO { get; set; }

        public DateTime? TERMINADO { get; set; }

        public virtual PROYECTO PROYECTO { get; set; }
    }
}
