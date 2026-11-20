using ClubesDeportivos.Models;
using Microsoft.Data.SqlClient;

namespace ClubesDeportivos.Datos;

public class RegistroDatos
{
    private readonly string cadenaConexion;

    public RegistroDatos(string cadenaConexion)
    {
        this.cadenaConexion = cadenaConexion;
    }

    public List<Registro> Listar()
    {
        var registros = new List<Registro>();
        using var conexion = new SqlConnection(cadenaConexion);
        using var comando = new SqlCommand(@"SELECT r.registro_id, r.club_id, r.miembro_id, r.fecha_registro,
            c.nombre AS club, c.deporte, m.nombre + ' ' + m.apellido AS miembro, m.email
            FROM Registros r
            JOIN Clubes c ON c.club_id = r.club_id
            JOIN Miembros m ON m.miembro_id = r.miembro_id
            ORDER BY c.nombre, m.apellido, m.nombre", conexion);
        conexion.Open();
        using var lector = comando.ExecuteReader();
        while (lector.Read())
        {
            registros.Add(new Registro
            {
                RegistroId = (int)lector["registro_id"],
                ClubId = (int)lector["club_id"],
                MiembroId = (int)lector["miembro_id"],
                FechaRegistro = (DateTime)lector["fecha_registro"],
                Club = (string)lector["club"],
                Deporte = (string)lector["deporte"],
                Miembro = (string)lector["miembro"],
                Email = (string)lector["email"]
            });
        }
        return registros;
    }

    public void Insertar(Registro registro)
    {
        using var conexion = new SqlConnection(cadenaConexion);
        using var comando = new SqlCommand(
            "INSERT INTO Registros (club_id, miembro_id, fecha_registro) VALUES (@club_id, @miembro_id, @fecha_registro)", conexion);
        comando.Parameters.AddWithValue("@club_id", registro.ClubId);
        comando.Parameters.AddWithValue("@miembro_id", registro.MiembroId);
        comando.Parameters.AddWithValue("@fecha_registro", registro.FechaRegistro);
        conexion.Open();
        comando.ExecuteNonQuery();
    }

    public void Eliminar(int id)
    {
        using var conexion = new SqlConnection(cadenaConexion);
        using var comando = new SqlCommand("DELETE FROM Registros WHERE registro_id = @id", conexion);
        comando.Parameters.AddWithValue("@id", id);
        conexion.Open();
        comando.ExecuteNonQuery();
    }
}
