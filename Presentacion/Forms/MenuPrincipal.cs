using Presentacion.Botones;
using Soporte.Cache;
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

namespace Presentacion.Inicios
{
    public partial class MenuPrincipal : Form
    {
        private readonly BtnsPnlSuperior btn = new();

        public MenuPrincipal()
        {
            InitializeComponent();
            //Estas lineas eliminan los parpadeos del formulario o controles en la interfaz grafica (Pero no en un 100%)
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.DoubleBuffered = true;
        }

        #region Cargar_Datos
        private void MenuAdmin_Load(object sender, EventArgs e)//Evento "load" del formulario "MenuPrincipal"
        {
            //Permite que al iniciar el programa, se muestre en toda la pantalla
            btnMaximizar_Click(sender, e);
            CargarDatosUsuario();
            //Oculta todos los sub-menús
            OcultarSubMenus();

            //Verifica el sexo para así identificar la imagen a mostrar
            if (UserLoginCache.Sexo == "Femenino")
            {
                imgUsuarioF.Visible = true;
            }
            else
            {
                imgUsuarioM.Visible = true;
            }

            //Permisos de usuario a partir del cargo del usuario que ha ingreado al programa
            if (UserLoginCache.Cargo == Cargos.Vendedor)
            {
                btnProductos.Enabled = false;
                btnEliminarVenta.Enabled = false;
                btnEliminarCliente.Enabled = false;
                btnCompras.Enabled = false;
                btnEmpleados.Enabled = false;
            }

            btnInicio.BackColor = Color.FromArgb(13, 93, 142);//Cambia el color del botón para simular que el botón de inicio está seleccionado
        }

        private void CargarDatosUsuario()
        {
            //Almacena el índice en donde se encuentra el primer espacio en blanco (Ya que solo quiero el primer nombre)
            int indice1 = UserLoginCache.Nombres.IndexOf(' ');
            //Almacena el índice en donde se encuentra el primer espacio en blanco (Ya que solo quiero el primer apellido)
            int indice2 = UserLoginCache.Apellidos.IndexOf(' ');
            string Nombre = UserLoginCache.Nombres.Remove(indice1);//Elimina todo lo que se encuentre a partir del espacio en blanco.
            string Apellido = UserLoginCache.Apellidos.Remove(indice2);
            lblNombre.Text = Nombre + " " + Apellido;//Mostramos el nombre del usuario
        }
        #endregion

        #region Redimencionar formulario
        //RESIZE METODO PARA REDIMENCIONAR/CAMBIAR TAMAÑO A FORMULARIO EN TIEMPO DE EJECUCION ----------------------------------------------------------
        private int tolerance = 12;
        private const int WM_NCHITTEST = 132;
        private const int HTBOTTOMRIGHT = 17;
        private Rectangle sizeGripRectangle;
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    base.WndProc(ref m);
                    var hitPoint = this.PointToClient(new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16));
                    if (sizeGripRectangle.Contains(hitPoint))
                        m.Result = new IntPtr(HTBOTTOMRIGHT);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }
        //----------------DIBUJAR RECTANGULO / EXCLUIR ESQUINA PANEL 
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            var region = new Region(new Rectangle(0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height));
            sizeGripRectangle = new Rectangle(this.ClientRectangle.Width - tolerance, this.ClientRectangle.Height - tolerance, tolerance, tolerance);
            region.Exclude(sizeGripRectangle);
            this.pnlPrincipal.Region = region;
            this.Invalidate();
        }
        //----------------COLOR Y GRIP DE RECTANGULO INFERIOR
        protected override void OnPaint(PaintEventArgs e)
        {
            SolidBrush blueBrush = new SolidBrush(Color.FromArgb(244, 244, 244));
            e.Graphics.FillRectangle(blueBrush, sizeGripRectangle);
            base.OnPaint(e);
            ControlPaint.DrawSizeGrip(e.Graphics, Color.Transparent, sizeGripRectangle);
        }
        #endregion

        #region Cerrar - Minimizar & Maximizar
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            //Consulta y pide al usuario que confirme que desea cerrar del todo la aplicaicón
            if (DialogResult.Yes == MessageBox.Show("¿ESTA SEGURO QUE DESEA CERRAR LA APLICACIÓN?", "¡ATENCION!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
            {
                btn.Cerrar();
            }
        }

        private void btnMaximizar_Click(object sender, EventArgs e)
        {
            btn.Maximizar(this, btnMaximizar, btnRestaurar);
        }

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            btn.Restaurar(this, btnRestaurar, btnMaximizar);
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            btn.Minimizar(this);
        }

        private void pnlSuperior_MouseDown(object sender, MouseEventArgs e)
        {
            btnMaximizar.Visible = true;
            btnRestaurar.Visible = false;
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        #endregion

        #region Mover Formulario
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);
        #endregion

        #region Cerrar_Sesion
        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            //Consulta y pide al usuario que confirme que desea cerrar del todo la aplicaicón
            if (DialogResult.Yes == MessageBox.Show("¿ESTA SEGURO QUE DESEA CERRAR SESION?", "¡ATENCION!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
            {
                this.Close();
            }
        }
        #endregion

        #region Ocultar - Mostrar SubMenús
        private void OcultarSubMenus()
        {
            //Oculta todos los submenús
            if (pnlSubProductos.Visible)
            {
                OcultarPanel_CambiarColorBtn_MsjAyuda(pnlSubProductos, btnProductos);
            }
            if (pnlSubVentas.Visible)
            {
                OcultarPanel_CambiarColorBtn_MsjAyuda(pnlSubVentas, btnVentas);
            }
            if (pnlSubClientes.Visible)
            {
                OcultarPanel_CambiarColorBtn_MsjAyuda(pnlSubClientes, btnClientes);
            }
            if (pnlSubCompras.Visible)
            {
                OcultarPanel_CambiarColorBtn_MsjAyuda(pnlSubCompras, btnCompras);
            }
            if (pnlSubEmpleados.Visible)
            {
                OcultarPanel_CambiarColorBtn_MsjAyuda(pnlSubEmpleados, btnEmpleados);
            }
        }

        private void OcultarPanel_CambiarColorBtn_MsjAyuda(Panel pnl, Button btn)
        {
            pnl.Visible = false;//Pone visible el panel de sub-menú
            btn.BackColor = Color.FromArgb(24, 30, 54);//Mantiene el color del botón para simular que no está seleccionado
            msmAyuda.SetToolTip(btn, "Desplegar submenú de opciones de " + btn.Text.ToLower());//Muestra el mensaje de ayuda
        }

        private void MostrarSubMenu(Panel mostrar, Button btnMenu)
        {
            if (mostrar.Visible == false)//Si el su-menu no está visible
            {
                btnInicio.BackColor = Color.FromArgb(24, 30, 54);//Mantiene el color del botón de inicio para simular que no está seleccionado
                btnMenu.BackColor = Color.FromArgb(13, 93, 142);//Cambia el color del botón indicado para simular que está seleccionado
                msmAyuda.SetToolTip(btnMenu, "Ocultar submenú de opciones de " + btnMenu.Text.ToLower());//Muestra el mensaje de ayuda
                OcultarSubMenus();//Oculta todos los submenús
                mostrar.Visible = true;//Pone visible el menú indicado
                //btn.BackColor = Color.FromArgb(0, 80, 200);
            }
            else//Si el sub-menu ya está visible
            {
                btnMenu.BackColor = Color.FromArgb(24, 30, 54);//Mantiene el color del botón para simular que no está seleccionado
                mostrar.Visible = false;//Oculta el panel indicado
                msmAyuda.SetToolTip(btnMenu, "Desplegar submenú de opciones de " + btnMenu.Text.ToLower());//Muestra el mensaje de ayuda
                //btn.BackColor = Color.FromArgb(32, 48, 68);
            }
        }

        private void btnInicio_Click(object sender, EventArgs e)
        {
            OcultarSubMenus();
            btnInicio.BackColor = Color.FromArgb(13, 93, 142);//Cambia el color del botón para simular que está seleccionado
        }

        private void btnProductos_Click(object sender, EventArgs e)
        {
            MostrarSubMenu(pnlSubProductos, btnProductos);
        }

        private void btnVentas_Click(object sender, EventArgs e)
        {
            MostrarSubMenu(pnlSubVentas, btnVentas);
        }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            MostrarSubMenu(pnlSubClientes, btnClientes);
        }

        private void btnCompras_Click(object sender, EventArgs e)
        {
            MostrarSubMenu(pnlSubCompras, btnCompras);
        }

        private void btnEmpleados_Click(object sender, EventArgs e)
        {
            MostrarSubMenu(pnlSubEmpleados, btnEmpleados);
        }
        #endregion
    }
}
