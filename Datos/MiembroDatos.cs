using ClubesDeportivos.Models;
using Microsoft.Data.SqlClient;

namespace ClubesDeportivos.Datos;

public class MiembroDatos
{
    private const string Consulta = "SELECT miembro_id, nombre, apellido, email, telefono FROM Miembros";

    private readonly string cadenaConexion;

    public MiembroDatos(string cadenaConexion)
    {
        this.cadenaConexion = cadenaConexion;
    }

    public List<Miembro> Listar()
    {
        var miembros = new List<Miembro>();
        using var conexion = new SqlConnection(cadenaConexion);
        using var comando = new SqlCommand(Consulta + " ORDER BY apellido, nombre", conexion);
        conexion.Open();
        using var lector = comando.ExecuteReader();
        while (lector.Read())
        {
            miembros.Add(Leer(lector));
        }
        return miembros;
    }

    public Miembro? Buscar(int id)
    {
        using var conexion = new SqlConnection(cadenaConexion);
        using var comando = new SqlCommand(Consulta + " WHERE miembro_id = @id", conexion);
        comando.Parameters.AddWithValue("@id", id);
        conexion.Open();
        using var lector = comando.ExecuteReader();
        return lector.Read() ? Leer(lector) : null;
    }

    public void Insertar(Miembro miembro)
    {
        using var conexion = new SqlConnection(cadenaConexion);
        using var comando = new SqlCommand(
            "INSERT INTO Miembros (nombre, apellido, email, telefono) VALUES (@nombre, @apellido, @email, @telefono)", conexion);
        AgregarDatos(comando, miembro);
        conexion.Open();
        comando.ExecuteNonQuery();
    }

    public void Actualizar(Miembro miembro)
    {
        using var conexion = new SqlConnection(cadenaConexion);
        using var comando = new SqlCommand(
            "UPDATE Miembros SET nombre = @nombre, apellido = @apellido, email = @email, telefono = @telefono WHERE miembro_id = @id", conexion);
        comando.Parameters.AddWithValue("@id", miembro.MiembroId);
        AgregarDatos(comando, miembro);
        conexion.Open();
        comando.ExecuteNonQuery();
    }

    public void Eliminar(int id)
    {
        using var conexion = new SqlConnection(cadenaConexion);
        using var comando = new SqlCommand("DELETE FROM Miembros WHERE miembro_id = @id", conexion);
        comando.Parameters.AddWithValue("@id", id);
        conexion.Open();
        comando.ExecuteNonQuery();
    }

    private static void AgregarDatos(SqlCommand comando, Miembro miembro)
    {
        comando.Parameters.AddWithValue("@nombre", miembro.Nombre);
        comando.Parameters.AddWithValue("@apellido", miembro.Apellido);
        comando.Parameters.AddWithValue("@email", miembro.Email);
        comando.Parameters.AddWithValue("@telefono", miembro.Telefono);
    }

    private static Miembro Leer(SqlDataReader lector)
    {
        return new Miembro
        {
            MiembroId = (int)lector["miembro_id"],
            Nombre = (string)lector["nombre"],
            Apellido = (string)lector["apellido"],
            Email = (string)lector["email"],
            Telefono = (string)lector["telefono"]
        };
    }
}
