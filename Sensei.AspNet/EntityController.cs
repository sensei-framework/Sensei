using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sensei.AspNet.Models;
using Sensei.AspNet.Queries;
using Sensei.AspNet.Queries.Entities;
using Sensei.AspNet.Queries.QueryFilters;
using Sensei.AspNet.Queries.QueryIncludes;
using Sensei.AspNet.Queries.QueryPagination;
using Sensei.AspNet.Queries.QuerySorts;
using Swashbuckle.AspNetCore.Annotations;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable VirtualMemberNeverOverridden.Global

namespace Sensei.AspNet
{
    /// <summary>
    ///     Provide an abstract base service for controllers
    /// </summary>
    /// <typeparam name="TModel">The type of the data model to handle</typeparam>
    /// <typeparam name="TDbContext">The type for the database context</typeparam>
    public abstract class EntityController<TModel, TDbContext> : ControllerBase
        where TModel : BaseModel
        where TDbContext : DbContext
    {
        /// <summary>
        ///     Controller constructor
        /// </summary>
        /// <param name="serviceProvider">Instance of service provider</param>
        protected EntityController(IServiceProvider serviceProvider)
        {
            // we use the service provider and not the DI in constructor because this can grow up in future
            // and we don't want to broke services that inherit this abstract class
            DbContext = serviceProvider.GetService<TDbContext>();
            QueryProcessor = serviceProvider.GetService<IQueryProcessor>();
            
            DbSet = DbContext.Set<TModel>();
        }

        /// <summary>
        ///     The database context
        /// </summary>
        protected TDbContext DbContext { get; }

        /// <summary>
        ///     The database set of data
        /// </summary>
        protected DbSet<TModel> DbSet { get; }

        /// <summary>
        ///     The query processor
        /// </summary>
        protected IQueryProcessor QueryProcessor { get; }

        /// <summary>
        ///     Get records
        /// </summary>
        /// <param name="filtering">The filtering data model</param>
        /// <param name="sorting">The sorting data model</param>
        /// <param name="pagination">The pagination data model</param>
        /// <param name="including">The including data model</param>
        /// <returns>The action result with read data</returns>
        [HttpGet]
        [SwaggerOperation(Summary = "Get records")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public virtual async Task<ActionResult<Paginator<TModel>>> Get(
            [FromQuery] Filtering filtering,
            [FromQuery] Sorting sorting,
            [FromQuery] Pagination pagination,
            [FromQuery] Including including
        )
        {
            return QueryProcessor.Start(DbSet)
                .ApplyFilters(filtering)
                .ApplySorting(sorting)
                .ApplyIncluding(including)
                .ApplyPagination(pagination);
        }

        /// <summary>
        ///     Get record by Id
        /// </summary>
        /// <param name="id">The record id</param>
        /// <param name="including">The including data model</param>
        /// <returns>The action result with read data</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get record by Id")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public virtual async Task<ActionResult<TModel>> Get(
            [FromRoute] [SwaggerParameter("The record Id", Required = true)]
            Guid id,
            [FromQuery] Including including
        )
        {
            var query =
                QueryProcessor.Start(DbSet)
                    .ApplyIncluding(including).Queryable;
            
            var result = await query.FirstOrDefaultAsync(e => e.Id == id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        ///     Create record
        /// </summary>
        /// <param name="entity">The data model</param>
        /// <returns>The action result with read data</returns>
        [HttpPost]
        [SwaggerOperation(Summary = "Create record")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public virtual async Task<ActionResult<TModel>> Post(
            [FromBody] TModel entity
        )
        {
            if (entity == null)
                return BadRequest();

            await DbSet.AddAsync(entity);
            await DbContext.SaveChangesAsync();
            return Ok(entity);
        }

        /// <summary>
        ///     Update a record
        /// </summary>
        /// <param name="id">The record id</param>
        /// <param name="entity">The data model</param>
        /// <returns>The action result with read data</returns>
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update record")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public virtual async Task<ActionResult<TModel>> Put(
            [FromRoute] [SwaggerParameter("The record Id", Required = true)]
            Guid id,
            [FromBody] TModel entity
        )
        {
            var result = await DbSet.FirstOrDefaultAsync(e => e.Id == id);
            if (result == null)
                return NotFound();

            entity.CopyTo(result);
            DbSet.Update(result);
            await DbContext.SaveChangesAsync();

            return Ok(result);
        }

        /// <summary>
        ///     Delete a record
        /// </summary>
        /// <param name="id">The record id</param>
        /// <returns>The action result</returns>
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a record")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public virtual async Task<ActionResult> Delete(
            [FromRoute] [SwaggerParameter("The record Id", Required = true)]
            Guid id
        )
        {
            var result = await DbSet.FirstOrDefaultAsync(e => e.Id == id);
            if (result == null)
                return NotFound();

            DbSet.Remove(result);
            await DbContext.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        ///     Count the records
        /// </summary>
        /// <param name="filtering">The filtering data model</param>
        /// <returns>The action result with the count result</returns>
        [HttpGet("count")]
        [SwaggerOperation(Summary = "Count records")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public virtual async Task<ActionResult<SingleValue<long>>> Count(
            [FromQuery] Filtering filtering
        )
        {
            var query =
                QueryProcessor.Start(DbSet)
                    .ApplyFilters(filtering).Queryable;

            return Ok(new SingleValue<long> {Value = await query.CountAsync()});
        }
    }
}