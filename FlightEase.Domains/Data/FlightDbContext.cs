using System;
using System.Collections.Generic;
using FlightEase.Domains.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlightEase.Domains.Data;

public partial class FlightDbContext : DbContext
{
    public FlightDbContext()
    {
    }

    public FlightDbContext(DbContextOptions<FlightDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Airport> Airports { get; set; }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<ClassType> ClassTypes { get; set; }

    public virtual DbSet<Flight> Flights { get; set; }

    public virtual DbSet<Meal> Meals { get; set; }

    public virtual DbSet<Season> Seasons { get; set; }

    public virtual DbSet<Seat> Seats { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<Transfer> Transfers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQL22_VIVES; Database=FlightEaseDB ;Trusted_Connection=True; TrustServerCertificate=True;MultipleActiveResultSets=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Airport>(entity =>
        {
            entity.HasKey(e => e.AirportId).HasName("PK_Airports");

            entity.ToTable("Airport");

            entity.Property(e => e.AirportId).HasColumnName("AirportID");
            entity.Property(e => e.CityId).HasColumnName("CityID");

            entity.HasOne(d => d.City).WithMany(p => p.Airports)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Airport_City");
        });

        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.Name).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK_Bookings");

            entity.ToTable("Booking");

            entity.Property(e => e.BookingId).HasColumnName("BookingID");
            entity.Property(e => e.BookingDate).HasColumnType("datetime");
            entity.Property(e => e.BookingName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TicketId).HasColumnName("TicketID");
            entity.Property(e => e.UserId)
                .HasMaxLength(450)
                .HasColumnName("UserID");

            entity.HasOne(d => d.Ticket).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.TicketId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Booking_Ticket");

            entity.HasOne(d => d.User).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Booking_AspNetUsers1");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.ToTable("City");

            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.CityName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ClassType>(entity =>
        {
            entity.ToTable("ClassType");

            entity.Property(e => e.ClassTypeId).HasColumnName("ClassTypeID");
            entity.Property(e => e.ClassName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Flight>(entity =>
        {
            entity.HasKey(e => e.FlightId).HasName("PK_Flights");

            entity.ToTable("Flight");

            entity.Property(e => e.FlightId).HasColumnName("FlightID");
            entity.Property(e => e.ArrivalTime).HasColumnType("datetime");
            entity.Property(e => e.DepartureTime).HasColumnType("datetime");
            entity.Property(e => e.FromAirportId).HasColumnName("FromAirportID");
            entity.Property(e => e.GateName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ToAirportId).HasColumnName("ToAirportID");
            entity.Property(e => e.TransferId).HasColumnName("TransferID");

            entity.HasOne(d => d.FromAirport).WithMany(p => p.FlightFromAirports)
                .HasForeignKey(d => d.FromAirportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Flight_Airport3");

            entity.HasOne(d => d.ToAirport).WithMany(p => p.FlightToAirports)
                .HasForeignKey(d => d.ToAirportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Flight_Airport4");

            entity.HasOne(d => d.Transfer).WithMany(p => p.Flights)
                .HasForeignKey(d => d.TransferId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Flight_Transfer");
        });

        modelBuilder.Entity<Meal>(entity =>
        {
            entity.HasKey(e => e.MealId).HasName("PK_Meals");

            entity.ToTable("Meal");

            entity.Property(e => e.MealId).HasColumnName("MealID");
            entity.Property(e => e.MealDescription)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.MealName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Season>(entity =>
        {
            entity.ToTable("Season");

            entity.Property(e => e.SeasonId).HasColumnName("SeasonID");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Seat>(entity =>
        {
            entity.ToTable("Seat");

            entity.Property(e => e.SeatId).HasColumnName("SeatID");
            entity.Property(e => e.SeatNumber).HasMaxLength(50);
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.ToTable("Ticket");

            entity.Property(e => e.TicketId).HasColumnName("TicketID");
            entity.Property(e => e.ClassTypeId).HasColumnName("ClassTypeID");
            entity.Property(e => e.FlightId).HasColumnName("FlightID");
            entity.Property(e => e.MealId).HasColumnName("MealID");
            entity.Property(e => e.SeasonId).HasColumnName("SeasonID");
            entity.Property(e => e.SeatId).HasColumnName("SeatID");
            entity.Property(e => e.SeatNumber).HasMaxLength(50);

            entity.HasOne(d => d.ClassType).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.ClassTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_ClassType");

            entity.HasOne(d => d.Flight).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.FlightId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_Flight");

            entity.HasOne(d => d.Meal).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.MealId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_Meal");

            entity.HasOne(d => d.Season).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.SeasonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_Season");

            entity.HasOne(d => d.Seat).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.SeatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_Seat");
        });

        modelBuilder.Entity<Transfer>(entity =>
        {
            entity.ToTable("Transfer");

            entity.Property(e => e.TransferId)
                .ValueGeneratedNever()
                .HasColumnName("TransferID");
            entity.Property(e => e.FirstAirportId).HasColumnName("FirstAirportID");
            entity.Property(e => e.SecondAirportId).HasColumnName("SecondAirportID");

            entity.HasOne(d => d.FirstAirport).WithMany(p => p.TransferFirstAirports)
                .HasForeignKey(d => d.FirstAirportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transfer_Airport");

            entity.HasOne(d => d.SecondAirport).WithMany(p => p.TransferSecondAirports)
                .HasForeignKey(d => d.SecondAirportId)
                .HasConstraintName("FK_Transfer_Airport1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
