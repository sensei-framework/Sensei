using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Sensei.AspNet
{
    /// <summary>
    /// A DbContext with pre/post processing capabilities. This kind of DbContext allow to use attributes
    /// with value manipulation or custom actions over the model properties.
    /// </summary>
    public class SenseiDbContext : DbContext
    {
        private readonly IServiceProvider _serviceProvider;

        public SenseiDbContext()
        {
            
        }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options">The options for this context</param>
        /// <param name="serviceProvider">Instance of service provider</param>
        public SenseiDbContext(DbContextOptions options, IServiceProvider serviceProvider)
            : base(options)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess"></param>
        /// <returns></returns>
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            DbProcessor.DbProcessor.PreProcess(this, _serviceProvider);
            var result = base.SaveChanges(acceptAllChangesOnSuccess);
            DbProcessor.DbProcessor.PostProcess(this, _serviceProvider);
            
            return result;
        }

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            DbProcessor.DbProcessor.PreProcess(this, _serviceProvider);
            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            DbProcessor.DbProcessor.PostProcess(this, _serviceProvider);
            
            return result;
        }
    }
}