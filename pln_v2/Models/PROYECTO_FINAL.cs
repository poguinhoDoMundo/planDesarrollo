namespace pln_v2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PDP.PROYECTO_FINAL")]
    public partial class PROYECTO_FINAL
    {
        public decimal? ID_PROYECTO { get; set; }

        [Key]
        public decimal ID_FINAL { get; set; }

        public decimal? NOTA_PROYECTO { get; set; }

        public decimal? PUESTO_PROYECTO { get; set; }

        public DateTime? FECHA { get; set; }

        [StringLength(150)]
        public string RESPONSABLE { get; set; }

        [StringLength(1)]
        public string APROBADO { get; set; }

        public virtual PROYECTO PROYECTO { get; set; }
    }
}
