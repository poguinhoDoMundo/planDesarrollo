--
-- Create Schema Script 
--   Database Version   : 12.2.0.1.0 
--   TOAD Version       : 9.6.1.1 
--   DB Connect String  : ACAD 
--   Schema             : PDP 
--   Script Created by  : PROVEDORES 
--   Script Created at  : 21/08/2019 8:30:05 a. m. 
--   Physical Location  :  
--   Notes              :  
--

-- Object Counts: 
--   Functions: 15      Lines of Code: 485 
--   Procedures: 11     Lines of Code: 532 


CREATE OR REPLACE PROCEDURE     AD_PROYECTO_V2( IID_FACTOR NUMBER, INOMBRE  VARCHAR2 , IDESCRIPCION VARCHAR2, ICOSTO NUMBER,
                                         IREQUISITOS VARCHAR2, IIMG VARCHAR, IOBJETIVO VARCHAR2, USUARIO VARCHAR2, 
                                         IMETA VARCHAR2, IPRIORIDAD INTEGER, IARCHIVO VARCHAR2,
                                         SALE OUT VARCHAR2 ) IS
                                         
       EXISTE INTEGER;       
       PROY_ID INTEGER;   
       CONVOCATORIA INTEGER;                        

BEGIN
        
        IF ( GETPUEDE_FECHAS( 1 ) = 1 ) THEN 
            SALE := 'la convocatoria se encuentra vencida' ;
            RETURN;
        END IF;


        SELECT COUNT( P.ID_PROYECTO) INTO EXISTE
        FROM   PROYECTO P
        WHERE  UPPER(P.NOM_PROYECTO) = UPPER(INOMBRE); 
        
        SELECT GET_CONVOCATORIA_ACTIVA INTO CONVOCATORIA
        FROM DUAL;
        
        IF ( EXISTE > 0  ) THEN 
                SALE := 'Ya existe un proyecto con este nombre en nuestra base de datos';
                RETURN;
        END IF; 
        
        SELECT ID_PROYECTO.NEXTVAL INTO PROY_ID
        FROM DUAL;
       
        INSERT INTO PDP.PROYECTO ( ID_PROYECTO, ID_FACTOR, NOM_PROYECTO, 
           DESCRIPCION, COSTO, REQUISITOS, 
           IMG, FECHA_INGRESO, ESTADO, 
           RESPONSABLE, OBJETIVO, ID_CONVOCATORIA, ID_PRIORIDAD, META, ARCHIVO ) 
        VALUES ( PROY_ID, IID_FACTOR, INOMBRE ,
                 IDESCRIPCION, ICOSTO, IREQUISITOS,
                 IIMG, SYSDATE, 0, 
                 USUARIO, IOBJETIVO, CONVOCATORIA, IPRIORIDAD, IMETA, IARCHIVO   );
                  
        INSERT INTO regis.log_acad
            (log_id, log_fecha, log_usuario
            , log_registro,
            log_observaciones, eve_id, log_usuariotipo, log_pagina)
        VALUES
            (regis.log_id_sec.nextval, sysdate, USUARIO
            , '<pr>'||PROY_ID||'</pr>'
            , '+', 113, 0, 'plan_participativo');          
        
        SALE := 'OK';    
        
        COMMIT;
END;
/

SHOW ERRORS;


CREATE OR REPLACE PROCEDURE     AD_PROYECTO( IID_FACTOR NUMBER, INOMBRE  VARCHAR2 , IDESCRIPCION VARCHAR2, ICOSTO NUMBER,
                                         IREQUISITOS VARCHAR2, IIMG VARCHAR, IOBJETIVO VARCHAR2, USUARIO VARCHAR2, SALE OUT VARCHAR2 ) IS
                                         
       EXISTE INTEGER;       
       PROY_ID INTEGER;   
       CONVOCATORIA INTEGER;                        

BEGIN
        
        IF ( GETPUEDE_FECHAS( 1 ) = 1 ) THEN 
            SALE := 'la convocatoria se encuentra vencida' ;
            RETURN;
        END IF;


        SELECT COUNT( P.ID_PROYECTO) INTO EXISTE
        FROM   PROYECTO P
        WHERE  UPPER(P.NOM_PROYECTO) = UPPER(INOMBRE); 
        
        SELECT GET_CONVOCATORIA_ACTIVA INTO CONVOCATORIA
        FROM DUAL;
        
        IF ( EXISTE > 0  ) THEN 
                SALE := 'Ya existe un proyecto con este nombre en nuestra base de datos';
                RETURN;
        END IF; 
        
        SELECT ID_PROYECTO.NEXTVAL INTO PROY_ID
        FROM DUAL;
       
        INSERT INTO PDP.PROYECTO ( ID_PROYECTO, ID_FACTOR, NOM_PROYECTO, 
           DESCRIPCION, COSTO, REQUISITOS, 
           IMG, FECHA_INGRESO, ESTADO, 
           RESPONSABLE, OBJETIVO, ID_CONVOCATORIA ) 
        VALUES ( PROY_ID, IID_FACTOR, INOMBRE ,
                 IDESCRIPCION, ICOSTO, IREQUISITOS,
                 IIMG, SYSDATE, 0, 
                 USUARIO, IOBJETIVO, CONVOCATORIA  );
                  
        INSERT INTO regis.log_acad
            (log_id, log_fecha, log_usuario
            , log_registro,
            log_observaciones, eve_id, log_usuariotipo, log_pagina)
        VALUES
            (regis.log_id_sec.nextval, sysdate, USUARIO
            , '<pr>'||PROY_ID||'</pr>'
            , '+', 113, 0, 'plan_participativo');          
        
        SALE := 'OK';    
        
        COMMIT;
END;
/

SHOW ERRORS;


CREATE OR REPLACE PROCEDURE     APR_PROYECTO( PROY_ID VARCHAR2, IESTADO INTEGER, USUARIO VARCHAR2 , SALE OUT VARCHAR2 )  IS 
    
    EXISTE INTEGER;
    
BEGIN
    
    IF ( GETPUEDE_FECHAS( 1 ) = 1 ) THEN 
        SALE := 'El proyecto no sera marcado, esta por fuera del periodo para realizar esta accion !!!' ;
        RETURN;
    END IF;
    
    IF ( IESTADO NOT IN (0,1,2) ) THEN
        RETURN;
    END IF;
    
    SELECT COUNT( PR.ID_PROYECTO ) INTO EXISTE
    FROM PROYECTO PR
    WHERE PR.ID_PROYECTO = PROY_ID AND PR.ESTADO IN ( 0,1,2 ) ;
    
    IF ( EXISTE = 0 ) THEN
        SALE := 'El proyecto se encuentra en una fase diferente !!!';
        RETURN;
    END IF;
    
    UPDATE  PROYECTO PR 
    SET     PR.ESTADO = IESTADO 
    WHERE   PR.ID_PROYECTO = PROY_ID;
    
    INSERT INTO regis.log_acad
            (log_id, log_fecha, log_usuario
            , log_registro,
            log_observaciones, eve_id, log_usuariotipo, log_pagina)
    VALUES
            (regis.log_id_sec.nextval, sysdate, USUARIO
            , '<pr>'||PROY_ID||'</pr><e>'||IESTADO||'</e>'
            , '+', 114, 0, 'plan_participativo');
            
            
    SALE := 'OK';        
    
    COMMIT;
    
END;
/

SHOW ERRORS;


CREATE OR REPLACE PROCEDURE AD_FACTOR( INOM_FACTOR VARCHAR2,IDESCRIPCION VARCHAR2, IIMG VARCHAR2, IMAX INTEGER  ) IS
    
    
BEGIN
    
    INSERT INTO PDP.FACTOR ( ID_FACTOR, NOM_FACTOR, DESCRIPCION, IMG, MAX_FAVORITO) 
    VALUES ( ID_FACTOR.NEXTVAL, INOM_FACTOR, IDESCRIPCION , IIMG , IMAX );    
    
    
    COMMIT;    
    
END;
/

SHOW ERRORS;


CREATE OR REPLACE procedure     EVALPROY_CALIFICARCRITERIOS_ADD( vIDCRITPROY VARCHAR, vIDPROY VARCHAR, vIDCRIT VARCHAR, vPONDERADO VARCHAR,
                                vCALIFICACION VARCHAR, vUSERCALIF VARCHAR, VDOCCALIF VARCHAR, vCOMENT VARCHAR,
                                vESTADO VARCHAR, vIDCONV VARCHAR,  
                                RESULT OUT VARCHAR2  )                                                             
IS     
    
   
BEGIN
    


         MERGE INTO PDP.EVALPROY_CRITERIOS_PROYECTO ECP
                USING ( SELECT 
                            vIDCRITPROY vIDCRITPROY, vIDPROY vIDPROY, vIDCRIT vIDCRIT, vPONDERADO vPONDERADO, vCALIFICACION vCALIFICACION, 
                            vUSERCALIF vUSERCALIF, VDOCCALIF VDOCCALIF, vCOMENT vCOMENT, vESTADO vESTADO, 
                            vIDCONV vIDCONV
                        FROM DUAL ) d
                    ON ( ECP.ID_CRIT_PROY = d.vIDCRITPROY )
                    WHEN MATCHED THEN UPDATE SET
                                       ID_PROYECTO                  = d.vIDPROY, 
                                       ID_CRITERIO                  = d.vIDCRIT, 
                                       PONDERADO                    = d.vPONDERADO, 
                                       CALIFICACION                 = d.vCALIFICACION, 
                                       FECHA_CALIFICACION           = SYSDATE, 
                                       USUARIO_CALIFICACION         = d.vUSERCALIF, 
                                       DOCUMENTO_CALIFICACION       = d.VDOCCALIF, 
                                       COMENTARIO                   = d.vCOMENT, 
                                       ESTADO                       = d.vESTADO, 
                                       ID_CONVOCATORIA              = d.vIDCONV   
                    WHEN NOT MATCHED THEN
                         INSERT ( 
                                       ID_PROYECTO, ID_CRITERIO, 
                                       PONDERADO, CALIFICACION, FECHA_CALIFICACION, 
                                       USUARIO_CALIFICACION, DOCUMENTO_CALIFICACION, COMENTARIO, 
                                       ESTADO, ID_CONVOCATORIA
                                ) 
                         VALUES ( 
                                   d.vIDPROY, d.vIDCRIT, d.vPONDERADO, d.vCALIFICACION, SYSDATE, d.vUSERCALIF, d.VDOCCALIF, d.vCOMENT,
                                   d.vESTADO, d.vIDCONV
                                );

    
              
                    RESULT := '1';
           
                  
       
   COMMIT;          
   
   EXCEPTION     
     WHEN OTHERS THEN       
       RESULT:= '-1';        
       RAISE;       
       
END;
/

SHOW ERRORS;


CREATE OR REPLACE PROCEDURE     AD_FAVORITO( PROY_ID INTEGER, USUARIO VARCHAR2, SALE OUT VARCHAR2 ) IS
    
    CONT_FAVORITO INTEGER;
    ESTADO_ACTUAL VARCHAR2(2); 
    ESTADO_TMP VARCHAR2(2); 
    PROY_EXISTE INTEGER;
    
    VECES_FACTOR INTEGER;
    FACTOR INTEGER;
    
    
BEGIN
    
    IF ( USUARIO IS NULL ) THEN 
        SALE := 'No se reconoce el usuario, inicie sesion e intentelo nuevamente';
        RETURN;
    END IF; 


    IF ( GETPUEDE_FECHAS( 1 ) = 1 ) THEN 
        SALE := 'El proyecto no sera marcado como favorito, esta por fuera del periodo para realizar esta accion !!!' ;
        RETURN;
    END IF;

    --validaciones --------------------------
    SELECT COUNT( * ) INTO PROY_EXISTE  
    FROM  PROYECTO PR  
    WHERE PR.ID_PROYECTO = PROY_ID AND ESTADO = 1 ;
        
        IF ( PROY_EXISTE = 0 ) THEN 
            SALE := 'Imposible calificar el proyecto, no se encuentra en activo en nuestra base de datos';
            RETURN;
        END IF;    
        
        
    SELECT F.MAX_FAVORITO, F.ID_FACTOR INTO VECES_FACTOR, FACTOR
    FROM   FACTOR F
    INNER  JOIN PROYECTO P ON P.ID_FACTOR = F.ID_FACTOR
    WHERE  P.ID_PROYECTO = PROY_ID;   
    --------------------------------------------
    
    
    
    SELECT COUNT( * ) INTO CONT_FAVORITO
    FROM   PROYECTO_FAVORITOS  PF
    WHERE  PF.ID_PROYECTO = PROY_ID AND PF.USUARIOVOTO = USUARIO;
    
    
    IF ( CONT_FAVORITO = 0 ) THEN
        
        IF (  GETFAVORITOS_USUARIO( USUARIO, FACTOR ) >=  VECES_FACTOR   ) THEN
               SALE :=  'Su voto no sera registrado, solo puede votar ' || VECES_FACTOR || ' veces en esta categoria.'  ;
               RETURN;
        END IF;
        
        -------------------------------------------
        
        INSERT INTO PROYECTO_FAVORITOS( ID_FAVORITO, ID_PROYECTO, FECHA_VOTO, ESTADO, USUARIOVOTO )  
        VALUES( ID_FAVORITO.NEXTVAL, PROY_ID, SYSDATE, 'A', USUARIO  ); 
        
        INSERT INTO regis.log_acad
            (log_id, log_fecha, log_usuario
            , log_registro,
            log_observaciones, eve_id, log_usuariotipo, log_pagina)
            VALUES
            (regis.log_id_sec.nextval, sysdate, USUARIO
            , '<pr>'||PROY_ID||'</pr>'
            , '+', 111, 0, 'plan_participativo');
        
    ELSE
        
        SELECT PF.ESTADO INTO ESTADO_ACTUAL
        FROM   PROYECTO_FAVORITOS PF
        WHERE  PF.ID_PROYECTO = PROY_ID AND PF.USUARIOVOTO = USUARIO ;
        
        
        IF ( ESTADO_ACTUAL = 'A' ) THEN
            ESTADO_TMP := 'R';
        ELSE 
        
            IF (  GETFAVORITOS_USUARIO( USUARIO, FACTOR ) >=  VECES_FACTOR   ) THEN
               SALE :=  'Su voto no sera registrado, solo puede votar ' || VECES_FACTOR || ' en esta categoria'  ;
               RETURN;
            END IF;
        
            ESTADO_TMP := 'A';
        END IF;       
        
                
        UPDATE PROYECTO_FAVORITOS PF
        SET    ESTADO = ESTADO_TMP
        WHERE  PF.ID_PROYECTO = PROY_ID AND PF.USUARIOVOTO = USUARIO;            
        
         
        INSERT INTO regis.log_acad
            (log_id, log_fecha, log_usuario
            , log_registro,
            log_observaciones, eve_id, log_usuariotipo, log_pagina)
            VALUES
            (regis.log_id_sec.nextval, sysdate, USUARIO
            , '<pr>'||PROY_ID||'</pr>'
            , '-', 111, 0, 'plan_participativo');
        
    END IF;
    
    
    SALE := 'OK';
    
    COMMIT;
    
END;
/

SHOW ERRORS;


CREATE OR REPLACE PROCEDURE     AD_CARRO( PROY_ID  NUMBER, USUARIO VARCHAR2, SALE OUT VARCHAR  ) AS
    
    CONT_CARRO INTEGER;
    PROY_EXISTE INTEGER;
    ESTADO_ACTUAL VARCHAR(2);
    ESTADO_TMP VARCHAR(2);
    
BEGIN
    
    IF ( USUARIO IS NULL ) THEN 
        SALE := 'No se reconoce el usuario, inicie sesion e intentelo nuevamente';
        RETURN;
    END IF;


    --validaciones --------------------------
    SELECT COUNT( * ) INTO PROY_EXISTE  
    FROM  PROYECTO PR  
    WHERE PR.ID_PROYECTO = PROY_ID and ESTADO = 1;
        
    IF ( PROY_EXISTE = 0 ) THEN 
       SALE := 'Imposible guardar el proyecto, no se encuentra activo en nuestras bases de datos';
       RETURN;
    END IF;    
        
    SELECT COUNT( * ) INTO CONT_CARRO
    FROM   PROYECTO_VISTA  PV
    WHERE  PV.ID_PROYECTO = PROY_ID AND PV.USUARIO_VISTA = USUARIO;
    
    IF ( CONT_CARRO = 0 ) THEN
        
        INSERT INTO PROYECTO_VISTA( ID_VISTA, ID_PROYECTO, FECHA, ESTADO, USUARIO_VISTA )  
        VALUES( ID_VISTA.NEXTVAL, PROY_ID, SYSDATE, 'A', USUARIO  ); 
        
        INSERT INTO regis.log_acad
            (log_id, log_fecha, log_usuario
            , log_registro,
            log_observaciones, eve_id, log_usuariotipo, log_pagina)
        VALUES
            (regis.log_id_sec.nextval, sysdate, USUARIO
            , '<pr>'||PROY_ID||'</pr>'
            , '+', 112, 0, 'plan_participativo');
        
    ELSE
        
        SELECT PF.ESTADO INTO ESTADO_ACTUAL
        FROM   PROYECTO_VISTA PF
        WHERE  PF.ID_PROYECTO = PROY_ID AND PF.USUARIO_VISTA = USUARIO;
        
        
        IF ( ESTADO_ACTUAL = 'A' ) THEN
            ESTADO_TMP := 'R';
        ELSE         
            ESTADO_TMP := 'A';
        END IF;       
        
                
        UPDATE PROYECTO_VISTA PF
        SET    ESTADO = ESTADO_TMP
        WHERE  PF.ID_PROYECTO = PROY_ID AND PF.USUARIO_VISTA = USUARIO;            
        
         
        INSERT INTO regis.log_acad
            (log_id, log_fecha, log_usuario
            , log_registro,
            log_observaciones, eve_id, log_usuariotipo, log_pagina)
        VALUES
            (regis.log_id_sec.nextval, sysdate, USUARIO
            , '<pr>'||PROY_ID||'</pr>'
            , '-', 112, 0, 'plan_participativo');
        
    END IF;
    
    SALE := 'OK';    
    COMMIT;
    
END;
/

SHOW ERRORS;


CREATE OR REPLACE procedure     EVALPROY_CERRARPROYECTO_ADD( vIDCIERRE VARCHAR, vIDPROY VARCHAR, vCALIFFINAL VARCHAR, vIDCALIFICADOR VARCHAR,
                                vIDPRIORIDAD VARCHAR, vIDIMPACTO VARCHAR, vCOMENTARIOS VARCHAR, vNUMDOC VARCHAR,
                                vIDCONV VARCHAR,  
                                RESULT OUT VARCHAR2  )                                                             
IS     
    
tmpVar NUMBER;
   
BEGIN
    

   
         MERGE INTO PDP.EVALPROY_CIERRE_PROYECTO ECP
                USING ( SELECT 
                            vIDCIERRE vIDCIERRE, vIDPROY vIDPROY, vCALIFFINAL vCALIFFINAL, vIDCALIFICADOR vIDCALIFICADOR, vIDPRIORIDAD vIDPRIORIDAD, 
                            vIDIMPACTO vIDIMPACTO, vCOMENTARIOS vCOMENTARIOS, vNUMDOC vNUMDOC, vIDCONV vIDCONV
                        FROM DUAL ) d
                    ON ( ECP.ID_CIERRE = d.vIDCIERRE )
                    WHEN MATCHED THEN UPDATE SET
                                   ID_PROYECTO              = d.vIDPROY, 
                                   CALIFICACION_FINAL       = d.vCALIFFINAL, 
                                   ID_CALIFICADOR           = d.vIDCALIFICADOR, 
                                   ID_PRIORIDAD             = d.vIDPRIORIDAD, 
                                   ID_IMPACTO               = d.vIDIMPACTO, 
                                   COMENTARIOS              = d.vCOMENTARIOS, 
                                   FECHA_CIERRE             = SYSDATE, 
                                   NUM_DOCUMENTO            = d.vNUMDOC, 
                                   ID_CONVOCATORIA          = d.vIDCONV 
                    WHEN NOT MATCHED THEN
                        INSERT  (
                                   ID_PROYECTO, CALIFICACION_FINAL, 
                                   ID_CALIFICADOR, ID_PRIORIDAD, ID_IMPACTO, 
                                   COMENTARIOS, FECHA_CIERRE, NUM_DOCUMENTO, 
                                   ID_CONVOCATORIA, ID_ESTADO_CIERRE
                                ) 
                         VALUES ( d.vIDPROY, d.vCALIFFINAL, d.vIDCALIFICADOR, d.vIDPRIORIDAD, 
                                          d.vIDIMPACTO, d.vCOMENTARIOS, SYSDATE, d.vNUMDOC, d.vIDCONV, 0 );

    
              
                    RESULT := '1';
           
                  
       
   COMMIT;          
   
   EXCEPTION     
     WHEN OTHERS THEN       
       RESULT:= '-1';        
       RAISE;       
       
END;
/

SHOW ERRORS;


CREATE OR REPLACE procedure     EVALPROY_ABRIRPROYECTO_ADD( vIDCIERRE VARCHAR, 
                                RESULT OUT VARCHAR2  )                                                             
IS     
    
tmpVar NUMBER;
   
BEGIN
    

        DELETE FROM EVALPROY_CIERRE_PROYECTO
        WHERE ID_CIERRE = vIDCIERRE;
              
        RESULT := '1';
           
                  
       
   COMMIT;          
   
   EXCEPTION     
     WHEN OTHERS THEN       
       RESULT:= '-1';        
       RAISE;       
       
END;
/

SHOW ERRORS;


CREATE OR REPLACE procedure     EVALPROY_APROBARPROYECTO_ADD( vIDCIERRE VARCHAR, vIDAPROBADOR VARCHAR,
                                vOBSERVACIONES VARCHAR,
                                RESULT OUT VARCHAR2  )                                                             
IS     
    
tmpVar NUMBER;
   
BEGIN
    

        UPDATE EVALPROY_CIERRE_PROYECTO
            SET ID_ESTADO_CIERRE = 1, ID_APROBADOR = vIDAPROBADOR, FECHA_APROBACION = SYSDATE, OBSERV_APROBACION = vOBSERVACIONES
        WHERE ID_CIERRE = vIDCIERRE;
              
        RESULT := '1';
           
                  
       
   COMMIT;          
   
   EXCEPTION     
     WHEN OTHERS THEN       
       RESULT:= '-1';        
       RAISE;       
       
END;
/

SHOW ERRORS;


CREATE OR REPLACE procedure     EVALPROY_APROBARPROYECTO_DEL( vIDCIERRE VARCHAR, vIDAPROBADOR VARCHAR,                                
                                RESULT OUT VARCHAR2  )                                                             
IS     
    
tmpVar NUMBER;
   
BEGIN   

        

        UPDATE EVALPROY_CIERRE_PROYECTO
            SET ID_ESTADO_CIERRE = 0, ID_APROBADOR = vIDAPROBADOR, FECHA_APROBACION = SYSDATE, OBSERV_APROBACION = NULL
        WHERE ID_CIERRE = vIDCIERRE;
              
        RESULT := '1';
           
                  
       
   COMMIT;          
   
   EXCEPTION     
     WHEN OTHERS THEN       
       RESULT:= '-1';        
       RAISE;       
       
END;
/

SHOW ERRORS;


CREATE OR REPLACE FUNCTION     GETMAIL( INUM_DOC VARCHAR2 ) RETURN VARCHAR IS 
    
    TIPO VARCHAR(20);
    CORREO VARCHAR2(50);
    
BEGIN 

    SELECT GETUSUARIO_JERARQUIA( INUM_DOC ) INTO TIPO
    FROM DUAL;

    
    IF ( TIPO IN ( 'Estudiante') ) THEN     
        SELECT MAX(E.EMAIL) INTO CORREO
        FROM REGIS.ESTUDIANTES E 
        WHERE NUM_DOC = INUM_DOC;    
    END IF;
    
    
    IF ( TIPO IN ( 'Egresado')  ) THEN 
        SELECT MAX(E.EMAIL) INTO CORREO
        FROM REGIS.EGREACAD E 
        WHERE NUM_DOC = INUM_DOC;
    END IF;
    
    IF TIPO IN ( 'Funcionario','Profesor','Administrador' ) THEN 
        SELECT MAX(CHOV_EMAIL) INTO CORREO
        FROM  REGIS.ACRED_PR_GENERALES3
        WHERE TO_CHAR(MHOV_IDENTIFICA) = INUM_DOC;        
    END IF;
    
    
    RETURN CORREO;

END;
/

SHOW ERRORS;


CREATE OR REPLACE FUNCTION GETPUEDE_FECHAS( PROCESO INTEGER ) RETURN INTEGER IS
/*0 si esta entre las fechas
  1 si no   
*/

    PUEDE INTEGER;
    INICIO DATE;
    FINAL DATE;
    ACTUAL DATE;

BEGIN
    

    --Oracle maneja mejor las fechas sueltas q en consulta, x eso la vuelta
    SELECT TO_DATE( FECHA_INICIO,'DD/MM/YYYY') , TO_DATE( FECHA_FINAL, 'DD/MM/YYYY' ), TO_DATE(SYSDATE, 'DD/MM/YYYY')  
           INTO INICIO, FINAL, ACTUAL
    FROM   FECHAS
    WHERE  PROCESO = ID;
    
    IF ( ACTUAL BETWEEN INICIO AND FINAL ) THEN 
        RETURN 0;
    END IF;
    
    RETURN 1;    
    
END;
/

SHOW ERRORS;


CREATE OR REPLACE FUNCTION     EVALPROY_TOTALCALC_CALIFCRIT(vIDPROY NUMBER )  
RETURN NUMBER IS


tmpVar NUMBER;


BEGIN


     SELECT SUM( TOTAL_CRITERIO ) INTO tmpVar
     FROM  EVALPROY_CRITERIOS_PROYECTO_VIEW
     WHERE ID_PROYECTO = vIDPROY;

   RETURN tmpVar;
   
   
   EXCEPTION     
     WHEN OTHERS THEN
       -- Consider logging the error and then re-raise
       RETURN -1;
       
       RAISE;
END;
/

SHOW ERRORS;


CREATE OR REPLACE FUNCTION     EVALPROY_GETTOTALFAV(vIDPROYECTO VARCHAR)  
RETURN NUMBER IS


tmpVar NUMBER;


BEGIN

 
  SELECT COUNT(*) INTO tmpVar 
  FROM PROYECTO_FAVORITOS
  WHERE ID_PROYECTO = vIDPROYECTO AND UPPER(ESTADO) = 'A';
   
  
   RETURN tmpVar;
   
   
   EXCEPTION     
     WHEN OTHERS THEN
       -- Consider logging the error and then re-raise
       RETURN -1;
       
       RAISE;
END EVALPROY_GETTOTALFAV;
/

SHOW ERRORS;


CREATE OR REPLACE FUNCTION     EVALPROY_GETTOTALCRITCALIF(vIDPROYECTO NUMBER)  
RETURN NUMBER IS


tmpVar NUMBER;


BEGIN

 
  SELECT COUNT(*) INTO tmpVar 
  FROM PDP.EVALPROY_CRITERIOS_PROYECTO 
  WHERE ID_PROYECTO = vIDPROYECTO AND ESTADO = 1;
  
   RETURN tmpVar;
   
   
   EXCEPTION     
     WHEN OTHERS THEN
       -- Consider logging the error and then re-raise
       RETURN -1;
       
       RAISE;
END EVALPROY_GETTOTALCRITCALIF;
/

SHOW ERRORS;


CREATE OR REPLACE FUNCTION     EVALPROY_GETTOTALCRITERIOS(vIDPROYECTO NUMBER)  
RETURN NUMBER IS


tmpVar NUMBER;


BEGIN

 
  SELECT COUNT(*) INTO tmpVar 
  FROM PDP.EVALPROY_CRITERIOS
  WHERE ESTADO_CRITERIO = 1;
  
   RETURN tmpVar;
   
   
   EXCEPTION     
     WHEN OTHERS THEN
       -- Consider logging the error and then re-raise
       RETURN -1;
       
       RAISE;
END;
/

SHOW ERRORS;


CREATE OR REPLACE FUNCTION     EVALPROY_GETCIERRE(vIDPROYECTO NUMBER, vIDCONV NUMBER )  
RETURN NUMBER IS


tmpVar NUMBER;


BEGIN

 IF vIDCONV IS NOT  NULL THEN
      
      SELECT COUNT(*) INTO tmpVar 
      FROM EVALPROY_CIERRE_PROYECTO
      WHERE ID_PROYECTO = vIDPROYECTO AND ID_CONVOCATORIA = vIDCONV;
  
 ELSE 
 
      SELECT COUNT(*) INTO tmpVar 
      FROM EVALPROY_CIERRE_PROYECTO
      WHERE ID_PROYECTO = vIDPROYECTO;
 
 END IF;
 
 
   RETURN tmpVar;
   
   
   EXCEPTION     
     WHEN OTHERS THEN
       -- Consider logging the error and then re-raise
       RETURN -1;
       
       RAISE;
END;
/

SHOW ERRORS;


CREATE OR REPLACE FUNCTION GET_CONVOCATORIA_ACTIVA RETURN INTEGER IS

    CONVOCATORIA INTEGER;
    
BEGIN

    SELECT  MAX( ID_CONVOCATORIA )  INTO CONVOCATORIA
    FROM EVALPROY_CONVOCATORIAS
    WHERE  ESTADO_CONVOCATORIA = 1;
    
    RETURN CONVOCATORIA;

END ;
/

SHOW ERRORS;


CREATE OR REPLACE FUNCTION     EVALPROY_TOTAL_PROYCERRADOS(vIDCONV NUMBER )  
RETURN NUMBER IS


tmpVar NUMBER;


BEGIN

 IF vIDCONV IS NOT  NULL THEN
      
      SELECT COUNT(*) INTO tmpVar 
      FROM EVALPROY_CIERRE_PROYECTO
      WHERE ID_CONVOCATORIA = vIDCONV AND NVL(ID_ESTADO_CIERRE,0) = 0;
  
 ELSE 
 
      SELECT COUNT(*) INTO tmpVar 
      FROM EVALPROY_CIERRE_PROYECTO
      WHERE NVL(ID_ESTADO_CIERRE,0) = 0;
      
 
 END IF;
 
 
   RETURN tmpVar;
   
   
   EXCEPTION     
     WHEN OTHERS THEN
       -- Consider logging the error and then re-raise
       RETURN -1;
       
       RAISE;
END;
/

SHOW ERRORS;


CREATE OR REPLACE FUNCTION     EVALPROY_TOTAL_PROYPENDIENTES(vIDCONV NUMBER )  
RETURN NUMBER IS


tmpVar NUMBER;


BEGIN


     IF vIDCONV IS NOT NULL THEN
          
          SELECT COUNT(P.ID_PROYECTO) INTO tmpVar 
          FROM PROYECTO P
          WHERE P.ESTADO = 1 AND P.ID_CONVOCATORIA = vIDCONV AND P.ID_PROYECTO NOT IN  
          (  
              SELECT  ECP.ID_PROYECTO
              FROM EVALPROY_CIERRE_PROYECTO ECP
              WHERE ECP.ID_CONVOCATORIA = vIDCONV
          );
     
     ELSE 
     
          SELECT COUNT(P.ID_PROYECTO) INTO tmpVar 
          FROM PROYECTO P
          WHERE P.ESTADO = 1 AND P.ID_PROYECTO NOT IN  
          (  
              SELECT  ECP.ID_PROYECTO
              FROM EVALPROY_CIERRE_PROYECTO ECP
          );
          
     
     END IF;
 
 
   RETURN tmpVar;
   
   
   EXCEPTION     
     WHEN OTHERS THEN
       -- Consider logging the error and then re-raise
       RETURN -1;
       
       RAISE;
END;
/

SHOW ERRORS;


CREATE OR REPLACE FUNCTION GETFAVORITOS_USUARIO( USUARIO VARCHAR2, FACTOR INTEGER   ) RETURN INTEGER IS
    
    TOTAL INTEGER;
    
BEGIN 
    
    SELECT COUNT(PR.ID_PROYECTO) INTO TOTAL
    FROM   PROYECTO_FAVORITOS PF 
    INNER JOIN PROYECTO PR ON PR.ID_PROYECTO = PF.ID_PROYECTO
    WHERE  PF.USUARIOVOTO = USUARIO AND PR.ID_FACTOR = FACTOR AND PF.ESTADO = 'A' ;    
    
    
    RETURN TOTAL;
    
END;
/

SHOW ERRORS;


CREATE OR REPLACE FUNCTION     EVALPROY_GETESTADOPROYECTO(vIDPROYECTO NUMBER, vIDCONV NUMBER )  
RETURN VARCHAR IS


tmpVar NUMBER;
tmpEstado NUMBER;


BEGIN

 
      
      SELECT COUNT(*) INTO tmpVar 
      FROM EVALPROY_CIERRE_PROYECTO
      WHERE ID_PROYECTO = vIDPROYECTO AND ID_CONVOCATORIA = vIDCONV;
      
      IF tmpVar > 0 THEN
      
            SELECT NVL(ID_ESTADO_CIERRE,0) INTO tmpEstado 
            FROM EVALPROY_CIERRE_PROYECTO
            WHERE ID_PROYECTO = vIDPROYECTO AND ID_CONVOCATORIA = vIDCONV;
            
            IF tmpEstado = 1 THEN
            
                RETURN 'Proyecto Seleccionado';
                
            ELSE
            
                RETURN 'Proyecto con Calificación Cerrada';
            
            END IF;
        
      ELSE
      
            
            SELECT NVL(ESTADO,0) INTO tmpEstado 
            FROM PDP.PROYECTO
            WHERE ID_PROYECTO = vIDPROYECTO AND ID_CONVOCATORIA = vIDCONV;
            
            
            IF tmpEstado = 1 THEN
            
                RETURN 'Proyecto Pendiente por Calificar';
                
            ELSE      
            
                RETURN 'Proyecto NO Calificable';
      
            END IF;
            
      
      END IF;
 
 
   RETURN tmpVar;
   
   
   EXCEPTION     
     WHEN OTHERS THEN
       -- Consider logging the error and then re-raise
       RETURN 'No Identificado';
       
       RAISE;
END;
/

SHOW ERRORS;


CREATE OR REPLACE FUNCTION     EVALPROY_TOTAL_PROYAPROBADOS(vIDCONV NUMBER )  
RETURN NUMBER IS


tmpVar NUMBER;


BEGIN

 IF vIDCONV IS NOT  NULL THEN
      
      SELECT COUNT(*) INTO tmpVar 
      FROM EVALPROY_CIERRE_PROYECTO
      WHERE ID_CONVOCATORIA = vIDCONV AND NVL(ID_ESTADO_CIERRE,0) = 1;
  
 ELSE 
 
      SELECT COUNT(*) INTO tmpVar 
      FROM EVALPROY_CIERRE_PROYECTO
      WHERE NVL(ID_ESTADO_CIERRE,0) = 1;
      
 
 END IF;
 
 
   RETURN tmpVar;
   
   
   EXCEPTION     
     WHEN OTHERS THEN
       -- Consider logging the error and then re-raise
       RETURN -1;
       
       RAISE;
END;
/

SHOW ERRORS;


CREATE OR REPLACE FUNCTION GETUSUARIO_JERARQUIA( IUSER VARCHAR2  )  RETURN VARCHAR2 IS

    CONT_ADMON INTEGER;
    CONT_PROFESOR INTEGER;
    CONT_ESTUDIANTE INTEGER;
    CONT_EGRESADO INTEGER;
    CONT_FUNCIONARIO INTEGER;
    
BEGIN
    
    
    SELECT NVL( SUM ( CASE TIPO
                         WHEN 'Administrativo' THEN 1 
                      END),0 ),
           NVL( SUM ( CASE TIPO
                         WHEN 'Egresado' THEN 1 
                      END),0 ),
           NVL( SUM ( CASE TIPO
                         WHEN 'Estudiante' THEN 1 
                      END),0 ),
           NVL( SUM ( CASE TIPO
                         WHEN 'Profesor' THEN 1 
                      END),0 ),      
           NVL( SUM ( CASE TIPO
                         WHEN 'Administrador' THEN 1 
                      END),0)
           INTO  CONT_FUNCIONARIO, CONT_EGRESADO, CONT_ESTUDIANTE, CONT_PROFESOR, CONT_ADMON                                        
    FROM   ( SELECT DISTINCT CASE TIPO_USUARIO
                         WHEN 'Administrativo' THEN TIPO_USUARIO
                         WHEN 'Egresado'  THEN TIPO_USUARIO
                         WHEN 'Estudiante' THEN TIPO_USUARIO
                         WHEN 'Profesor' THEN TIPO_USUARIO
                         ELSE 'Administrador'
                    END TIPO,USUARIO     
              FROM   USUARIO ) u
    WHERE  U.USUARIO = IUSER;
    
    
    IF ( CONT_ADMON > 0 ) THEN
        RETURN 'Administrador';
    END If;

    IF ( CONT_PROFESOR > 0 ) THEN
        RETURN 'Profesor';
    END IF;    
    
    IF ( CONT_ESTUDIANTE > 0 ) THEN
        RETURN 'Estudiante';
    END IF;     
    
    IF ( CONT_FUNCIONARIO > 0 ) THEN
        RETURN 'Funcionario';
    END IF;  

    RETURN 'Egresado';
     
END;
/

SHOW ERRORS;


CREATE OR REPLACE FUNCTION GET_CEDULAPROYECTO( ID INTEGER ) RETURN VARCHAR2 IS

    USERP VARCHAR2(50);
    NUM_DOC VARCHAR2(50);
    
    CONTADMON INTEGER;

BEGIN 

    SELECT P.RESPONSABLE INTO USERP
    FROM PROYECTO P
    WHERE  P.ID_PROYECTO = ID;
    
    SELECT COUNT( * ) INTO CONTADMON 
    FROM   USUARIOS_ADMON 
    WHERE  USUARIO = USERP;
    
    IF (  CONTADMON > 0 ) THEN 
        RETURN USERP;
    END IF;
    
    
    SELECT DOCUMENTO INTO NUM_DOC
    FROM REGIS.LOGIN_USUARIO 
    WHERE USUARIO = USERP;
    
    
    RETURN NUM_DOC;
    
END;
/

SHOW ERRORS;


