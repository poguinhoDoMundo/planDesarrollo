using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using pln_v2.Models;
using System.Web.Http.Cors;
using System.IO;

namespace pln_v2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Factores()
        {
           if (Session["user"] == null)
                return RedirectToAction("Login");

            FACTOR factor = new FACTOR();
            List<FACTOR> factores = factor.getFactores();

            return View( factores );
        }

        public ActionResult Proyectos( int id )
        {
            if (Session["user"] == null)
                return RedirectToAction("Login");

            PROYECTO proyecto = new PROYECTO();
            List<PROYECTO> proyectos = proyecto.getProyectos(id) ;
            
            return View(proyectos);
        } 
        
        public ActionResult Head()
        {
            return PartialView();
        }

        public ActionResult NavBar()
        {
            if (Session["user"] == null)
                return RedirectToAction("Login");

            ViewBag.TipoUser = Convert.ToString(Session["tipo"]);

            return PartialView();
        }


        public ActionResult ProyectoDetalle( int id, int viene)
        {
            if (Session["user"] == null)
                return RedirectToAction("Login");

            ViewBag.refer = viene;

            PROYECTO proyecto = new PROYECTO();
            proyecto = proyecto.getProyecto(id);

            return View( proyecto );
        }


        public ActionResult pvFavoritos(int id )
        {
            if (Session["user"] == null)
                return RedirectToAction("Login");

            string usuario = Convert.ToString(  Session["user"] );

            ViewBag.isSave = PROYECTO.isSalvado(id, usuario);
            ViewBag.isFavorito = PROYECTO.isFavorito(id, usuario);
            ViewBag.idProyecto = id;

            return PartialView();
        }

        public JsonResult addFavorito( int id  )
        {
            string usuario= Convert.ToString(Session["user"]);
            string result = PROYECTO.marcaFavorito(usuario, id.ToString());
            
            return Json(result);
        }


        public JsonResult addCarro( int id )
        {
            string usuario = Convert.ToString(Session["user"]);
            string result = PROYECTO.marcaCarro(usuario, id.ToString());
            
            return Json(result);
        }

        public JsonResult isFavorito( int id )
        {
            string usuario = Convert.ToString(Session["user"]);

            return Json(PROYECTO.isFavorito(id, usuario));
        }

        public JsonResult isSave( int id )
        {
            string usuario = Convert.ToString(Session["user"]);

            return Json( PROYECTO.isSalvado(id, usuario ) );
        }

        public ActionResult Propuesta()
        {
            if (Session["user"] == null)
                return RedirectToAction("Login");

            FACTOR factor = new FACTOR();
            List<FACTOR> factores = factor.getFactores();
            ViewBag.factores = factores;

            PLAZO plazo = new PLAZO();
            List<PLAZO> plazos = plazo.getPrioridades();
            ViewBag.prioridades = plazos;
                
            return View( new PROYECTO() );
        }

        [HttpPost]
        public JsonResult addPropuesta( PROYECTO model )
        {
            string usuario = Convert.ToString(Session["user"]);
            string result = "No se pudo registrar el proyecto, inicie sesion de nuevo e intentelo nuevamente !!!";

          
            if (ModelState.IsValid)
            {
                result = model.addProyecto(usuario, Convert.ToString(Session["archivo"]) );                
            }
            
            return Json(result );
                        
        }

        public ActionResult resultOK( int id )
        {
            ViewBag.mensaje = id;
            Session["archivo"] = null;
            return View();
        }

        public ActionResult MisProyectos( int id )
        {
            if (Session["user"] == null)
                return RedirectToAction("Login");

            string usuario = Convert.ToString( Session["user"] );

            ViewBag.tipo = id;

            PROYECTO proyecto = new PROYECTO();
            List<MisProyecto> proyectos = proyecto.getMisPropuestas(usuario, id);
            
            return View(proyectos);
        }

        public ActionResult SearchProyectos( string id )
        {
            PROYECTO proyecto = new PROYECTO();
            List<PROYECTO> proyectos = proyecto.getSearch(id);
            

            return View(proyectos);
        }

        public ActionResult About()
        {
            return View();
        }


        public  ActionResult Login( string user, string pass )
        {
            if (user != null && pass != null)
            {
                USUARIO usuario = new USUARIO();
                bool result = usuario.isUser(user, pass);

                if (result)
                {
                    Session["user"] = user;
                    Session["tipo"] = usuario.getTipoUsuario(user);

                    return RedirectToAction("Factores");/*ojo, cambiar*/
                }
                else 
                    ViewBag.isValid = "El usuario no se encuentra activo en la base de datos del SIA";
            }

            Session["user"] = null;
            Session["tipo"] = null;
            Session["archivo"] = null;

            return View();
        }
        
        public ActionResult Revision()
        {
            if (Session["user"] == null)
                return RedirectToAction("Login");
            if ( Convert.ToString( Session["tipo"]) != "Administrador")
                return RedirectToAction("Factores");

            PROYECTO proyecto = new PROYECTO();
            List<MisProyecto> proyectos = proyecto.getRevision();

            return View(proyectos);
        }

        public ActionResult RevisionDetalle(int id)
        {
            if (Session["user"] == null)
                return RedirectToAction("Login");
            if (Convert.ToString(Session["tipo"]) != "Administrador")
                return RedirectToAction("Factores");

            PROYECTO proyecto = new PROYECTO();
            proyecto = proyecto.getProyecto(id);

            return View(proyecto);            
        }

        public JsonResult aprobarWeb( int id, int concepto)
        {
            string usuario = Convert.ToString(Session["user"]);
            string result = PROYECTO.aprProyecto(id, concepto, usuario);

            if (result == "OK")
                armarEmail(concepto, USUARIO.getDocumentoProyecto( Convert.ToString( id)  ), id );

            return Json( result );
        }


        public void armarEmail(int concepto, string user, int proyecto )
        {
            string asunto = "Universidad de Caldas, plan de desarrollo participativo";
            string body = "Estimado " + USUARIO.getNombre(user) +  " : \n\n";

            if (concepto == 1)
                body += "Su propuesta '" + PROYECTO.getNombreProyecto(proyecto) + "' fue aceptada y publicada en la web. "
                      + " Le agradecemos por su valioso aporte y recuerde que, con cada idea postulada, está colaborando con el mejoramiento, crecimiento y fortalecimiento de la Universidad de Caldas.";
            else
                body += "Sentimos informarle que su propuesta '" + PROYECTO.getNombreProyecto(proyecto) + "' no fue aprobada, "
                      + " pero le invitamos a que siga participando compartiendo sus ideas para contribuir con el mejoramiento, crecimiento y fortalecimiento de la Universidad de Caldas.";

            string mail = USUARIO.getMail(user);
            string message = EnviarEmail("notificaciones.siaucaldas@gmail.com", mail, asunto, body);
        }


        public string EnviarEmail(string remitente, string destinatario, string asunto, string cuerpo)
        {
            if ( String.IsNullOrEmpty(destinatario)  )
                return null;

            if (!destinatario.Contains("@") )
                return null;

            System.Net.Mail.MailMessage correo = new System.Net.Mail.MailMessage();

            correo.From = new System.Net.Mail.MailAddress(remitente, "UNIVERSIDAD DE CALDAS");
            correo.To.Add(destinatario);
            correo.Subject = asunto;
            correo.Body = cuerpo;
            correo.IsBodyHtml = false;
            correo.Priority = System.Net.Mail.MailPriority.Normal;

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
            smtp.Credentials = new System.Net.NetworkCredential("notificaciones.siaucaldas@gmail.com", "regacad2012");
            smtp.Port = 587;
            //smtp.Port = 465;
            smtp.Host = "smtp.googlemail.com";
            smtp.EnableSsl = true;

            try
            {
                smtp.Send(correo);
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /*********** ultima ***********************************/
        public ActionResult cargaArchivo()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult cargaArchivo(HttpPostedFileBase file)
        {
            bool bandera = true;

            try
            {
                if (file == null)
                {
                    bandera = false;
                    ViewBag.FileStatus = "Se necesita un archivo adjunto para continuar";
                }

                if (!Path.GetFileName(file.FileName).ToLower().Contains(".pdf"))
                {
                    bandera = false;
                    ViewBag.FileStatus = "El archivo debe ser pdf no superior a 2 megas";
                }

                if (bandera)
                {
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
                    var path = Path.Combine(Server.MapPath("~/docs/"), fileName);

                    Session["archivo"] = "/docs/" + fileName; ;
                    file.SaveAs(path);
                }

            }
            catch (Exception ex)
            {
                ViewBag.FileStatus = "No se pudo cargar el archivo." + ex.Message;
            }

            return PartialView("cargaArchivo");
        }

        public ActionResult DeleteFactura(string hdArchivo)
        {
            /*
            string fullPath = Request.MapPath("~/documentos/" + delete );
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }*/

            Session["archivo"] = null;
            return RedirectToAction("cargaArchivo");
        }


        /******************************************************/
    }
}