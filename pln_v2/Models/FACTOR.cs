namespace pln_v2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using Oracle.ManagedDataAccess.Client;
    using System.Data;
    using System.ComponentModel;


    [Table("PDP.FACTOR")]
    public partial class FACTOR
    {


        [Key]
        public int ID_FACTOR { get; set; }

        [DisplayName("Nombre"), Required(ErrorMessage = "La comunidad es requerida")]
        [StringLength(150)]
        public string NOM_FACTOR { get; set; }

        [DisplayName("Descripción"), Required( AllowEmptyStrings =true) ]
        [StringLength(500)]
        public string DESCRIPCION { get; set; }

        [DisplayName("Imagen"), Required(AllowEmptyStrings = true, ErrorMessage = "Se necesita una imagen para continuar")]
        [StringLength(50)]
        public string IMG { get; set; }

        public int MAX_FAVORITO { get; set; }
        

        public List<FACTOR> getFactores()
        {
            List<FACTOR> factor = new List<FACTOR>();

            try
            {
                using (plnContext context = new plnContext())
                {
                    string sql = " SELECT * "
                               + " FROM FACTOR ";

                    factor = context.Database.SqlQuery<FACTOR>(sql).ToList();

                }
            }
            catch { throw; }

            return factor;
        }


        public int addFactor( string nombre, string descripcion, string imagen, string maximo  )
        {

            int total;

            try
            {
                using (plnContext pc = new plnContext())
                {
                    string sql = "BEGIN AD_FACTOR( :INOM_FACTOR, :IDESCRIPCION, :IIMG , :IMAX); END;";

                    OracleParameter pNombre = new OracleParameter("INOM_FACTOR", this.NOM_FACTOR );
                    OracleParameter pDescripcion = new OracleParameter("IDESCRIPCION", this.DESCRIPCION  );
                    OracleParameter pImagen = new OracleParameter("IIMG", this.IMG );
                    OracleParameter pMaximo = new OracleParameter("IMAX", this.MAX_FAVORITO );

                    total = pc.Database.ExecuteSqlCommand(sql, pNombre, pDescripcion, pImagen, pMaximo);
                }

            }
            catch { throw; }

            return total;
        }


        public int get_Proyectos_x_Factor( int factor)
        {
       
            int total = 0;

            try
            {
                using (plnContext context = new plnContext())
                {
                    string sql = " SELECT COUNT(*) total"
                               + " FROM PROYECTO PR "
                               + " WHERE  PR.ID_FACTOR = :FACTOR ";

                    OracleParameter pFactor = new OracleParameter("FACTOR", factor);
                    total = context.Database.SqlQuery<int>(sql, pFactor).Single();

                }
            }
            catch { }

            return total;
        }
 


    }
}
