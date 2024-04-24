using Microsoft.AspNetCore.Mvc;
using PruebaSysDatec.Models;
using System.Data.SqlClient;

namespace PruebaSysDatec.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstudiantesController : ControllerBase
    {
        public readonly string con;

        public EstudiantesController(IConfiguration configuration)
        {
            con = configuration.GetConnectionString("connection");
        }

        [HttpGet]
        public IActionResult Get()
        {
            List<Estudiante> estudiantes = new();

            try
            {
                using (SqlConnection connection = new(con))
                {
                    connection.Open();
                    using (SqlCommand cmd = new("ObtenerEstudiantes", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Estudiante e = new Estudiante
                                {
                                    Estudiante_id = Convert.ToInt32(reader["Estudiante_id"]),
                                    Nombre = reader["Nombre"].ToString(),
                                };

                                estudiantes.Add(e);
                            }
                        }
                    }
                }

                return Ok(estudiantes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener los estudiantes: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
public IActionResult GetById(int id)
{
    try
    {
        Estudiante estudiante = new Estudiante();

        using (SqlConnection connection = new SqlConnection(con))
        {
            connection.Open();
            using (SqlCommand cmd = new SqlCommand("ObtenerEstudiantePorId", connection))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Estudiante_id", id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        estudiante = new Estudiante
                        {
                            Estudiante_id = Convert.ToInt32(reader["Estudiante_id"]),
                            Nombre = reader["Nombre"].ToString(),
                        };
                    }
                    else
                    {
                        return NotFound($"No se encontró ningún estudiante con id {id}");
                    }
                }
            }
        }

        return Ok(estudiante);
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Error al obtener el estudiante: {ex.Message}");
    }
}

        [HttpPost]
        public IActionResult Post([FromBody] Estudiante e)
        {
            if (string.IsNullOrEmpty(e.Nombre))
            {
                return BadRequest("El nombre del estudiante es requerido.");
            }

            try
            {
                using (SqlConnection connection = new(con))
                {
                    connection.Open();
                    using (SqlCommand cmd = new("InsertarEstudiante", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Nombre", e.Nombre);
                        cmd.ExecuteNonQuery();
                    }
                }

                return Ok("Estudiante guardado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al guardar el estudiante: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Estudiante e)
        {
            if (string.IsNullOrEmpty(e.Nombre))
            {
                return BadRequest("El nombre del estudiante es requerido.");
            }

            try
            {
                using (SqlConnection connection = new(con))
                {
                    connection.Open();
                    using (SqlCommand cmd = new("ActualizarEstudiante", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Estudiante_id", id);
                        cmd.Parameters.AddWithValue("@Nombre", e.Nombre);
                        cmd.ExecuteNonQuery();
                    }
                }

                return Ok($"Estudiante con id {id} actualizado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar el estudiante: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                using (SqlConnection connection = new(con))
                {
                    connection.Open();
                    using (SqlCommand cmd = new("EliminarEstudiante", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Estudiante_id", id);
                        cmd.ExecuteNonQuery();
                    }
                }

                return Ok($"Estudiante con id {id} eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar el estudiante: {ex.Message}");
            }
        }
    }
}
