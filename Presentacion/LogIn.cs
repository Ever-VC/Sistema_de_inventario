using Presentacion.Botones;
using Presentacion.Inicios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Dominio.Modelos;
using Persistencia.Modelos;
using Soporte.Cache;

namespace Presentacion
{
    public partial class LogIn : Form
    {
        private readonly BtnsPnlSuperior btn = new();

        public LogIn()
        {
            InitializeComponent();
        }

        #region Enter & Leave txt
        //Efecto "Placeholder"
        private void txtUser_Enter(object sender, EventArgs e)//Se ejecuta cuando el usuario le da click al cuadro de texto (para insertar texto)
        {
            if (txtUser.Text == "USUARIO")
            {//Si el texto del cadro es "USUARIO"
                txtUser.Text = "";//Limpia el texto
                txtUser.ForeColor = Color.LightGray;//Da un color gris claro a lo que el usuario digite
            }
        }

        private void txtUser_Leave(object sender, EventArgs e)//Se ejecuta cuando el usuario sale del cuadro de texto (Da click en otro lugar)
        {
            if (txtUser.Text == "")
            {//Si el usuario no insertó ningún caracter
                txtUser.Text = "USUARIO";//Se asigna "USUARIO" a la propiedad de texto
                txtUser.ForeColor = Color.DimGray;//Cambia el color de texto al color inicial
            }
        }

        private void txtPassword_Enter(object sender, EventArgs e)//Se ejecuta cuando el usuario le da click al cuadro de texto (para insertar texto)
        {
            if (txtPassword.Text == "CONTRASEÑA")
            {//Si el texto del cadro es "CONTRASEÑA"
                txtPassword.Text = "";//Limpia el texto
                txtPassword.ForeColor = Color.LightGray;//Da un color gris claro a lo que el usuario digite
                txtPassword.UseSystemPasswordChar = true;//Asigna el formato de contraseña al texto que inserte el usuario
            }
        }

        private void txtPassword_Leave(object sender, EventArgs e)//Se ejecuta cuando el usuario sale del cuadro de texto (Da click en otro lugar)
        {
            if (txtPassword.Text == "")
            {//Si el usuario no insertó ningún caracter
                txtPassword.Text = "CONTRASEÑA";//Se asigna "CONTRASEÑA" a la propiedad de texto
                txtPassword.ForeColor = Color.DimGray;//Cambia el color de texto al color inicial
                txtPassword.UseSystemPasswordChar = false;//Indica que NO es un formato de contraseña
            }
        }
        #endregion

        #region Mover el Formulario
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);

        private void LogIn_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        #endregion

        #region Cerrar & Minimizar
        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            btn.Minimizar(this);
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            btn.Cerrar();
        }
        #endregion

        #region Acceder_Salir
        private void btnAcceder_Click(object sender, EventArgs e)
        {
            if (txtUser.Text != "USUARIO")//Valida que se haya insertado un nombre de usuario
            {
                if (txtPassword.Text != "CONTRASEÑA")//Valida que se haya insertado una contraseña
                {
                    ModeloEmpleado usuario = new();//Se accede a la capa de Dominio y se crea un objeto de la clase "ModeloEmpleado"
                    //A través del objeto de la capa de dominio se verifica si existe el usuario que desea acceder al programa
                    if (usuario.LoingEmpleado(txtUser.Text, txtPassword.Text))//Verifica si es DIFERENTE de nulo (Si es así, significa que el ususario fue encontrado)
                    {
                        //Almacena el índice en donde se encuentra el primer espacio en blanco(Ya que solo quiero el primer nombre)
                        int indice1 = UserLoginCache.Nombres.IndexOf(' ');
                        //Almacena el índice en donde se encuentra el primer espacio en blanco (Ya que solo quiero el primer apellido)
                        int indice2 = UserLoginCache.Apellidos.IndexOf(' ');
                        string Nombre = UserLoginCache.Nombres.Remove(indice1);//Elimina todo lo que se encuentre a partir del espacio en blanco.
                        string Apellido = UserLoginCache.Apellidos.Remove(indice2);
                        if (UserLoginCache.Sexo == "Femenino")
                        {
                            MessageBox.Show("¡BIENVENIDA " + Nombre.ToUpper() + " " + Apellido.ToUpper() + "! ES UN GUSTO VERTE POR ACÁ DE NUEVO :D", "Acceso confirmado.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("¡BIENVENIDO " + Nombre.ToUpper() + " " + Apellido.ToUpper() + "! ES UN GUSTO VERTE POR ACÁ DE NUEVO :D", "Acceso confirmado.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        MenuPrincipal admin = new();
                        admin.Show();
                        //Se ejecuta el método de "Cerrar sesión" al cerrar el formulario principal, por lo cual lo sobrecargamos.
                        admin.FormClosed += Salir;
                        this.Hide();

                    }
                    else
                    {
                        //En caso que el objeto de la clase empleado sea nulo (lo cual significa que el usuario no existe o están mal escritos los datos)
                        MessageBox.Show("NOMBRE DE USUARIO O CONTRASEÑA INCORRECTO.\nPOR FAVOR INTENTELO NUEVAMENTE :D");
                        txtPassword.Text = "";
                        txtPassword.UseSystemPasswordChar = false;
                        txtPassword.Focus();
                    }
                }
                else MensajeError("POR FAVOR INGRESE SU CONTRASEÑA.");
            }
            else MensajeError("POR FAVOR INGRESE SU NOMBRE DE USUARIO.");
        }

        private void MensajeError(string msg)
        {
            //Recibe el mensaje de errror que se desea mostrar el la etiqueta
            lblError.Text = "     " + msg;
            lblError.Visible = true;
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))//Verifica si la tecla precionada es "Enter"
            {
                btnAcceder_Click(sender, e);//Si es "Enter" ejecuta la función Acceder
            }
        }

        private void Salir(Object sender, FormClosedEventArgs e)
        {
            txtPassword.Text = "CONTRASEÑA";
            txtPassword.UseSystemPasswordChar = false;
            txtUser.Text = "USUARIO";
            lblError.Visible = false;
            this.Show();
        }
        #endregion
    }
}
