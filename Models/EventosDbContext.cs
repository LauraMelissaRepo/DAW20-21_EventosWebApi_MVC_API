using EventosWebApi_v1.Models.Authentication;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventosWebApi_v1.Models
{
    public class EventosDbContext : IdentityDbContext<ApplicationUser>
    {
        public EventosDbContext(DbContextOptions<EventosDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Evento> Eventos { get; set; }
        public virtual DbSet<Local> Locais { get; set; }
        public virtual DbSet<Tipo> Tipos { get; set; }

    }
}
