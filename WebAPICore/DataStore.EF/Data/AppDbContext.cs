using Core.Models;
using Core.Models.Chats;
using Core.Models.EmailSender;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DataStore.EF.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<BPRPPhoneNumber> BPRPPhoneNumbers { get; set; }
        public DbSet<BPResponsiblePerson> BPResponsiblePeople { get; set; }
        public DbSet<BusinessPartner> BusinessPartners { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<BPRPEmail> BPRPEmails { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<SCRPPhoneNumber> SCRPPhoneNumbers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }
        public DbSet<RequestedProduct> RequestedProducts { get; set; }
        public DbSet<ResponsiblePerson> ResponsiblePeople { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<SupplierCompany> SupplierCompanies { get; set; }
        public DbSet<SCRPEmail> SCRPEmails { get; set; }
        public DbSet<SupplierCompanyTag> SupplierCompanyTags { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<Request> Requests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductTag>()
                .HasKey(bc => new { bc.ProductId, bc.TagId });
            modelBuilder.Entity<ProductTag>()
                .HasOne(bc => bc.Product)
                .WithMany(b => b.ProductTags)
                .HasForeignKey(bc => bc.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ProductTag>()
                .HasOne(bc => bc.Tag)
                .WithMany(c => c.ProductTags)
                .HasForeignKey(bc => bc.TagId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SupplierCompanyTag>()
                .HasKey(bc => new { bc.SupplierCompanyId, bc.TagId });
            modelBuilder.Entity<SupplierCompanyTag>()
                .HasOne(bc => bc.SupplierCompany)
                .WithMany(b => b.SupplierCompanyTags)
                .HasForeignKey(bc => bc.SupplierCompanyId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<SupplierCompanyTag>()
                .HasOne(bc => bc.Tag)
                .WithMany(c => c.SupplierCompanyTags)
                .HasForeignKey(bc => bc.TagId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Offer>()
              .HasOne(p => p.Payment)
              .WithOne(t => t.Offer)
              .HasForeignKey<Offer>(x => x.PaymentId)
              .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Payment>()
              .HasOne(p => p.Offer)
              .WithOne(t => t.Payment)
              .HasForeignKey<Payment>(x => x.OfferId)
              .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ResponsiblePerson>()
              .HasMany(x => x.Emails)
              .WithOne(p => p.ResponsiblePerson)
              .HasForeignKey(x => x.ResponsiblePersonId)
              .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SupplierCompany>()
              .HasMany(x => x.Offers)
              .WithOne(p => p.SupplierCompany)
              .HasForeignKey(x => x.SupplierCompanyId)
              .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<BPResponsiblePerson>()
              .HasMany(x => x.Emails)
              .WithOne(p => p.BPResponsiblePerson)
              .HasForeignKey(x => x.BPResponsiblePersonId)
              .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<BusinessPartner>()
              .HasMany(x => x.Requests)
              .WithOne(p => p.BusinessPartner)
              .HasForeignKey(x => x.BusinessPartnerId);

            modelBuilder.Entity<Request>()
             .HasOne(x => x.BusinessPartner)
             .WithMany(p => p.Requests)
             .HasForeignKey(x => x.BusinessPartnerId)
             .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Request>()
             .HasMany(x => x.RequestedProducts)
             .WithOne(p => p.Request)
             .HasForeignKey(x => x.RequestId)
             .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Request>()
             .HasMany(x => x.Chats)
             .WithOne(p => p.Request)
             .HasForeignKey(x => x.RequestId)
             .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Chat>()
             .HasOne(x => x.Request)
             .WithMany(p => p.Chats)
             .HasForeignKey(x => x.RequestId);


            modelBuilder.Entity<Product>()
              .HasMany(x => x.RequestedProducts)
              .WithOne(p => p.Product)
              .HasForeignKey(x => x.RequestId)
              .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RequestedProduct>()
              .HasMany(x => x.Offers)
              .WithOne(p => p.RequestedProduct)
              .HasForeignKey(x => x.RequestedProductId)
              .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SupplierCompany>()
              .HasMany(x => x.ResponsiblePeople)
              .WithOne(p => p.SupplierCompany)
              .HasForeignKey(x => x.SupplierCompanyId)
              .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SupplierCompany>()
             .HasMany(x => x.Chats)
             .WithOne(p => p.SupplierCompany)
             .HasForeignKey(x => x.SupplierCompanyId)
             .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Chat>()
             .HasOne(x => x.SupplierCompany)
             .WithMany(p => p.Chats)
             .HasForeignKey(x => x.SupplierCompanyId);

            modelBuilder.Entity<ResponsiblePerson>()
             .HasMany(x => x.PhoneNumbers)
             .WithOne(p => p.ResponsiblePerson)
             .HasForeignKey(x => x.ResponsiblePersonId)
             .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Product>()
             .HasMany(x => x.RequestedProducts)
             .WithOne(p => p.Product)
             .HasForeignKey(x => x.ProductId)
             .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<BusinessPartner>()
             .HasMany(x => x.BPResponsiblePeople)
             .WithOne(p => p.BusinessPartner)
             .HasForeignKey(x => x.BusinessPartnerId)
             .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BPResponsiblePerson>()
             .HasMany(x => x.PhoneNumbers)
             .WithOne(p => p.BPResponsiblePerson)
             .HasForeignKey(x => x.BPResponsiblePersonId)
             .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Chat>()
             .HasMany(x => x.Messages)
             .WithOne(p => p.Chat)
             .HasForeignKey(x => x.ChatId)
             .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Message>()
             .HasOne(x => x.Chat)
             .WithMany(p => p.Messages)
             .HasForeignKey(x => x.ChatId);

            //seeding
            /*
               modelBuilder.Entity<Ticket>().HasData(
                new Ticket { Id = 1, Barcode = "Ticket 1" },
                new Ticket { Id = 2, Barcode = "Ticket 2" }
                );


            modelBuilder.Entity<Theatre>().HasData(
                new Theatre { Id = 1, Name = "Theatre 1", Description = "Test", Date = DateTime.Now.AddDays(2)},
                new Theatre { Id = 2, Name = "Theatre 2", Description = "Test" , Date = DateTime.Now.AddDays(3)}
                );
             */

            modelBuilder.Entity<Tag>().HasData(
                new Tag { Id = 1, Name = "tag1" }
                );
            modelBuilder.Entity<Tag>().HasData(
                new Tag { Id = 2, Name = "tag2" }
                );
            modelBuilder.Entity<Tag>().HasData(
                new Tag { Id = 3, Name = "tag3" }
                );
            modelBuilder.Entity<Tag>().HasData(
                new Tag { Id = 4, Name = "tag4" }
                );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "product1" }
                );
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 2, Name = "product2" }
                );

            modelBuilder.Entity<ProductTag>().HasData(
                new ProductTag { TagId = 1, ProductId = 1 }
                );
            modelBuilder.Entity<ProductTag>().HasData(
                new ProductTag { TagId = 2, ProductId = 2 }
                );
            modelBuilder.Entity<ProductTag>().HasData(
                new ProductTag { TagId = 3, ProductId = 2 }
                );

            modelBuilder.Entity<SupplierCompany>().HasData(
                new SupplierCompany { Id = 1, Adress = "sakarya", Email = "test@gmail.com", Gsm = "+905556661122", Name = "Fibu", Phone = null, Token = "asdsadsa" }
                );
            modelBuilder.Entity<SupplierCompany>().HasData(
                new SupplierCompany { Id = 2, Adress = "sakarya", Email = "test2@gmail.com", Gsm = "+905556661122", Name = "Fibu2", Phone = null, Token = "asdsadsa" }
                );
            modelBuilder.Entity<SupplierCompanyTag>().HasData(
                new SupplierCompanyTag { TagId = 1, SupplierCompanyId = 1 }
                );
            modelBuilder.Entity<SupplierCompanyTag>().HasData(
               new SupplierCompanyTag { TagId = 2, SupplierCompanyId = 1 }
               );

            modelBuilder.Entity<ResponsiblePerson>().HasData(
                new ResponsiblePerson { Id = 1, Name = "Yasin", SupplierCompanyId = 1 }
                );
            modelBuilder.Entity<ResponsiblePerson>().HasData(
               new ResponsiblePerson { Id = 2, Name = "Behlül", SupplierCompanyId = 1 }
               );
            modelBuilder.Entity<SCRPEmail>().HasData(
              new SCRPEmail { Id = 1, Email = "yasin@gmail.com", ResponsiblePersonId = 1 }
              );
            modelBuilder.Entity<SCRPEmail>().HasData(
             new SCRPEmail { Id = 2, Email = "yasin2@gmail.com", ResponsiblePersonId = 1 }
             );
            modelBuilder.Entity<SCRPEmail>().HasData(
             new SCRPEmail { Id = 3, Email = "behlül@gmail.com", ResponsiblePersonId = 2 }
             );
            modelBuilder.Entity<SCRPPhoneNumber>().HasData(
             new SCRPPhoneNumber { Id = 1, Gsm = "+90 555 444 33 22", ResponsiblePersonId = 1 }
             );
            modelBuilder.Entity<SCRPPhoneNumber>().HasData(
            new SCRPPhoneNumber { Id = 2, Gsm = "+90 555 444 33 22 new", ResponsiblePersonId = 1 }
            );
            modelBuilder.Entity<SCRPPhoneNumber>().HasData(
            new SCRPPhoneNumber { Id = 3, Gsm = "+90 555 444 33 22 beh", ResponsiblePersonId = 2 }
            );

            modelBuilder.Entity<BusinessPartner>().HasData(
                new BusinessPartner { Id = 1, Name = "Partner111", Email = "test@gg.com", Gsm = "555555", Adress = "adres1" }
                );
            modelBuilder.Entity<BusinessPartner>().HasData(
                new BusinessPartner { Id = 2, Name = "Partner222", Email = "test22@gg.com", Gsm = "4444444", Adress = "adres222" }
                );
            modelBuilder.Entity<BPResponsiblePerson>().HasData(
               new BPResponsiblePerson { Id = 1, Name = "Yasin", BusinessPartnerId = 1 }
               );
            modelBuilder.Entity<BPResponsiblePerson>().HasData(
               new BPResponsiblePerson { Id = 2, Name = "Behlül", BusinessPartnerId = 2 }
               );
            modelBuilder.Entity<BPRPEmail>().HasData(
              new BPRPEmail { Id = 1, Email = "yasin@gmail.com", BPResponsiblePersonId = 1 }
              );
            modelBuilder.Entity<BPRPEmail>().HasData(
             new BPRPEmail { Id = 2, Email = "yasin2@gmail.com", BPResponsiblePersonId = 1 }
             );
            modelBuilder.Entity<BPRPEmail>().HasData(
             new BPRPEmail { Id = 3, Email = "behlül@gmail.com", BPResponsiblePersonId = 2 }
             );
            modelBuilder.Entity<BPRPPhoneNumber>().HasData(
            new BPRPPhoneNumber { Id = 2, Gsm = "+90 555 444 33 22 new", BPResponsiblePersonId = 1 }
            );
            modelBuilder.Entity<BPRPPhoneNumber>().HasData(
            new BPRPPhoneNumber { Id = 3, Gsm = "+90 555 444 33 22 beh", BPResponsiblePersonId = 2 }
            );

            modelBuilder.Entity<Request>().HasData(
            new Request { Id = 1,Token="asdsadsadsa", BusinessPartnerId = 1 }
            );

            modelBuilder.Entity<RequestedProduct>().HasData(
                new RequestedProduct { Id = 1, RequestId = 1, Deadline = new DateTime(), ProductId = 1, Quantity = 100}
            );
            modelBuilder.Entity<RequestedProduct>().HasData(
                new RequestedProduct { Id = 2, RequestId = 1, Deadline = new DateTime(), ProductId = 2, Quantity = 2200}
            );

        }
    }
}
