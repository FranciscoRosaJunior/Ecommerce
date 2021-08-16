using Ecommerce.Api.Interface;
using Ecommerce.Domain.Models;
using Ecommerce.Repository.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsuariosController : Controller
    {
        public readonly EcommerceContext _context;
        private readonly IJwtAuth _jwtAuth;

        public UsuariosController(EcommerceContext context, IJwtAuth jwtAuth)
        {
            _context = context;
            _jwtAuth = jwtAuth;
        }

        [HttpPost("Registrar")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(Usuario userRegister)
        {
            try
            {
                var user = new Usuario();

                if (userRegister.NomeCompleto == "" || userRegister.Email == "" || userRegister.Senha == "")
                {
                    return BadRequest("Todos os campos devem conter informações.");
                }

                var usuario = _context.Usuarios.Where(x => x.Email == userRegister.Email).ToList();

                if (usuario.Count > 0)
                {
                    return BadRequest("Email já cadastrado.");
                }

                user.NomeCompleto = userRegister.NomeCompleto;
                user.Email = userRegister.Email;
                user.Senha = userRegister.Senha;

                _context.Usuarios.Add(user);

                await _context.SaveChangesAsync();

                usuario = _context.Usuarios.Where(x => x.Email == userRegister.Email).ToList();

                if (usuario.Count > 0)
                {
                    return Created("GetUser", usuario);
                }

                return BadRequest();
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco Dados Falhou {ex.Message}");
            }
        }

        [AllowAnonymous]
        // POST api/<MembersController>
        [HttpPost("Autenticação")]
        public IActionResult GenerateJWToken([FromBody] UsuarioLogin userLogin)
        {
            var token = _jwtAuth.Authentication(userLogin.Email, userLogin.Senha);
            if (token == null)
                return Unauthorized();
            return Ok(token);
        }
    }
}
