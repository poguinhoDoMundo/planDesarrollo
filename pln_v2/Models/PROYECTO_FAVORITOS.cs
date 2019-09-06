namespace pln_v2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PDP.PROYECTO_FAVORITOS")]
    public partial class PROYECTO_FAVORITOS
    {
        [Key]
        public decimal ID_FAVORITO { get; set; }

        public decimal? ID_PROYECTO { get; set; }

        public DateTime? FECHA_VOTO { get; set; }

        [StringLength(1)]
        public string ESTADO { get; set; }

        [StringLength(100)]
        public string USUARIOVOTO { get; set; }

        public virtual PROYECTO PROYECTO { get; set; }
    }
}
