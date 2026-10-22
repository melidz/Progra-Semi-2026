using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Progra.parcial2
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            Conexion_base_datos conexion = new Conexion_base_datos();

            if (conexion.ValidarLogin(
                txtUsuario.Text.Trim(),
                txtClave.Text))
            {
                MessageBox.Show("Inicio de sesión correcto.");

                frm_usuario formulario = new frm_usuario();
                formulario.Show();

                this.Hide();
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos.");
                txtClave.Clear();
                txtClave.Focus();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
