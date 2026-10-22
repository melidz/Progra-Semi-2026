
using System;
using System.Data;
using System.Data.SqlClient;

namespace Progra.parcial2
{
    public class Conexion_base_datos
    {
        private readonly string cadenaConexion =
            @"Data Source=(localdb)\ProjectsV13;Initial Catalog=db_academica;Integrated Security=True;";

        // Crear la conexión
        private SqlConnection ObtenerConexion()
        {
            return new SqlConnection(cadenaConexion);
        }

        // Crear el DataSet y cargar los usuarios
        public DataSet crearDs()
        {
            DataSet ds = new DataSet();

            using (SqlConnection cn = ObtenerConexion())
            {
                string sql = @"SELECT idUsuario, usuario, clave, nombre,
                               [dirección], teléfono
                               FROM usuarios_USSS148126";

                SqlDataAdapter adaptador = new SqlDataAdapter(sql, cn);

                adaptador.Fill(ds, "usuarios_USSS148126");
            }

            return ds;
        }

        // Ejecutar operaciones SQL
        public int procesar(string sql, params SqlParameter[] parametros)
        {
            using (SqlConnection cn = ObtenerConexion())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                if (parametros != null)
                {
                    cmd.Parameters.AddRange(parametros);
                }

                cn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        // Agregar, modificar o eliminar usuarios
        public int mantenimiento_usuarios_USSS148126(
            string accion,
            int idUsuario,
            string usuario,
            string clave,
            string nombre,
            string direccion,
            string telefono)
        {
            string sql;

            if (accion == "AGREGAR")
            {
                sql = @"INSERT INTO usuarios_USSS148126
                        (usuario, clave, nombre, [dirección], teléfono)
                        VALUES (@usuario, @clave, @nombre, @direccion, @telefono)";

                return procesar(sql,
                    new SqlParameter("@usuario", usuario),
                    new SqlParameter("@clave", clave),
                    new SqlParameter("@nombre", nombre),
                    new SqlParameter("@direccion", (object)direccion ?? DBNull.Value),
                    new SqlParameter("@telefono", (object)telefono ?? DBNull.Value));
            }
            else if (accion == "MODIFICAR")
            {
                sql = @"UPDATE usuarios_USSS148126
                        SET usuario = @usuario, clave = @clave,
                            nombre = @nombre, [dirección] = @direccion,
                            teléfono = @telefono
                        WHERE idUsuario = @idUsuario";
            }
            else if (accion == "ELIMINAR")
            {
                sql = @"DELETE FROM usuarios_USSS148126
                        WHERE idUsuario = @idUsuario";

                return procesar(sql,
                    new SqlParameter("@idUsuario", idUsuario));
            }
            else
            {
                throw new ArgumentException("Acción no válida.");
            }

            return procesar(sql,
                new SqlParameter("@idUsuario", idUsuario),
                new SqlParameter("@usuario", usuario),
                new SqlParameter("@clave", clave),
                new SqlParameter("@nombre", nombre),
                new SqlParameter("@direccion", (object)direccion ?? DBNull.Value),
                new SqlParameter("@telefono", (object)telefono ?? DBNull.Value));
        }
        public bool ValidarLogin(string usuario, string clave)
        {
            using (SqlConnection cn = ObtenerConexion())
            {
                string sql = @"SELECT COUNT(*)
                       FROM usuarios_USSS148126
                       WHERE usuario = @usuario
                       AND clave = @clave";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@usuario", usuario);
                    cmd.Parameters.AddWithValue("@clave", clave);

                    cn.Open();

                    int cantidad = Convert.ToInt32(cmd.ExecuteScalar());

                    return cantidad > 0;
                }
            }
        }
    }

}
