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
    public partial class frm_usuario : Form
    {

        private Conexion_base_datos conexion = new Conexion_base_datos();
        private System.Data.DataSet ds;
        private int posicion = 0;

        public frm_usuario()
        {
            InitializeComponent();
            CargarUsuarios();
        }

        private void frm_usuario_Load(object sender, EventArgs e)
        {

        }

        private void CargarUsuarios()
        {
            ds = conexion.crearDs();
            posicion = 0;
            dgvUsuarios.DataSource = ds.Tables["usuarios_USSS148126"];
            MostrarUsuario();
        }

        private void MostrarUsuario()
        {
            if (ds == null ||
                !ds.Tables.Contains("usuarios_USSS148126") ||
                ds.Tables["usuarios_USSS148126"].Rows.Count == 0)
            {
                return;
            }

            DataRow fila = ds.Tables["usuarios_USSS148126"].Rows[posicion];

            txtIdUsuario.Text = fila["idUsuario"].ToString();
            txtUsuario.Text = fila["usuario"].ToString();
            txtClave.Text = fila["clave"].ToString();
            txtNombre.Text = fila["nombre"].ToString();
            txtDireccion.Text = fila["dirección"].ToString();
            txtTelefono.Text = fila["teléfono"].ToString();

            txtConfirmarClave.Clear();

        }

        private void btnPrimero_Click(object sender, EventArgs e)
        {
            if (ds != null &&
                ds.Tables["usuarios_USSS148126"].Rows.Count > 0)
            {
                posicion = 0;
                MostrarUsuario();
            }
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            if (ds != null && posicion > 0)
            {
                posicion--;
                MostrarUsuario();
            }
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            if (ds != null &&
                posicion < ds.Tables["usuarios_USSS148126"].Rows.Count - 1)
            {
                posicion++;
                MostrarUsuario();
            }
        }

        private void btnUltimo_Click(object sender, EventArgs e)
        {
            if (ds != null &&
                ds.Tables["usuarios_USSS148126"].Rows.Count > 0)
            {
                posicion = ds.Tables["usuarios_USSS148126"].Rows.Count - 1;
                MostrarUsuario();
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (!ValidarUsuario())
                return;

            try
            {
                int resultado = conexion.mantenimiento_usuarios_USSS148126(
                    "AGREGAR",
                    0,
                    txtUsuario.Text.Trim(),
                    txtClave.Text,
                    txtNombre.Text.Trim(),
                    txtDireccion.Text.Trim(),
                    txtTelefono.Text.Trim()
                );

                if (resultado > 0)
                {
                    MessageBox.Show("Usuario agregado correctamente.");

                    CargarUsuarios();

                    posicion = ds.Tables["usuarios_USSS148126"].Rows.Count - 1;

                    MostrarUsuario();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar usuario: " + ex.Message);
            }
        }

        private bool ValidarUsuario()
        {
            string usuario = txtUsuario.Text.Trim();
            string clave = txtClave.Text;
            string confirmar = txtConfirmarClave.Text;

            if (usuario.Length < 6 || usuario.Length > 16)
            {
                MessageBox.Show("El usuario debe tener entre 6 y 16 caracteres.");
                txtUsuario.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Escribe el nombre completo.");
                txtNombre.Focus();
                return false;
            }

            if (clave != confirmar)
            {
                MessageBox.Show("Las contraseñas no coinciden.");
                txtConfirmarClave.Focus();
                return false;
            }

            bool tieneLetra = false;
            bool tieneNumero = false;

            foreach (char c in clave)
            {
                if (char.IsLetter(c))
                    tieneLetra = true;

                if (char.IsDigit(c))
                    tieneNumero = true;
            }

            if (!tieneLetra || !tieneNumero)
            {
                MessageBox.Show("La contraseña debe contener letras y números.");
                txtClave.Focus();
                return false;
            }

            return true;
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIdUsuario.Text))
            {
                MessageBox.Show("Selecciona un usuario para modificar.");
                return;
            }

            if (!ValidarUsuario())
                return;

            try
            {
                int id = int.Parse(txtIdUsuario.Text);

                int resultado = conexion.mantenimiento_usuarios_USSS148126(
                    "MODIFICAR",
                    id,
                    txtUsuario.Text.Trim(),
                    txtClave.Text,
                    txtNombre.Text.Trim(),
                    txtDireccion.Text.Trim(),
                    txtTelefono.Text.Trim()
                );

                if (resultado > 0)
                {
                    MessageBox.Show("Usuario modificado correctamente.");
                    CargarUsuarios();

                    for (int i = 0; i < ds.Tables["usuarios_USSS148126"].Rows.Count; i++)
                    {
                        if (Convert.ToInt32(
                            ds.Tables["usuarios_USSS148126"].Rows[i]["idUsuario"]) == id)
                        {
                            posicion = i;
                            break;
                        }
                    }

                    MostrarUsuario();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar usuario: " + ex.Message);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIdUsuario.Text))
            {
                MessageBox.Show("Selecciona un usuario para eliminar.");
                return;
            }

            DialogResult respuesta = MessageBox.Show(
                "¿Deseas eliminar este usuario?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (respuesta != DialogResult.Yes)
                return;

            try
            {
                int resultado = conexion.mantenimiento_usuarios_USSS148126(
                    "ELIMINAR",
                    int.Parse(txtIdUsuario.Text),
                    "", "", "", "", ""
                );

                if (resultado > 0)
                {
                    MessageBox.Show("Usuario eliminado correctamente.");
                    CargarUsuarios();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar usuario: " + ex.Message);
            }
        }
    }
}
