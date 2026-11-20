using ClubesDeportivos.Models;
using Microsoft.Data.SqlClient;

namespace ClubesDeportivos.Datos;

public class ClubDatos
{
    private const string Consulta = @"SELECT c.club_id, c.nombre, c.deporte, c.ubicacion, c.fecha_fundacion,
        (SELECT COUNT(*) FROM Registros r WHERE r.club_id = c.club_id) AS total_miembros
        FROM Clubes c";

    private readonly string cadenaConexion;

    public ClubDatos(string cadenaConexion)
    {
        this.cadenaConexion = cadenaConexion;
    }

    public List<Club> Listar()
    {
        var clubes = new List<Club>();
        using var conexion = new SqlConnection(cadenaConexion);
        using var comando = new SqlCommand(Consulta + " ORDER BY c.nombre", conexion);
        conexion.Open();
        using var lector = comando.ExecuteReader();
        while (lector.Read())
        {
            clubes.Add(Leer(lector));
        }
        return clubes;
    }

    public Club? Buscar(int id)
    {
        using var conexion = new SqlConnection(cadenaConexion);
        using var comando = new SqlCommand(Consulta + " WHERE c.club_id = @id", conexion);
        comando.Parameters.AddWithValue("@id", id);
        conexion.Open();
        using var lector = comando.ExecuteReader();
        return lector.Read() ? Leer(lector) : null;
    }

    public void Insertar(Club club)
    {
        using var conexion = new SqlConnection(cadenaConexion);
        using var comando = new SqlCommand(
            "INSERT INTO Clubes (nombre, deporte, ubicacion, fecha_fundacion) VALUES (@nombre, @deporte, @ubicacion, @fecha_fundacion)", conexion);
        AgregarDatos(comando, club);
        conexion.Open();
        comando.ExecuteNonQuery();
    }

    public void Actualizar(Club club)
    {
        using var conexion = new SqlConnection(cadenaConexion);
        using var comando = new SqlCommand(
            "UPDATE Clubes SET nombre = @nombre, deporte = @deporte, ubicacion = @ubicacion, fecha_fundacion = @fecha_fundacion WHERE club_id = @id", conexion);
        comando.Parameters.AddWithValue("@id", club.ClubId);
        AgregarDatos(comando, club);
        conexion.Open();
        comando.ExecuteNonQuery();
    }

    public void Eliminar(int id)
    {
        using var conexion = new SqlConnection(cadenaConexion);
        using var comando = new SqlCommand("DELETE FROM Clubes WHERE club_id = @id", conexion);
        comando.Parameters.AddWithValue("@id", id);
        conexion.Open();
        comando.ExecuteNonQuery();
    }

    private static void AgregarDatos(SqlCommand comando, Club club)
    {
        comando.Parameters.AddWithValue("@nombre", club.Nombre);
        comando.Parameters.AddWithValue("@deporte", club.Deporte);
        comando.Parameters.AddWithValue("@ubicacion", club.Ubicacion);
        comando.Parameters.AddWithValue("@fecha_fundacion", club.FechaFundacion);
    }

    private static Club Leer(SqlDataReader lector)
    {
        return new Club
        {
            ClubId = (int)lector["club_id"],
            Nombre = (string)lector["nombre"],
            Deporte = (string)lector["deporte"],
            Ubicacion = (string)lector["ubicacion"],
            FechaFundacion = (DateTime)lector["fecha_fundacion"],
            TotalMiembros = (int)lector["total_miembros"]
        };
    }
}
