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
    using System.Web;

    [Table("PDP.PROYECTO")]
    public partial class PROYECTO
    {
        #region atributos
        [Key]
        public decimal ID_PROYECTO { get; set; }

        [DisplayName("Condición de acreditación"), Required(ErrorMessage = "Seleccione un factor")]
        public decimal ID_FACTOR { get; set; }

        [DisplayName("Nombre"), Required(ErrorMessage = "Ingrese un nombre para la idea")]
        [StringLength(300)]
        public string NOM_PROYECTO { get; set; }

        [DisplayName("Descripción"), Required(ErrorMessage = "Ingrese una breve descripcion para la idea")]
        [StringLength(2000)]
        public string DESCRIPCION { get; set; }

        [DisplayName("Costo"), Required(ErrorMessage = "Que costo aproximado tiene la idea")]
        public decimal COSTO { get; set; }

        [DisplayName("Requisitos"), Required(ErrorMessage = "Que requistos deberia cumplir la idea para iniciar ? ")]
        [StringLength(300)]
        public string REQUISITOS { get; set; }

        [StringLength(50)]
        public string IMG { get; set; }

        public DateTime FECHA_INGRESO { get; set; }

        public int ESTADO { get; set; }

        [StringLength(150)]
        public string RESPONSABLE { get; set; }

        [DisplayName("Objetivo"), Required(ErrorMessage = "Cual es el objetivo principal de la idea")]
        [StringLength(150)]
        public string OBJETIVO { get; set; }

        [DisplayName("Meta (Opcional)")]
        [StringLength(200)]
        public string META { get; set; }
        

        [DisplayName("Prioridad (Opcional)")]
        public int ID_PRIORIDAD { get; set; }

        [StringLength(50)]
        public string ARCHIVO { get; set; }

        public virtual ESTADOS_PROY ESTADOS_PROY { get; set; }
        #endregion

        public List<PROYECTO> getProyectos( int id_factor )
        {
            List<PROYECTO> proyectos = new List<PROYECTO>();

            try
            {
                using ( plnContext context = new plnContext() )
                {
                    string sql = " SELECT * "
                               + " FROM PROYECTO PR "
                               + " WHERE  PR.ID_FACTOR = :FACTOR AND ESTADO IN (1,5) ";

                    OracleParameter pNombre = new OracleParameter(":FACTOR", id_factor );
                    proyectos = context.Database.SqlQuery<PROYECTO>(sql, pNombre).ToList();
                }
            }
            catch { throw; }
            
            return proyectos;
        } 

        public PROYECTO getProyecto( int id_proyecto )
        {
            PROYECTO proyecto = new PROYECTO();

            try
            {
                using (plnContext context = new plnContext())
                {
                    string sql = " SELECT * "
                               + " FROM PROYECTO PR "
                               + " WHERE  PR.ID_PROYECTO = :PROYECTO ";

                    OracleParameter pNombre = new OracleParameter(":PROYECTO", id_proyecto);
                    proyecto = context.Database.SqlQuery<PROYECTO>(sql, pNombre).SingleOrDefault();
                }
            }
            catch { throw; }

            return proyecto;
        }

        public static string marcaFavorito( string usuario, string id_proyecto )
        {
            string salida = "";
            int total;

            try
            {
                using ( plnContext pc = new plnContext())
                {
                    string sql = "BEGIN AD_FAVORITO( :PROY_ID, :USUARIO, :SALE); END;";

                    OracleParameter pId = new OracleParameter("PROY_ID", id_proyecto );
                    OracleParameter pUsuario = new OracleParameter("USUARIO", usuario );

                    OracleParameter pResult = new OracleParameter("SALE", OracleDbType.Varchar2, 150);
                    pResult.Direction = ParameterDirection.Output;

                    total = pc.Database.ExecuteSqlCommand(sql, pId, pUsuario , pResult);
                    salida = Convert.ToString(pResult.Value);

                }

            }
            catch { throw; }
            
            return salida;
        }
        
        public static string marcaCarro( string usuario, string id_proyecto)
        {
            string salida = "";
            int total;

            try
            {
                using (plnContext pc = new plnContext())
                {
                    string sql = "BEGIN AD_CARRO( :PROY_ID, :USUARIO, :SALE); END;";

                    OracleParameter pId = new OracleParameter("PROY_ID", id_proyecto);
                    OracleParameter pUsuario = new OracleParameter("USUARIO", usuario);

                    OracleParameter pResult = new OracleParameter("SALE", OracleDbType.Varchar2, 150);
                    pResult.Direction = ParameterDirection.Output;

                    total = pc.Database.ExecuteSqlCommand(sql, pId, pUsuario, pResult);
                    salida = Convert.ToString(pResult.Value);

                }

            }
            catch { throw; }

            return salida;
        }
        
        public static bool isFavorito( int id_proyecto, string usuario )
        {
            int total = 0;

            try
            {
                using (plnContext context = new plnContext())
                {
                    string sql = " SELECT COUNT( PF.ID_PROYECTO ) "
                               + " FROM PDP.PROYECTO_FAVORITOS PF "
                               + " WHERE PF.ID_PROYECTO = :ID_PROYECTO AND PF.USUARIOVOTO = :USUARIO " 
                               + "                         AND PF.ESTADO = 'A' ";

                    OracleParameter pProyecto = new OracleParameter("ID_PROYECTO", id_proyecto);
                    OracleParameter pUsuario = new OracleParameter("USUARIO", usuario );

                    total = context.Database.SqlQuery<int>(sql, pProyecto, pUsuario).Single();

                }
            }
            catch { }
            
            return (total==0)?false:true;
        }
        
        public static bool isSalvado(int id_proyecto, string usuario)
        {
            int total = 0;

            try
            {
                using (plnContext context = new plnContext())
                {
                    string sql = " SELECT COUNT( PF.ID_PROYECTO ) "
                               + " FROM PDP.PROYECTO_VISTA PF "
                               + " WHERE PF.ID_PROYECTO = :ID_PROYECTO AND PF.USUARIO_VISTA = :USUARIO "
                               + "                         AND PF.ESTADO = 'A' ";

                    OracleParameter pProyecto = new OracleParameter("ID_PROYECTO", id_proyecto);
                    OracleParameter pUsuario = new OracleParameter("USUARIO", usuario);

                    total = context.Database.SqlQuery<int>(sql, pProyecto, pUsuario).Single();

                }
            }
            catch { }

            return (total == 0) ? false : true;
        }
    
        public string addProyecto( string responsable, string file )
        {
            int total = 0;
            string salida = "";

            try
            {
                using ( plnContext context = new plnContext() )
                {
                    string sql = "BEGIN AD_PROYECTO_V2( :IID_FACTOR, :INOMBRE,:IDESCRIPCION, :ICOSTO, "
                               + " :IREQUISITOS, :IIMG, :IOBJETIVO, :USUARIO, :IMETA, :IPRIORIDAD, :IARCHIVO, :SALE); END;";

                    OracleParameter pFactor = new OracleParameter("IID_FACTOR", this.ID_FACTOR );
                    OracleParameter pNombre = new OracleParameter("INOMBRE", this.NOM_PROYECTO );
                    OracleParameter pDesc = new OracleParameter("IDESCRIPCION", this.DESCRIPCION );
                    OracleParameter pCosto = new OracleParameter("ICOSTO", this.COSTO );
                    OracleParameter pReq = new OracleParameter("IREQUISITOS", this.REQUISITOS );
                    OracleParameter pImg = new OracleParameter("IIMG", this.IMG );
                    OracleParameter pObj = new OracleParameter("IOBJETIVO", this.OBJETIVO );
                    OracleParameter pUsuario = new OracleParameter("USUARIO", responsable );
                    OracleParameter pMeta = new OracleParameter("IMETA", this.META  );
                    OracleParameter pPrioridad = new OracleParameter("IPRIORIDAD", this.ID_PRIORIDAD );
                    OracleParameter pArchivo = new OracleParameter("IARCHIVO", file );


                    OracleParameter pResult = new OracleParameter("SALE", OracleDbType.Varchar2, 150);
                    pResult.Direction = ParameterDirection.Output;

                    total = context.Database.ExecuteSqlCommand(sql, pFactor, pNombre, pDesc, pCosto, pReq,
                                                                    pImg,pObj,pUsuario, pMeta, pPrioridad, pArchivo ,  pResult );
                    salida = Convert.ToString(pResult.Value);
                }
            }
            catch { throw; }

            return salida;
        }

        public List< MisProyecto > getMisPropuestas( string responsable, int tipo )
        {
            List<MisProyecto> proyectos = new List<MisProyecto>();

            try
            {
                using (plnContext context = new plnContext())
                {
                    string sql = getQuery(tipo);

                    OracleParameter pU = new OracleParameter(":USUARIO", responsable);
                    proyectos = context.Database.SqlQuery<MisProyecto>(sql, pU).ToList();                    
                }
            }
            catch { }

            return proyectos;
        }
        
        public string getQuery(int tipo)
        {
            string query = "";

            if (tipo == 1)
                query = " SELECT PF.ID_PROYECTO, F.NOM_FACTOR, PR.NOM_PROYECTO, TO_CHAR(PR.ESTADO) ESTADO "
                      + " FROM PROYECTO_FAVORITOS PF "
                      + " INNER JOIN PROYECTO PR ON PR.ID_PROYECTO = PF.ID_PROYECTO "
                      + " INNER JOIN FACTOR F ON F.ID_FACTOR = PR.ID_FACTOR "
                      + " WHERE PF.USUARIOVOTO = :USUARIO AND PF.ESTADO = 'A' " 
                      + " ORDER BY 2,3 ";

            if (tipo == 2)
                query = " SELECT PF.ID_PROYECTO, F.NOM_FACTOR, PR.NOM_PROYECTO, TO_CHAR(PR.ESTADO) ESTADO  "
                      + " FROM PROYECTO_VISTA PF "
                      + " INNER JOIN PROYECTO PR ON PR.ID_PROYECTO = PF.ID_PROYECTO "
                      + " INNER JOIN FACTOR F ON F.ID_FACTOR = PR.ID_FACTOR "
                      + " WHERE PF.USUARIO_VISTA = :USUARIO AND PF.ESTADO = 'A' "
                      + " ORDER BY 2,3 ";

            if (tipo == 0   )
                query = " SELECT PR.ID_PROYECTO, F.NOM_FACTOR, PR.NOM_PROYECTO, EP.NOM_ESTADO ESTADO "
                      + " FROM PROYECTO PR "
                      + " INNER JOIN FACTOR F ON F.ID_FACTOR = PR.ID_FACTOR "
                      + " INNER JOIN ESTADOS_PROY EP ON EP.ID_ESTADO = PR.ESTADO"
                      + " WHERE PR.RESPONSABLE = :USUARIO "
                      + " ORDER BY 2,3 ";

            if (tipo == 3)
                query = " SELECT PR.ID_PROYECTO, F.NOM_FACTOR, PR.NOM_PROYECTO, EP.NOM_ESTADO ESTADO "
                      + " FROM PROYECTO PR "
                      + " INNER JOIN FACTOR F ON F.ID_FACTOR = PR.ID_FACTOR "
                      + " INNER JOIN ESTADOS_PROY EP ON EP.ID_ESTADO = PR.ESTADO "
                      + " ORDER BY 2,3 ";

            return query;

        }
        
        public List<PROYECTO> getSearch( string testo )
        {
            List<PROYECTO> proyectos = new List<PROYECTO>();

            using ( plnContext context = new plnContext() )
            {
                string sql = " SELECT PR.*  "
                           + " FROM PDP.PROYECTO PR "
                           + " WHERE UPPER(PR.NOM_PROYECTO)LIKE '%' || UPPER(:NOMBRE) || '%' AND ESTADO = 1  "
                           + " ORDER BY 3 ";

                OracleParameter pNombre = new OracleParameter(":NOMBRE",testo);
                proyectos = context.Database.SqlQuery<PROYECTO>( sql,pNombre ).ToList();

            }

            return proyectos;
        }
        

        public List<MisProyecto> getRevision()
        {
            List<MisProyecto> proyectos = new List<MisProyecto>();

            try
            {
                using (plnContext context = new plnContext())
                {
                    string sql = " SELECT ID_PROYECTO, NOM_FACTOR, NOM_PROYECTO, EP.NOM_ESTADO ESTADO "
                               + " FROM PROYECTO P "
                               + " INNER JOIN FACTOR F ON P.ID_FACTOR = F.ID_FACTOR "
                               + " INNER JOIN ESTADOS_PROY EP ON EP.ID_ESTADO = P.ESTADO "
                               + " WHERE P.ESTADO = 0 "
                               + " ORDER BY FECHA_INGRESO ";

                    proyectos = context.Database.SqlQuery<MisProyecto>(sql).ToList();
                }
            }
            catch { }

            return proyectos;            
        }


        public static string aprProyecto( int id_proyecto, int status, string usuario )
        {
            string salida = "";
            int total = 0;

            try
            {
                using (plnContext pc = new plnContext())
                {
                    string sql = "BEGIN APR_PROYECTO( :PROY_ID, :IESTADO,:USUARIO, :SALE); END;";
                    /*APR_PROYECTO( PROY_ID VARCHAR2, IESTADO INTEGER, USUARIO VARCHAR2 , SALE OUT VARCHAR2 )*/

                    OracleParameter pId = new OracleParameter("PROY_ID", id_proyecto);
                    OracleParameter pEstado = new OracleParameter("IESTADO", status );
                    OracleParameter pUsuario = new OracleParameter("USUARIO", usuario);
                    
                    OracleParameter pResult = new OracleParameter("SALE", OracleDbType.Varchar2, 150);
                    pResult.Direction = ParameterDirection.Output;

                    total = pc.Database.ExecuteSqlCommand(sql, pId, pEstado ,pUsuario, pResult);
                    salida = Convert.ToString(pResult.Value);
                }
            }
            catch { }


            return salida;
        }


        public string getEstadoProyecto( decimal id )
        {
            string result = "";

            try
            {
                using ( plnContext context = new plnContext()  )
                {
                    string sql = " SELECT EP.NOM_ESTADO "
                               + " FROM PROYECTO P "
                               + " INNER JOIN ESTADOS_PROY EP ON EP.ID_ESTADO = P.ESTADO "
                               + " WHERE P.ID_PROYECTO = :ID_PROYECTO ";

                    OracleParameter pId = new OracleParameter("ID_PROYECTO",id);
                    result = context.Database.SqlQuery<string>(sql,pId).Single();
                }
            }
            catch { throw; }

            return result;
        }

        public static string getNombreProyecto( int id_proyecto )
        {
            string nombre = "";

            try
            {
                using (plnContext context = new plnContext())
                {
                    string sql = " SELECT NOM_PROYECTO "
                               + " FROM PROYECTO "
                               + " WHERE ID_PROYECTO = :ID_PROYECTO ";

                    OracleParameter pID = new OracleParameter("ID_PROYECTO", id_proyecto );
                    nombre = context.Database.SqlQuery<string>(sql, pID ).Single();
                }

            }
            catch { throw; }

            return nombre;

        }


    }

    public partial class MisProyecto
    {
        public decimal ID_PROYECTO { get; set; }
        public string NOM_FACTOR { get; set; }
        public string NOM_PROYECTO { get; set; }
        public string ESTADO { get; set; }
    }

    public partial class PLAZO
    {
        public int ID_PRIORIDAD { get; set; }
        public string PRIORIDAD { get; set; }


        public List<PLAZO> getPrioridades()
        {

            List<PLAZO> prioridades = new List<PLAZO>();

            using (plnContext context = new plnContext())
            {
                string sql = " SELECT ID_PRIORIDAD, PRIORIDAD "
                           + " FROM EVALPROY_PRIORIDAD "
                           + " WHERE ESTADO = 1 ";

                prioridades = context.Database.SqlQuery<PLAZO>(sql).ToList();

            }

            return prioridades;
        }

    }

    
}