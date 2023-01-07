using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Conexion
{
    public class Conexion
    {
        private readonly string url;

        public Conexion()
        {
            url = "Data Source=database.db; Version = 3; New = True; Compress = True; ";
        }

        protected SQLiteConnection getConexion()
        {
            SQLiteConnection connection;
            connection = new SQLiteConnection(url);

            try
            {
                connection.Open();//Abre la conexión a la base de datos
            }
            catch (Exception ex)//verifica si ocurre un error al abrir la conexión
            {
                MessageBox.Show("Parece que ocurrió un error al conectarnos a la base de datos: " + ex, "Error 410", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return connection;//retorna la conexión
        }
    }
}
