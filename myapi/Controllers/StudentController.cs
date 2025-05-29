using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using myapi.Data;
using myapi.Models;
using myapi.Validators.StudentValidator;

namespace myapi.Controllers
{
    [Route("apiv1/[controller]/[action]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentRepository _studentRepository;
        private readonly IValidator<StudentAddEditModel> _addEditValidator;
        private readonly IValidator<StudentLoginModel> _loginValidator;
        private readonly IConfiguration _configuration;
        private readonly IValidator<StudentUpdatePasswordModel> _updateValidator;
        public StudentController(StudentRepository studentRepository,
            IValidator<StudentAddEditModel> addEditValidator,
            IValidator<StudentLoginModel> loginValidator,
            IValidator<StudentUpdatePasswordModel> updateValidator,
            IConfiguration configuration)
        {
            _studentRepository = studentRepository;
            _addEditValidator = addEditValidator;
            _loginValidator = loginValidator;
            _configuration = configuration;
            _updateValidator = updateValidator;
        }

        [HttpGet]
        public IActionResult SelectAllStudent(int id)
        {
            try
            {
                var res = _studentRepository.SelectAllStudent(id);

                if (res == null || !res.Any())
                {
                    return NotFound(new
                    {
                        Status = "Failure",
                        Message = "No students found"
                    });
                }
                return Ok(new
                {
                    Status = "Success",
                    Data = res,
                    Message = "students found"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = "Failure",
                    Message = "Internal server error",
                    Error = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetStudentID(int id)
        {
            try
            {
                var res = _studentRepository.StudentSelectByID(id);
                if (res == null)
                {
                    return NotFound(new
                    {
                        Status = "Failure",
                        Message = "No student found"
                    });
                }
                return Ok(new
                {
                    Status = "Success",
                    Data = res,
                    Message = "student found"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = "Failure",
                    Message = "Internal server error",
                    Error = ex.Message
                });
            }
        }

        [HttpPost]
        public IActionResult StudentLogin([FromBody] StudentLoginModel sm)
        {
            try
            {
                var validateRes = _loginValidator.Validate(sm);

                if (!validateRes.IsValid)
                {

                    return BadRequest(new
                    {
                        Status = "Failure",
                        Errors = validateRes.Errors.Select(e => e.ErrorMessage)
                    });
                }

                var isLoginSuccessful = _studentRepository.StudentLogin(sm);

                if (!isLoginSuccessful.IsNullOrEmpty())
                {
                    return Ok(new
                    {
                        Status = "Success",
                        Data = isLoginSuccessful,
                        Message = "Login successful"
                    });
                }

                return Unauthorized(new
                {
                    Status = "Failure",
                    Message = "Invalid username or password"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = "Failure",
                    Message = "Internal server error",
                    Error = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id) 
        {
            try
            {
                var res = _studentRepository.StudentDelete(id);

                if(!res)
                {
                    return BadRequest(new
                    {
                        Status = "Failure",
                        Errors = "Failed to Delete"
                    });
                }
                else
                {
                    return Ok(new
                    {
                        Status = "Failure",
                        Message = "Student Deleted Successfully"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = "Failure",
                    Message = "Internal server error",
                    Error = ex.Message
                });
            }
        }

        [HttpPost]
        public IActionResult InsertStudent([FromBody] StudentAddEditModel sm)
        {
            var validateRes = _addEditValidator.Validate(sm);

            if(!validateRes.IsValid)
            {
                return BadRequest(new
                {
                    Status = "Failure",
                    Errors = validateRes.Errors.Select(e => e.ErrorMessage)
                });
            }

            var res = _studentRepository.StudentInsert(sm);

            if (res)
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("StudentID",sm.StudentID.ToString()),
                    new Claim("HostelID",sm.HostelID.ToString()),
                    new Claim("RoomID",sm.RoomID.ToString())
                };

				var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
				var sign = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(20),
                    signingCredentials: sign
                );

                string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

				return Ok(new
                {
                    Status = "Success",
                    Message = "Insertion successful" ,
                    Token = tokenValue
                });
            }
            else
            {
                return BadRequest(new
                {
                    Status = "Failure",
                    Errors = "Room Full! Cannot insert more students."
				});
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateStudent(int id,[FromBody] StudentAddEditModel sm)
        {
            var validateRes = _addEditValidator.Validate(sm);

            if (!validateRes.IsValid)
            {
                return BadRequest(new
                {
                    Status = "Failure",
                    Errors = validateRes.Errors.Select(e => e.ErrorMessage)
                });
            }

            var res = _studentRepository.StudentUpdate(id,sm);

            if (res)
            {
                return Ok(new
                {
                    Status = "Success",
                    Message = "Update successful"
                });
            }
            else
            {
                return BadRequest(new
                {
                    Status = "Failure",
                    Errors = "Failed to Update"
                });
            }
        }

        [HttpPut]
        public IActionResult StudentUpdatePassword([FromBody] StudentUpdatePasswordModel hm)
        {
            var validateRes = _updateValidator.Validate(hm);

            if (!validateRes.IsValid)
            {
                return BadRequest(new
                {
                    Status = "Failure",
                    Errors = validateRes.Errors.Select(e => e.ErrorMessage)
                });
            }

            var res = _studentRepository.UpdateStudentPassword(hm);

            if (res)
            {
                return Ok(new
                {
                    Status = "Success",
                    Message = "Password updated successfully"
                });
            }
            else
            {
                return BadRequest(new
                {
                    Status = "Failure",
                    Message = "Invalid password"
                });
            }
        }
    }
}
