using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentacion.Botones
{
    public class BtnsPnlSuperior
    {
        public void Maximizar(Form edit, Button btnMaximizar, Button btnRestaurar)
        {
            edit.WindowState = FormWindowState.Maximized;
            btnMaximizar.Visible = false;
            btnRestaurar.Visible = true;
        }
        public void Restaurar(Form edit, Button btnRestaurar, Button btnMaximizar)
        {
            edit.WindowState = FormWindowState.Normal;
            btnRestaurar.Visible = false;
            btnMaximizar.Visible = true;
        }
        public void Minimizar(Form edit)
        {
            edit.WindowState = FormWindowState.Minimized;
        }
        public void Cerrar()
        {
            Application.Exit();
        }
    }
}
