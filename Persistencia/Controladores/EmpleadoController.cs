using Dominio.Conexion;
using Persistencia.Modelos;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soporte.Cache;
using System.Data;

namespace Persistencia.Controladores
{
    public class EmpleadoController : Conexion
    {
        //Aplicando patrón de diseño Singleton.
        private static EmpleadoController _intancia = null;

        public static EmpleadoController Instancia
        {
            get
            {
                if (_intancia == null)//Verifica sino existe ninguna instancia al objeto.
                {
                    _intancia = new EmpleadoController();//Sino existe ninguna instancia, la crea.
                }
                return _intancia;//Retorna la instancia.
            }
        }

        public bool Login(string usuario, string pass)
        {
            using (SQLiteConnection connection = getConexion())
            {
                //Consulta con parámetros
                string query = "SELECT * FROM Empleado WHERE usuario = @Usuario and password = @Password";
                SQLiteCommand cmd = new(query, connection);//Creamos el comando SQLite

                //Asignamos los valores a los parámetros de la consulta
                cmd.Parameters.Add(new SQLiteParameter("@Usuario", usuario));
                cmd.Parameters.Add(new SQLiteParameter("@Password", pass));

                cmd.CommandType = System.Data.CommandType.Text;//Indica el tipo del comando
                using SQLiteDataReader dataReader = cmd.ExecuteReader();//El using permite ejecutar un bloque de codigo y al pasar, simplemente lo olvida (No es necesario cerrar instancias)
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())//Se leen todos los registros
                    {
                        UserLoginCache.IdUsuario = dataReader.GetInt32(0);//El "id" del empleado es el atributo numero 0 en la tabla
                        UserLoginCache.Nombres = dataReader.GetString(2);
                        UserLoginCache.Apellidos = dataReader.GetString(3);
                        UserLoginCache.Cargo = dataReader.GetString(6);
                        UserLoginCache.Email = dataReader.GetString(7);
                        UserLoginCache.Sexo = dataReader.GetString(8);
                        //nombre = dataReader["nombres"].ToString();
                        //cargo = dataReader["cargo"].ToString();

                    }
                    return true;
                }
                else
                {
                    return false;
                }
                /*
                if (nombre != "" && cargo != "")
                {
                    empleado = new()
                    {
                        Cargo = cargo
                    };
                    MessageBox.Show("¡BIENVENIDO " + nombre.ToUpper() + "!, ES UN GUSTO VERTE POR ACÁ DE NUEVO :D", "Acceso confirmado.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                */
            }                
        }
        /*
        public bool Insertar(Empleado empleado)
        {
            bool respuesta = true;//Permite verificar si se pudo insertar el registro (En caso que no se inserte cambiará a falso, de lo contrario va a retornar verdadero)

            using (SQLiteConnection connection = getConexion())//Se utiliza el using para no tener que cerrar isntancias, ya que al salir, ignora todo
            {
                //Consulta con parámetros
                string query = "INSERT INTO Empleado(nombre, apellido, correo, password, username) VALUES (@Nombre, @Apellido, @Correo, @Password, @Username)";

                SQLiteCommand cmd = new(query, connection);//Creamos el comando SQLite

                //Asignamos los valores a los parámetros de la consulta
                cmd.Parameters.Add(new SQLiteParameter("@Nombre", empleado.Nombres));
                cmd.Parameters.Add(new SQLiteParameter("@Apellido", empleado.Apellidos));
                cmd.Parameters.Add(new SQLiteParameter("@Correo", empleado.Email));
                cmd.Parameters.Add(new SQLiteParameter("@Password", empleado.Password));
                cmd.Parameters.Add(new SQLiteParameter("@Username", empleado.Usuario));

                cmd.CommandType = System.Data.CommandType.Text;//Indica el tipo del comando

                if (cmd.ExecuteNonQuery() < 1)//Verifica si la cantida de registros afectados es menor que 1 (Significa que no hubo ninguna insersión)
                {
                    respuesta = false;//Retorna falso en caso que no se haya guradado el registro en la db
                }
            }

            return respuesta;
        }

        public bool Editar(Empleado empleado)
        {
            bool respuesta = true;//Permite verificar si se pudo insertar el registro (En caso que no se inserte cambiará a falso, de lo contrario va a retornar verdadero)

            using (SQLiteConnection connection = getConexion())//Se utiliza el using para no tener que cerrar isntancias, ya que al salir, ignora todo
            {
                //Consulta con parámetros
                string query = "UPDATE Empleado set nombre = @Nombre, apellido = @Apellido, correo = @Correo, password = @Password, username = @Username WHERE id = @Id";
                /*
                SQLiteCommand cmd = new(query, connection);//Creamos el comando SQLite

                //Asignamos los valores a los parámetros de la consulta
                cmd.Parameters.Add(new SQLiteParameter("@Id", empleado.Id));
                cmd.Parameters.Add(new SQLiteParameter("@Nombre", empleado.Nombre));
                cmd.Parameters.Add(new SQLiteParameter("@Apellido", empleado.Apellido));
                cmd.Parameters.Add(new SQLiteParameter("@Correo", empleado.Correo));
                cmd.Parameters.Add(new SQLiteParameter("@Password", empleado.Password));
                cmd.Parameters.Add(new SQLiteParameter("@Username", empleado.Usuario));

                cmd.CommandType = System.Data.CommandType.Text;//Indica el tipo del comando

                if (cmd.ExecuteNonQuery() < 1)//Verifica si la cantida de registros afectados es menor que 1 (Significa que no hubo ninguna insersión)
                {
                    respuesta = false;//Retorna falso en caso que no se haya guradado el registro en la db
                }
                
            }

            return respuesta;
        }

        public bool Eliminar(Empleado empleado)
        {
            bool respuesta = true;//Permite verificar si se pudo insertar el registro (En caso que no se inserte cambiará a falso, de lo contrario va a retornar verdadero)
            /*
            using (SQLiteConnection connection = Conexion_db.Instancia.getConexion())//Se utiliza el using para no tener que cerrar isntancias, ya que al salir, ignora todo
            {
                //Consulta con parámetros
                string query = "DELETE FROM Empleado WHERE id = @Id";

                SQLiteCommand cmd = new(query, connection);//Creamos el comando SQLite

                //Asignamos el valor al parámetro id (Es el único que se necesita para eliminar)
                cmd.Parameters.Add(new SQLiteParameter("@Id", empleado.Id));

                cmd.CommandType = System.Data.CommandType.Text;//Indica el tipo del comando

                if (cmd.ExecuteNonQuery() < 1)//Verifica si la cantida de registros afectados es menor que 1 (Significa que no hubo ninguna insersión)
                {
                    respuesta = false;//Retorna falso en caso que no se haya guradado el registro en la db
                }
            }
            

            return respuesta;
        }

        public List<Empleado> getListaEmpleados()
        {

            List<Empleado> listaEmpleados = new List<Empleado>();
            /* 
            using (SQLiteConnection connection = Conexion.Instancia.getConexion())
            {
                string query = "SELECT * FROM Empleado";//Creación la consulta

                SQLiteCommand cmd = new(query, connection)
                {
                    CommandType = System.Data.CommandType.Text//Indica el tipo del comando
                };//Crea el comando sqlite

                using SQLiteDataReader dataReader = cmd.ExecuteReader();//El using permite ejecutar un bloque de codigo y al pasar, simplemente lo olvida (No es necesario cerrar instancias)
                while (dataReader.Read())//Se leen todos los registros
                {
                    listaEmpleados.Add(new Empleado()//Crea un nuevo empleado y almacena cada uno de los atributos
                    {
                        Id = int.Parse(dataReader["id"].ToString()),
                        Nombre = dataReader["nombre"].ToString(),
                        Apellido = dataReader["apellido"].ToString(),
                        Correo = dataReader["correo"].ToString(),
                        Password = dataReader["password"].ToString(),
                        Usuario = dataReader["username"].ToString()
                    });
                }
            }
            
            return listaEmpleados;
        }
        */
    }
}
