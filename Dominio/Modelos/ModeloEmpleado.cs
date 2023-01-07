using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Persistencia;
using Persistencia.Controladores;
using Persistencia.Modelos;
using Soporte.Cache;

namespace Dominio.Modelos
{
    public class ModeloEmpleado
    {
        public bool LoingEmpleado(string usuario, string password)
        {
            //Hace el llamado a la función  "Login" (lA CUAL ES BOOLEANA) de la capa de persistencia (Controladores.EmpleadoController)
            //y retorna el valor de RETORNO de esa función.
            return EmpleadoController.Instancia.Login(usuario, password);
            //La función "Login" retorna un valor booleano, ese mismo valor retornará ésta función "LoingEmpleado".
        }
    }
}
