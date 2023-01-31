using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Dominio.Modelos;

namespace Presentacion.Forms
{
    public partial class RecuperarPassword : Form
    {
        public RecuperarPassword()
        {
            InitializeComponent();
        }

        private void btnRecuperar_Click(object sender, EventArgs e)
        {
            var empleado = new ModeloEmpleado();
            string resultado = empleado.recuperarPassword(txtUsuario.Text);
            lblResultado.Text = resultado;
        }
    }
}
