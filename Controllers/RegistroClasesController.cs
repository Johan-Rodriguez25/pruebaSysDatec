using Microsoft.AspNetCore.Mvc;
using PruebaSysDatec.Models;
using System.Data.SqlClient;

namespace PruebaSysDatec.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistroClasesController : ControllerBase
    {
        public readonly string con;

        public RegistroClasesController(IConfiguration configuration)
        {
            con = configuration.GetConnectionString("connection");
        }

        [HttpGet]
        public IActionResult Get()
        {
            List<RegistroClase> registroClases = new();

            try
            {
                using (SqlConnection connection = new(con))
                {
                    connection.Open();
                    using (SqlCommand cmd = new("ObtenerRegistroClases", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                RegistroClase r = new RegistroClase
                                {
                                    Registro_id = Convert.ToInt32(reader["Registro_id"]),
                                    Estudiante_id = Convert.ToInt32(reader["Estudiante_id"]),
                                    Clase_id = Convert.ToInt32(reader["Clase_id"])
                                };

                                registroClases.Add(r);
                            }
                        }
                    }
                }

                return Ok(registroClases);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener los registros de clase: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] RegistroClase r)
        {
            if (r.Estudiante_id <= 0 || r.Clase_id <= 0)
            {
                return BadRequest("El id del estudiante y el id de la clase son requeridos y deben ser mayores que cero.");
            }

            bool estudianteRegistrado = EstudianteRegistradoEnClase(r.Estudiante_id, r.Clase_id);
            if (estudianteRegistrado)
            {
                return BadRequest("El estudiante ya está registrado en esta clase.");
            }

            int estudiantesEnClase = ObtenerEstudiantesEnClase(r.Clase_id);
            if (estudiantesEnClase >= 20)
            {
                return BadRequest("La clase ya tiene el máximo de 20 estudiantes inscritos.");
            }

            try
            {
                using (SqlConnection connection = new(con))
                {
                    connection.Open();
                    using (SqlCommand cmd = new("AsignarClaseAEstudiante", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Estudiante_id", r.Estudiante_id);
                        cmd.Parameters.AddWithValue("@Clase_id", r.Clase_id);
                        cmd.ExecuteNonQuery();
                    }
                }

                return Ok("Registro de clase guardado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al guardado el registro de clase: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] RegistroClase r)
        {
            if (r.Estudiante_id <= 0 || r.Clase_id <= 0)
            {
                return BadRequest("El id del estudiante y el id de la clase son requeridos y deben ser mayores que cero.");
            }

            try
            {
                using (SqlConnection connection = new(con))
                {
                    connection.Open();
                    using (SqlCommand cmd = new("ActualizarRegistroClase", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Registro_id", id);
                        cmd.Parameters.AddWithValue("@Estudiante_id", r.Estudiante_id);
                        cmd.Parameters.AddWithValue("@Clase_id", r.Clase_id);
                        cmd.ExecuteNonQuery();
                    }
                }

                return Ok($"Registro de clase con id {id} actualizado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar el registro de clase: {ex.Message}");
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
                    using (SqlCommand cmd = new("EliminarRegistroClase", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Registro_id", id);
                        cmd.ExecuteNonQuery();
                    }
                }

                return Ok($"Registro de clase con id {id} eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar el registro de clase: {ex.Message}");
            }
        }

        private int ObtenerEstudiantesEnClase(int claseId)
        {
            int estudiantesEnClase = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(con))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("ObtenerCantidadEstudiantesEnClase", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Clase_id", claseId);
                        estudiantesEnClase = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener la cantidad de estudiantes en la clase: {ex.Message}");
            }

            return estudiantesEnClase;
        }

        private bool EstudianteRegistradoEnClase(int estudianteId, int claseId)
        {
            bool registrado = false;

            try
            {
                using (SqlConnection connection = new(con))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("VerificarEstudianteEnClase", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Estudiante_id", estudianteId);
                        cmd.Parameters.AddWithValue("@Clase_id", claseId);
                        registrado = Convert.ToBoolean(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al verificar si el estudiante está registrado en la clase: {ex.Message}");
            }

            return registrado;
        }
    }
}
