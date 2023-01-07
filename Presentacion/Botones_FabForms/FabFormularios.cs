using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion.Botones_FabForms
{
    internal class FabFormularios
    {
        private Form? formActivo = null;

        public void AbrirFormsHijos(Form formHijo, Panel pnlContenedor)
        {
            if (formActivo != null)//Si el formulario es diferente de nulo (Ya se ha instanciado)
            {
                formActivo.Close();//Se cierra la anterior instancia
                formActivo = formHijo;//Se le asigna el nuevo formulario a instanciar
                formHijo.TopLevel = false;//Indica que el formulario hijo no es de nivel superior, es decir que se comportará como un control
                formHijo.FormBorderStyle = FormBorderStyle.None;//Quita el borde del formulario
                formHijo.Dock = DockStyle.Fill;//Rellena todo el panel contenedor con el formulario hijo
                pnlContenedor.Controls.Add(formHijo);//ALmacena el formulario hijo en el panel contenedor
                pnlContenedor.Tag = formHijo;//Asocia el formulario con el panel contenedor
                formHijo.BringToFront();//Enviamos el panel al frente
                formHijo.Show();//Mostramos el panel
            }
        }
    }
}
