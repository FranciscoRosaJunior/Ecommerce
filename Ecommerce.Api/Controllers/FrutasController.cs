using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ecommerce.Repository.Context;
using Ecommerce.Domain.Models;

namespace Ecommerce.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FrutasController : Controller
    {
        public readonly EcommerceContext _context;
        public FrutasController(EcommerceContext context)
        {
            _context = context;
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var results = await _context.Frutas.ToListAsync();

                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha banco de dados");
            }
        }
        [HttpGet("{FrutaId}")]
        public async Task<Fruta> GetById(int FrutaId)
        {
            IQueryable<Fruta> query = _context.Frutas;

            query = query
                        .AsNoTracking()
                        .Where(x => x.Id == FrutaId);

            return await query.FirstOrDefaultAsync();
        }

        [HttpGet("GetByName/{nome}")]
        public async Task<List<Fruta>> GetByName(string nome)
        {
            IQueryable<Fruta> query = _context.Frutas;

            query = query.AsNoTracking()
                        .OrderByDescending(x => x.Nome)
                        .Where(x => x.Nome.ToLower().Contains(nome.ToLower()));

            return await query.ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Post(Fruta model)
        {
            try
            {
                var fruta = new Fruta();
                fruta = model;

                _context.Frutas.Add(fruta);

                if (await _context.SaveChangesAsync() > 0)
                {
                    return Created($"/api/Frutas/{model.Id}", await _context.Frutas.Where(x => x.Id == model.Id).ToListAsync());
                }
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Banco Dados Falhou {ex.Message}");
            }

            return BadRequest();
        }

        //UPDATE
        [HttpPut("{FrutaId}")]
        public async Task<IActionResult> Put(int FrutaId, Fruta model)
        {
            try
            {
                var fruta = await _context.Frutas.Where(x => x.Id == FrutaId).FirstOrDefaultAsync();
                if (fruta == null) return NotFound();
                if (model.Nome != null) fruta.Nome = model.Nome;
                if (model.Descricao != null) fruta.Descricao = model.Descricao;
                if (model.Foto != null) fruta.Foto = model.Foto;
                if (model.Quantidade > 0) fruta.Quantidade = model.Quantidade;
                if (model.Estoque > 0) fruta.Estoque = model.Estoque;
                if (model.Valor > 0) fruta.Valor = model.Valor;

                _context.Update(fruta);

                if (await _context.SaveChangesAsync() > 0)
                {
                    return Created($"/api/Frutas/{FrutaId}", fruta);
                }
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco Dados Falhou " + ex.Message);
            }

            return BadRequest();
        }

        //DELETE
        [HttpDelete("{FrutaId}")]
        public async Task<IActionResult> Delete(int FrutaId)
        {
            try
            {
                var fruta = await _context.Frutas.Where(x => x.Id == FrutaId).FirstOrDefaultAsync();
                if (fruta == null) return NotFound();

                _context.Remove(fruta);

                if (await _context.SaveChangesAsync() > 0)
                {
                    return Ok();
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco Dados Falhou");
            }

            return BadRequest();
        }
    }
}
