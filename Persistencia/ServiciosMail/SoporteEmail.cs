using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia.ServiciosMail
{
    public class SoporteEmail : EmailServer
    {
        public SoporteEmail()
        {
            emailRemitente = "soporteLlanteriaElBuffalo@gmail.com";
            password = "admin4321";
            host = "smtp.gmail.com";
            puerto = 587;
            ssl = true;
            inicializarClienteSmtp();
        }
    }
}
