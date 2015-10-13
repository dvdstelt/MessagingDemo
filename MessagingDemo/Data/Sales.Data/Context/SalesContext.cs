using Common.Logging;
using Sales.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.Data.Context
{
	public interface ISalesContext : IDisposable
	{
		IDbSet<Product> Products { get; set; }
    IDbSet<OrderedItem> OrderedItems { get; set; }

		/// <summary>
		/// Stores changes in the database
		/// </summary>
		/// <returns>Number of rows changed</returns>
		int SaveChanges();
	}
	public class SalesContext : DbContext, ISalesContext
	{
		private static readonly ILog Log = LogManager.GetLogger<SalesContext>();

		public IDbSet<Product> Products { get; set; }
    public IDbSet<OrderedItem> OrderedItems { get; set; }

    /// <summary>
    /// Constructor
    /// </summary>
    public SalesContext() : base("SalesDatabase")
		{
			Database.SetInitializer<SalesContext>(null);
			this.Configuration.LazyLoadingEnabled = false;
		}

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Stores changes in the database
    /// </summary>
    /// <returns>Number of rows changed</returns>
    public override int SaveChanges()
		{
			try
			{
				return base.SaveChanges();
			}
			catch (DbEntityValidationException ex)
			{
				var message = new StringBuilder();
				message.AppendLine("Database validation error(s):");
				foreach (var error in ex.EntityValidationErrors)
				{
					message.AppendFormat("\t{0}", error.Entry.Entity).AppendLine();
					foreach (var i in error.ValidationErrors)
					{
						message.AppendFormat("\t\t{0} => {1}", i.PropertyName, i.ErrorMessage).AppendLine();
					}
				}
				Log.Warn(message);
				throw;
			}
		}
	}
}
