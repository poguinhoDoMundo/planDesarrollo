namespace pln_v2.Models
{
    using Oracle.ManagedDataAccess.Client;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("PDP.USUARIO")]
    public partial class USUARIO
    {
        [Key]
        [Column(Order = 0)]
        public decimal ID { get; set; }

        [Key]
        [Column("USUARIO", Order = 1)]
        [StringLength(20)]
        public string USUARIO1 { get; set; }

        [StringLength(50)]
        public string CLAVE { get; set; }

        [StringLength(50)]
        public string NOMBRES { get; set; }

        [StringLength(50)]
        public string TIPO_USUARIO { get; set; }


        public bool isUser( string user, string pass  )
        {
            int total = 0;
            try
            {
                using (plnContext context = new plnContext())
                {
                    string sql = " SELECT COUNT( ID ) TOTAL "
                               + " FROM USUARIO "
                               + " WHERE USUARIO = :USUARIO AND CLAVE = REGIS.MD5(:PASS) ";

                    OracleParameter pUsuario = new OracleParameter("USUARIO", user);
                    OracleParameter pContrasena = new OracleParameter("PASS", pass );

                    total = context.Database.SqlQuery<int>(sql, pUsuario, pContrasena).Single();

                }
            }
            catch { throw; }

            return (total == 0) ? false : true;
        }
        
        public string getTipoUsuario(  string user )
        {
            string usuario = "";

            try
            {
                using (plnContext context = new plnContext())
                {

                    string sql = " SELECT MAX(GETUSUARIO_JERARQUIA( USUARIO )) "
                               + " FROM USUARIO "
                               + " WHERE USUARIO = :USUARIO ";

                    OracleParameter pClase = new OracleParameter("USUARIO", user);

                    usuario = context.Database.SqlQuery<string>(sql, pClase).Single();
                }

            }
            catch { throw; }
            
            return usuario;
        }


        public static string getNombre( string cedula )
        {
            string nombre = "";

            try
            {
                using (plnContext context = new plnContext())
                {
                    string sql = " SELECT MAX(INITCAP(NOMBRES)) NOMBRES "
                               + " FROM USUARIO "
                               + " WHERE USUARIO = :NUM_DOC ";

                    OracleParameter pCedula = new OracleParameter("NUM_DOC", cedula );
                    nombre = context.Database.SqlQuery<string>(sql, pCedula).Single();
                }

            }
            catch { throw; }

            return nombre;
        }


        public static string getDocumentoProyecto(string id)
        {
            string num_doc = "";

            try
            {
                using (plnContext context = new plnContext())
                {
                    string sql = " SELECT GET_CEDULAPROYECTO( :ID ) "
                               + " FROM DUAL ";

                    OracleParameter pCedula = new OracleParameter("ID", id);
                    num_doc = context.Database.SqlQuery<string>(sql, id).Single();
                }

            }
            catch { throw; }

            return num_doc;
        }



        public static string getMail(string cedula)
        {
            string mail = "";

            try
            {
                using (plnContext context = new plnContext())
                {
                    string sql = " SELECT GETMAIL( :NUM_DOC ) FROM DUAL ";
                    
                    OracleParameter pCedula = new OracleParameter("NUM_DOC", cedula);
                    mail = context.Database.SqlQuery<string>(sql, pCedula).Single();
                }

            }
            catch { throw; }

            return mail;
        }

    }
}
