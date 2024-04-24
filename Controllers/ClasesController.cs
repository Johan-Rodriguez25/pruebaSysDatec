using Microsoft.AspNetCore.Mvc;
using PruebaSysDatec.Models;
using System.Data.SqlClient;

namespace PruebaSysDatec.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClasesController : ControllerBase
    {
        public readonly string con;

        public ClasesController(IConfiguration configuration)
        {
            con = configuration.GetConnectionString("connection");
        }

        [HttpGet]
        public IActionResult Get()
        {
            List<Clase> clases = new();

            try
            {
                using (SqlConnection connection = new(con))
                {
                    connection.Open();
                    using (SqlCommand cmd = new("ObtenerClases", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Clase c = new Clase
                                {
                                    Clase_id = Convert.ToInt32(reader["Clase_id"]),
                                    Nombre_clase = reader["Nombre_clase"].ToString(),
                                    Profesor_id = Convert.ToInt32(reader["Profesor_id"])
                                };

                                clases.Add(c);
                            }
                        }
                    }
                }

                return Ok(clases);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener las clases: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
public IActionResult GetById(int id)
{
    try
    {
        Clase clase = new Clase();

        using (SqlConnection connection = new SqlConnection(con))
        {
            connection.Open();
            using (SqlCommand cmd = new SqlCommand("ObtenerClasePorId", connection))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Clase_id", id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        clase = new Clase
                        {
                            Clase_id = Convert.ToInt32(reader["Clase_id"]),
                            Nombre_clase = reader["Nombre_clase"].ToString(),
                            Profesor_id = Convert.ToInt32(reader["Profesor_id"])
                        };
                    }
                    else
                    {
                        return NotFound($"No se encontró ninguna clase con id {id}");
                    }
                }
            }
        }

        return Ok(clase);
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Error al obtener la clase: {ex.Message}");
    }
}

        [HttpPost]
        public IActionResult Post([FromBody] Clase c)
        {
            if (string.IsNullOrEmpty(c.Nombre_clase))
            {
                return BadRequest("El nombre de la clase es requerido.");
            }

            if (c.Profesor_id <= 0)
            {
                return BadRequest("El id del profesor es inválido.");
            }

            try
            {
                using (SqlConnection connection = new(con))
                {
                    connection.Open();
                    using (SqlCommand cmd = new("InsertarClase", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Nombre_clase", c.Nombre_clase);
                        cmd.Parameters.AddWithValue("Profesor_id", c.Profesor_id);
                        cmd.ExecuteNonQuery();
                    }
                }

                return Ok("Clase insertada correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al insertar la clase: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Clase c)
        {
            if (string.IsNullOrEmpty(c.Nombre_clase))
            {
                return BadRequest("El nombre de la clase es requerido.");
            }

            if (c.Profesor_id <= 0)
            {
                return BadRequest("El id del profesor es inválido.");
            }

            try
            {
                using (SqlConnection connection = new(con))
                {
                    connection.Open();
                    using (SqlCommand cmd = new("ActualizarClase", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Clase_id", id);
                        cmd.Parameters.AddWithValue("@Nombre_clase", c.Nombre_clase);
                        cmd.Parameters.AddWithValue("Profesor_id", c.Profesor_id);
                        cmd.ExecuteNonQuery();
                    }
                }

                return Ok($"Clase con id {id} actualizada correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar la clase: {ex.Message}");
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
                    using (SqlCommand cmd = new("EliminarClase", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Clase_id", id);
                        cmd.ExecuteNonQuery();
                    }
                }

                return Ok($"Clase con id {id} eliminada correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar la clase: {ex.Message}");
            }
        }
    }
}
