using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventosWebApi_v1.Models;

namespace EventosWebApi_v1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            return await _context.Eventos.Include(y => y.Tipo).Include(x => x.Local).ToListAsync();
        }

        // GET: api/Eventos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Evento>> GetEvento(int id)
        {

            var evento = await _context.Eventos.FindAsync(id);

            var local = _context.Locais.FindAsync(evento.LocalId);
            var tipo = _context.Tipos.FindAsync(evento.TipoId);

            evento.Local = await local;
            evento.Tipo = await tipo;

            if (evento == null)
            {
                return NotFound();
            }
            return evento;
        }

        [Route("tipo/{tipoEventoId}/evento/{localId}/local/{data}/data")]
        [HttpGet]
        public async Task<ActionResult<List<Evento>>> GetEventosFilter(int tipoEventoId, int localId, DateTime data)
        {
            //guardar na variavel quais os eventos que
            //têm o mesmo tipoEventoId. ex: guardo todos os eventos do tipo dança

            var eventosDoMesmoTipo = _context.Eventos.Where(et => et.TipoId == tipoEventoId)
                .Where(el => el.LocalId == localId);

            var mes = data.Date.Month; //vou buscar o mês recebido

            List<Evento> eventosR = new List<Evento>();
            foreach (Evento evento in eventosDoMesmoTipo)
            {
                if (evento.Data.Month == mes)
                {
                    eventosR.Add(evento);
                }
            }
            return eventosR;
        }

        // PUT: api/Eventos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvento(int id, Evento evento)
        {
            if (id != evento.Id)
            {
                return BadRequest();
            }

            _context.Entry(evento).State = EntityState.Modified;          

            _context.Locais.Update(evento.Local);
            _context.Tipos.Update(evento.Tipo);

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
