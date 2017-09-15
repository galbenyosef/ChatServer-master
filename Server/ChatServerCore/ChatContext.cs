using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ChatServerCore
{
    public class ChatContext : DbContext
    {
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Message> Messages { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

        }
    }

}

