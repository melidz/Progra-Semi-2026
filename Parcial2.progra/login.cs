using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parcial2.progra
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        private void btningresar_Click(object sender, EventArgs e)
        {
        {
                // Consulta para verificar si el usuario y la clave existen en tu tabla
                string query = "SELECT COUNT(*) FROM Usuario_USSS148126 WHERE Usuario = '" + txtUsuario.Text + "' AND Clave = '" + txtClave.Text + "'";

                // Ejecuta la consulta usando tu clase de conexión (cambia "Conexion_base_datos" si tu clase se llama diferente)
                int resultado = Convert.ToInt32(Conexion_base_datos.ejecutarScalar(query));

                if (resultado > 0)
                {
                    MessageBox.Show("¡Bienvenido al sistema!", "Acceso Concedido", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Abre el formulario de usuario o menú principal
                    login principal = new login();
                    principal.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos. Acceso denegado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
