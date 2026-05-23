using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using StudentManagementAPI.Repository;
using System.Numerics;
using System.Security.Claims;
using System.Xml.Linq;
using StudentManagementAPI.Models;

namespace StudentManagementAPI.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly StudentRepository _repo;

        public StudentController(StudentRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult GetStudents()
        {
            var userIdCheck = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdCheck, out int userId))
                return Unauthorized("Invalid token");

            using (var con = _repo.GetCon())
            {
                con.Open();

                var cmd = new SqlCommand("select s.Id, s.Name, s.Email, s.Phone, u.Username AS CreatedByName from Students s join Users u ON s.CreatedBy = u.Id where s.CreatedBy = @uid", con);
                cmd.Parameters.AddWithValue("@uid", userId);

                var reader = cmd.ExecuteReader();

                var list = new List<object>();

                while (reader.Read())
                {
                    list.Add(new
                    {
                        Id = reader["Id"],
                        Name = reader["Name"],
                        Email = reader["Email"],
                        Phone = reader["Phone"],
                        CreatedBy = reader["CreatedByName"]
                    });
                }

                return Ok(list);
            }
        }

        [HttpPost]
        public IActionResult AddStudent([FromBody] Student student)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("Invalid token");

            int userId = int.Parse(userIdClaim);

            using (var con = _repo.GetCon())
            {
                con.Open();

                var cmd = new SqlCommand("insert into Students(Name, Email, Phone, CreatedBy, CreatedAt, UpdatedAt) values (@n, @e, @p, @cb, GETDATE(), GETDATE())", con);
                cmd.Parameters.AddWithValue("@n", student.Name);
                cmd.Parameters.AddWithValue("@e", student.Email);
                cmd.Parameters.AddWithValue("@p", student.Phone);
                cmd.Parameters.AddWithValue("@cb", userId);

                cmd.ExecuteNonQuery();

                return Ok("Student added");
            }
        }

        private int GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(claim, out int userId))
                throw new UnauthorizedAccessException("Invalid token");

            return userId;
        }

        [HttpGet("{id}")]
        public IActionResult GetStudent(int id)
        {
            int userId = GetUserId();

            using (var con = _repo.GetCon())
            {
                con.Open();

                var cmd = new SqlCommand("select * from Students where Id=@id AND CreatedBy=@uid", con);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@uid", userId);

                var reader = cmd.ExecuteReader();

                if (!reader.Read())
                    return NotFound("Student not found");

                var student = new
                {
                    Id = reader["Id"],
                    Name = reader["Name"],
                    Email = reader["Email"],
                    Phone = reader["Phone"]
                };

                return Ok(student);
            }
        }


        [HttpPut("{id}")]
        public IActionResult UpdateStudent(int id, [FromBody] Student student)
        {
            int userId = GetUserId();

            using (var con = _repo.GetCon())
            {
                con.Open();

                var cmd = new SqlCommand("update Students set Name=@n, Email=@e, Phone=@p, UpdatedAt=getdate() where Id=@id AND CreatedBy=@uid", con);

                cmd.Parameters.AddWithValue("@n", student.Name);
                cmd.Parameters.AddWithValue("@e", student.Email);
                cmd.Parameters.AddWithValue("@p", student.Phone);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@uid", userId);

                int rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                    return NotFound("Student not found or not authorized");

                return Ok("Student updated");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            int userId = GetUserId();

            using (var con = _repo.GetCon())
            {
                con.Open();

                var cmd = new SqlCommand("delete from Students where Id=@id AND CreatedBy=@uid", con);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@uid", userId);

                int rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                    return NotFound("Student not found or not authorized");

                return Ok("Student deleted");
            }
        }
    }
}
