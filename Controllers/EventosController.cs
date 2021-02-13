using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventosWebApi_v1.Models;
using Microsoft.AspNetCore.Authorization;
using EventosWebApi_v1.Models.Authentication;

namespace EventosWebApi_v1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class EventosController : ControllerBase
    {
        private readonly EventosDbContext _context;

        public EventosController(EventosDbContext context)
        {
            _context = context;
        }

        // GET: api/Eventos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Evento>>> GetEventos()
        {
            return await _context.Eventos.Include(y => y.Tipo).Include(x => x.Local).OrderBy(y=> y.Data).ToListAsync();
        }

        // GET: api/Eventos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Evento>> GetEvento(int id)
        {

            var evento = await _context.Eventos.FindAsync(id);

            System.Diagnostics.Debug.WriteLine(evento.LocalId);

            var local = await _context.Locais.FindAsync(evento.LocalId);
            var tipo = await _context.Tipos.FindAsync(evento.TipoId);

            evento.Local = local;
            evento.Tipo = tipo;

            if (evento == null)
            {
                return NotFound();
            }
            return evento;
        }


        [HttpGet("tipo/{tipoEventoId}/local/{localId}/data/{mes}")]
        public async Task<ActionResult<List<Evento>>> GetEventosFilter(int tipoEventoId, int localId, String mes)
        {
            System.Diagnostics.Debug.WriteLine("Tipo de evento recebido->" + tipoEventoId + " local->" + localId + "data->" + mes);
            //ir buscar a localidade através do localId
            List<Local> stringLocalidade = await _context.Locais.Where(l => l.Id == localId).ToListAsync();

            string v = stringLocalidade[0].Localidade.ToString();
           

            //guardar na variavel quais os eventos que
            //têm o mesmo tipoEventoId. ex: guardo todos os eventos do tipo dança
            List<Evento> eventosDoMesmoTipo = await _context.Eventos.Where(et => et.TipoId == tipoEventoId).Where(et => et.Local.Localidade == v).ToListAsync();

            if(eventosDoMesmoTipo.Count() == 0)
            {
                return new List<Evento>();
            }

            string id = eventosDoMesmoTipo[0].Titulo.ToString();

            //var mes = data.Date.Month; //vou buscar o mês recebido

            List<Evento> eventosR = new List<Evento>();
            
            foreach (Evento evento in eventosDoMesmoTipo.ToList())
            {
                var local = await _context.Locais.FindAsync(evento.LocalId);
                var tipo = await _context.Tipos.FindAsync(evento.TipoId);
                if (evento.Data.Month.ToString() == mes)
                {
                    evento.Local = local;
                    evento.Tipo = tipo;
                    eventosR.Add(evento);
                }
            }

            return eventosDoMesmoTipo;
        }

        // PUT: api/Eventos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        //[Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> PutEvento(int id, Evento evento)
        {
            if (id != evento.Id)
            {
                return BadRequest();
            } 

            _context.Entry(evento).State = EntityState.Modified;

            Local local = _context.Locais.Find(evento.LocalId);
            local.NomeLocal = evento.Local.NomeLocal;
            local.Morada = evento.Local.Morada;
            local.Localidade = evento.Local.Localidade;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Eventos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        //[Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult<Evento>> PostEvento(Evento evento)
        {

            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();


            return CreatedAtAction("GetEvento", new { id = evento.Id }, evento);
        }

        // DELETE: api/Eventos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvento(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null)
            {
                return NotFound();
            }

            _context.Eventos.Remove(evento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventoExists(int id)
        {
            return _context.Eventos.Any(e => e.Id == id);
        }
    }
}
