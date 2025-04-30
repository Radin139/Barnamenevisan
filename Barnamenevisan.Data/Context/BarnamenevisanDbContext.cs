using Barnamenevisan.Domain.Models.Auth;
using Microsoft.EntityFrameworkCore;

namespace Barnamenevisan.Data.Context;

public class BarnamenevisanDbContext:DbContext
{
    #region DbSets

    public DbSet<User> Users { get; set; }

    #endregion

    #region Constructor

    public BarnamenevisanDbContext(DbContextOptions<BarnamenevisanDbContext> options)
        : base(options)
    {
    }

    #endregion
}