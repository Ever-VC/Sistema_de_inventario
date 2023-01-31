using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace Persistencia.ServiciosMail
{
    /// <summary>
    /// ESTANDARES DE SERVICIOS DE CORREO ELECTRONICO....
    /// 
    /// SMTP: Envía correos desde un cliete de correo (gmail, hotmail,... etc) a un servidor de correo.
    /// 
    /// POP (V3) /IMAP (V4): Se encargan de recuperar mensajes de correo electrónico de un mail server a un cliente de correo.
    /// </summary>
    public abstract class EmailServer
    {
        //Atributos
        private SmtpClient smtpClient;
        protected string emailRemitente { get; set; }
        protected string password { get; set; }
        protected string host { get; set; }
        protected int puerto { get; set; }
        protected bool ssl { get; set; }

        protected void inicializarClienteSmtp()
        {
            //Configuración básica para un cliente de protocolo simple de transferencia de correo
            smtpClient = new SmtpClient
            {
                Credentials = new NetworkCredential(emailRemitente, password),
                Host = host,
                Port = puerto,
                EnableSsl = ssl
            };
        }
        //El parámtro "destinatarios" es de tipo lista en caso que se desee enviar un correo a múltiples emails a la vez.
        public void enviarCorreo(string asunto, string cuerpo, List<string> destinatarios)
        {
            var mensajeCorreo = new MailMessage();
            try
            {
                //Estructura del mensaje
                mensajeCorreo.From = new MailAddress(emailRemitente);
                //Se recorren todos los destinatarios a los que se enviará el mensaje
                foreach (string destinatario in destinatarios)
                {
                    mensajeCorreo.To.Add(destinatario);
                }
                //Se indica el asunto, cuerpo y la prioridad del correo.
                mensajeCorreo.Subject = asunto;
                mensajeCorreo.Body = cuerpo;
                mensajeCorreo.Priority = MailPriority.Normal;

                //Envia el mensaje mendiante el protocolo simple de correo
                smtpClient.Send(mensajeCorreo);
            }
            catch (Exception){ }
            finally
            {
                //Se desechan los objetos creados para liberar los recursos.
                mensajeCorreo?.Dispose();
                smtpClient.Dispose();
            }
        }

    }
}
