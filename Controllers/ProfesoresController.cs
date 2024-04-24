using Microsoft.AspNetCore.Mvc;
using PruebaSysDatec.Models;
using System.Data.SqlClient;

namespace PruebaSysDatec.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfesoresController : ControllerBase
    {
        public readonly string con;

        public ProfesoresController(IConfiguration configuration)
        {
            con = configuration.GetConnectionString("connection");
        }

        [HttpGet]
        public IActionResult Get()
        {
            List<Profesor> profesores = new();

            try
            {
                using (SqlConnection connection = new(con))
                {
                    connection.Open();
                    using (SqlCommand cmd = new("ObtenerProfesores", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Profesor p = new Profesor
                                {
                                    Profesor_id = Convert.ToInt32(reader["Profesor_id"]),
                                    Nombre_profesor = reader["Nombre_profesor"].ToString(),
                                };

                                profesores.Add(p);
                            }
                        }
                    }
                }

                return Ok(profesores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener los profesores: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
public IActionResult GetById(int id)
{
    try
    {
        Profesor profesor = new Profesor();

        using (SqlConnection connection = new SqlConnection(con))
        {
            connection.Open();
            using (SqlCommand cmd = new SqlCommand("ObtenerProfesorPorId", connection))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Profesor_id", id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        profesor = new Profesor
                        {
                            Profesor_id = Convert.ToInt32(reader["Profesor_id"]),
                            Nombre_profesor = reader["Nombre_profesor"].ToString(),
                        };
                    }
                    else
                    {
                        return NotFound($"No se encontró ningún profesor con id {id}");
                    }
                }
            }
        }

        return Ok(profesor);
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Error al obtener el profesor: {ex.Message}");
    }
}

        [HttpPost]
        public IActionResult Post([FromBody] Profesor p)
        {
            if (string.IsNullOrEmpty(p.Nombre_profesor))
            {
                return BadRequest("El nombre del profesor es requerido.");
            }

            try
            {
                using (SqlConnection connection = new(con))
                {
                    connection.Open();
                    using (SqlCommand cmd = new("InsertarProfesor", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Nombre_profesor", p.Nombre_profesor);
                        cmd.ExecuteNonQuery();
                    }
                }

                return Ok("Profesor guardado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al guardar agl profesor: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromBody] Profesor p, int id)
        {
            try
            {
                using (SqlConnection connection = new(con))
                {
                    connection.Open();
                    using (SqlCommand cmd = new("ActualizarProfesor", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Profesor_id", id);
                        cmd.Parameters.AddWithValue("@Nombre_profesor", p.Nombre_profesor);
                        cmd.ExecuteNonQuery();
                    }
                }

                return Ok($"Profesor con id {id} actualizado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar al profesor: {ex.Message}");
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
                    using (SqlCommand cmd = new("EliminarProfesor", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Profesor_id", id);
                        cmd.ExecuteNonQuery();
                    }
                }

                return Ok($"Profesor con id {id} eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar el profesor: {ex.Message}");
            }
        }
    }
}
