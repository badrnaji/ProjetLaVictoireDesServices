using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ProjetLaVictoireDesServices.Models
{
    public partial class HelpDeskVictoireDBEntities : DbContext
    {
        public HelpDeskVictoireDBEntities()
        {
        }

        public HelpDeskVictoireDBEntities(DbContextOptions<HelpDeskVictoireDBEntities> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Compte> Comptes { get; set; }
        public virtual DbSet<Discussion> Discussions { get; set; }
        public virtual DbSet<Periode> Periodes { get; set; }
        public virtual DbSet<Photo> Photos { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Statut> Statuts { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasIndex(e => e.Nom, "IX_Categories")
                    .IsUnique();

                entity.Property(e => e.Nom)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Compte>(entity =>
            {
                entity.HasIndex(e => e.NomUtilisateur, "IX_Comptes_RoleId")
                    .IsUnique();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.NomComplet)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.NomUtilisateur)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Comptes)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<Discussion>(entity =>
            {
                entity.HasIndex(e => e.CompteId, "IX_Discussions_CompteId");

                entity.Property(e => e.Contenu).HasColumnType("text");

                entity.Property(e => e.CreerDate).HasColumnType("date");

                entity.HasOne(d => d.Compte)
                    .WithMany(p => p.Discussions)
                    .HasForeignKey(d => d.CompteId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Ticke)
                    .WithMany(p => p.Discussions)
                    .HasForeignKey(d => d.TickeId)
                    .HasConstraintName("FK_Discussions_Tickets");
            });

            modelBuilder.Entity<Periode>(entity =>
            {
                entity.Property(e => e.Couleur)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nom)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Photo>(entity =>
            {
                entity.HasIndex(e => e.TicketId, "IX_Photos_TicketId");

                entity.Property(e => e.Nom)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.Ticket)
                    .WithMany(p => p.Photos)
                    .HasForeignKey(d => d.TicketId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Nom)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Statut>(entity =>
            {
                entity.Property(e => e.Couleur)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nom)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.Display).HasColumnName("Display");
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasIndex(e => e.CategorieId, "IX_Tickets_CategorieId");

                entity.HasIndex(e => e.PeriodeId, "IX_Tickets_PeriodeId");

                entity.HasIndex(e => e.StatutId, "IX_Tickets_StatutId");

                entity.Property(e => e.CreerDate).HasColumnType("date");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.Categorie)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.CategorieId);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.TicketEmployees)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tickets_Comptes");

                entity.HasOne(d => d.Periode)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.PeriodeId);

                entity.HasOne(d => d.Statut)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.StatutId);

                entity.HasOne(d => d.Supporter)
                    .WithMany(p => p.TicketSupporters)
                    .HasForeignKey(d => d.SupporterId)
                    .HasConstraintName("FK_Tickets_Comptes1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
