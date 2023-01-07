using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soporte.Cache
{
    //Es "Static" ya que permite que los datos se mantengan en memoria mientras la aplicación esté abierta, así permite acceder a ellos en cualquier momento
    public static class UserLoginCache//Caché de inicio de sesión de usuario
    {
        public static int IdUsuario { get; set; }
        public static string Nombres { get; set;}
        public static string Apellidos { get; set;}
        public static string Cargo { get; set; }
        public static string Email { get; set;}
        public static string Sexo { get; set;}
    }
}
