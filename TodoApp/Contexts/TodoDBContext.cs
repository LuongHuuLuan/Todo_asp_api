using Microsoft.EntityFrameworkCore;
using TodoApp.Models;
using TodoApp.Models.JWT;
using TodoApp.Models.Test;
using TodoApp.Models.Todos;
using TodoApp.Models.User;

namespace TodoApp.Contexts
{
    public class TodoDBContext : DbContext
    {
        public DbSet<TodoList> TodoList { get; set; }
        public DbSet<TodoItem> TodoItems { get; set; }

        public DbSet<Tag> Tags { get; set; }
        public DbSet<Status> Status { get; set; }

        public DbSet<People> Person { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Account> Accounts { get; set; }

        public DbSet<Parent> Parents { get; set; }
        public DbSet<Children> Childrens { get; set; }
        public DbSet<OutstandingToken> OutstandingTokens { get; set; }
        public DbSet<BlacklistedToken> BlacklistedTokens { get; set; }

        public TodoDBContext(DbContextOptions<TodoDBContext> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            modelbuilder.Entity<People>()
                .HasOne(e => e.Account)
                .WithOne(e => e.People)
                .HasForeignKey<Account>(e => e.PeopleId)
                .IsRequired(false);

            modelbuilder.Entity<People>()
               .HasOne(e => e.Contact)
               .WithOne(e => e.People)
               .HasForeignKey<Contact>(e => e.PeopleId)
               .IsRequired(false);

            modelbuilder.Entity<Account>()
                .HasMany(e => e.TodoLists)
                .WithOne(e => e.Account)
                .HasForeignKey(e => e.AccountId)
                .IsRequired();

            modelbuilder.Entity<TodoList>()
               .HasMany(e => e.TodoItems)
               .WithOne(e => e.TodoList)
               .HasForeignKey(e => e.TodoListId)
               .IsRequired();

            modelbuilder.Entity<TodoItem>()
                .HasMany(e => e.Tags)
                .WithMany(e => e.TodoItems)
                .UsingEntity("TodoItemTag",
                                l => l.HasOne(typeof(Tag)).WithMany().HasForeignKey("TagId").HasPrincipalKey(nameof(Tag.Id)),
                                r => r.HasOne(typeof(TodoItem)).WithMany().HasForeignKey("TodoItemId").HasPrincipalKey(nameof(TodoItem.Id)),
                                j => j.HasKey("TagId", "TodoItemId"));

            modelbuilder.Entity<Status>()
                .HasMany(e => e.TodoItems)
                .WithOne(e => e.Status)
                .HasForeignKey(e => e.StatusId)
                .IsRequired();

            //modelbuilder.Entity<OutstandingToken>()
            //    .HasOne(e => e.Account)
            //    .WithMany(e => e.outstandingTokens)
            //    .HasForeignKey(e => e.AccountId)
            //    .IsRequired(false);
        }
    }
}
