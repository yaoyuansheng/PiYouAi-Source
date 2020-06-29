// ***********************************************************************
// Assembly         : Eason.Domain
// Author           : yaoyuansheng
// Created          : 03-03-2017
//
// Last Modified By : yaoyuansheng
// Last Modified On : 03-03-2017
// ***********************************************************************
// <copyright file="RepositoryBase.cs" company="Microsoft">
//     Copyright © Microsoft 2017
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Eason.Domain.Entities;
using System.Data.Entity;
using Eason.Domain.Uow;
using Eason.Extension;

namespace Eason.Domain.Repositories
{
    /// <summary>
    /// Base class to implement <see cref="IRepository{TEntity,TPrimaryKey}" />.
    /// It implements some methods in most simple way.
    /// </summary>
    /// <typeparam name="TEntity">Type of the Entity for this repository</typeparam>
    /// <typeparam name="TPrimaryKey">Primary key of the entity</typeparam>
    /// <seealso cref="Eason.Domain.Repositories.IRepository{TEntity, TPrimaryKey}" />
    public abstract class RepositoryBase<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        #region 构造函数
        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>The context.</value>
       // public abstract IUnitOfWork UnitOfWork { get; set; }
        public abstract DbContext Context { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase{TEntity, TPrimaryKey}"/> class.
        /// </summary>
        /// <param name="_context">The _context.</param>
        public RepositoryBase()
        {

        }
        #endregion

        #region Select
        /// <summary>
        /// Used to get a IQueryable that is used to retrieve entities from entire table.
        /// <see cref="!:UnitOfWorkAttribute" /> attribute must be used to be able to call this method since this method
        /// returns IQueryable and it requires open database connection to use it.
        /// </summary>
        /// <returns>IQueryable to be used to select entities from database</returns>
        public virtual IQueryable<TEntity> GetAll()
        {
            return Context.Set<TEntity>().AsQueryable();
        }

        /// <summary>
        /// Used to get all entities.
        /// </summary>
        /// <returns>List of all entities</returns>
        public virtual IList<TEntity> GetAllList()
        {
            return Context.Set<TEntity>().ToList();
        }

        /// <summary>
        /// get all list as an asynchronous operation.
        /// </summary>
        /// <returns>List of all entities</returns>
        public virtual async Task<List<TEntity>> GetAllListAsync()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }

        /// <summary>
        /// Gets all list.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        public virtual IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate);
        }
        public virtual IQueryable<TEntity> GetAllList(int offset, int limit)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            if (limit <= 0)
            {
                limit = 10;
            }
            return Context.Set<TEntity>().OrderByDescending(m => m.id).Skip(offset*limit).Take(limit);
        }
        public virtual async Task<IQueryable<TEntity>> GetAllListAsync(int offset, int limit)
        {
            return await Task.FromResult(GetAllList(offset, limit));
        }
        public virtual IQueryable<TEntity> GetAllList(int offset, int limit, Expression<Func<TEntity, bool>> predicate)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            if (limit <= 0)
            {
                limit = 10;
            }
            return Context.Set<TEntity>().Where(predicate).OrderByDescending(m => m.id).Skip(offset*limit).Take(limit);
        }
       
        public virtual IQueryable<TEntity> GetAllList(int offset, int limit, Expression<Func<TEntity, long>> predicate, long[] list)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            if (limit <= 0)
            {
                limit = 10;
            }
            return Context.Set<TEntity>().WhereIn(predicate, list).OrderByDescending(m => m.id).Skip(offset*limit).Take(limit);
        }
       

        public virtual IQueryable<TEntity> GetAllList(int offset, int limit, Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, long>> predicate, long[] list)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            if (limit <= 0)
            {
                limit = 10;
            }
            return Context.Set<TEntity>().Where(where).WhereIn(predicate, list).OrderByDescending(m => m.id).Skip(offset*limit).Take(limit);
        }

        public virtual IQueryable<TEntity> GetAllList(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, long>> predicate, long[] list)
        {
            return Context.Set<TEntity>().Where(where).WhereIn(predicate, list).OrderByDescending(m => m.id);
        }
        public virtual async Task<IQueryable<TEntity>> GetAllListAsync(int offset, int limit, Expression<Func<TEntity, bool>> predicate)
        {
            return await Task.FromResult(GetAllList(offset, limit, predicate));
        }


        /// <summary>
        /// get all list as an asynchronous operation.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Task&lt;IList&lt;TEntity&gt;&gt;.</returns>
        public async virtual Task<IList<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll(predicate).ToListAsync();
        }

        /// <summary>
        /// Used to run a query over entire entities.
        /// <see cref="!:UnitOfWorkAttribute" /> attribute is not always necessary (as opposite to <see cref="M:Eason.Domain.Repositories.IRepository`2.GetAll" />)
        /// if <paramref name="queryMethod" /> finishes IQueryable with ToList, FirstOrDefault etc..
        /// </summary>
        /// <typeparam name="T">Type of return value of this method</typeparam>
        /// <param name="queryMethod">This method is used to query over entities</param>
        /// <returns>Query result</returns>
        public virtual T Query<T>(Func<IQueryable<TEntity>, T> queryMethod)
        {
            return queryMethod(GetAll());
        }
        #endregion

        #region Single
        /// <summary>
        /// Gets an entity with given primary key.
        /// </summary>
        /// <param name="id">Primary key of the entity to get</param>
        /// <returns>Entity</returns>
        /// <exception cref="System.NullReferenceException">There is no such an entity with given primary key. Entity type:  + typeof(TEntity).FullName + , primary key:  + id</exception>
        public virtual TEntity Get(TPrimaryKey id)
        {
            var entity = FirstOrDefault(id);
            if (entity == null)
            {
                throw new NullReferenceException("There is no such an entity with given primary key. Entity type: " + typeof(TEntity).FullName + ", primary key: " + id);
            }

            return entity;
        }

        /// <summary>
        /// get as an asynchronous operation.
        /// </summary>
        /// <param name="id">Primary key of the entity to get</param>
        /// <returns>Entity</returns>
        /// <exception cref="System.NullReferenceException">There is no such an entity with given primary key. Entity type:  + typeof(TEntity).FullName + , primary key:  + id</exception>
        public virtual async Task<TEntity> GetAsync(TPrimaryKey id)
        {
            var entity = await FirstOrDefaultAsync(id);
            if (entity == null)
            {
                throw new NullReferenceException("There is no such an entity with given primary key. Entity type: " + typeof(TEntity).FullName + ", primary key: " + id);
            }

            return entity;
        }

        /// <summary>
        /// Gets exactly one entity with given predicate.
        /// Throws exception if no entity or more than one entity.
        /// </summary>
        /// <param name="predicate">Entity</param>
        /// <returns>TEntity.</returns>
        public virtual TEntity Single(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Single(predicate);
        }

        /// <summary>
        /// single as an asynchronous operation.
        /// </summary>
        /// <param name="predicate">Entity</param>
        /// <returns>Task&lt;TEntity&gt;.</returns>
        public async virtual Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Context.Set<TEntity>().SingleAsync(predicate);
        }

        /// <summary>
        /// Gets an entity with given primary key or null if not found.
        /// </summary>
        /// <param name="id">Primary key of the entity to get</param>
        /// <returns>Entity or null</returns>
        public virtual TEntity FirstOrDefault(TPrimaryKey id)
        {
            return Context.Set<TEntity>().FirstOrDefault(CreateEqualityExpressionForId(id));
        }

        /// <summary>
        /// first or default as an asynchronous operation.
        /// </summary>
        /// <param name="id">Primary key of the entity to get</param>
        /// <returns>Entity or null</returns>
        public async virtual Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id)
        {
            return await Context.Set<TEntity>().FirstOrDefaultAsync(CreateEqualityExpressionForId(id));
        }

        /// <summary>
        /// Gets an entity with given given predicate or null if not found.
        /// </summary>
        /// <param name="predicate">Predicate to filter entities</param>
        /// <returns>TEntity.</returns>
        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().FirstOrDefault(predicate);
        }

        /// <summary>
        /// first or default as an asynchronous operation.
        /// </summary>
        /// <param name="predicate">Predicate to filter entities</param>
        /// <returns>Task&lt;TEntity&gt;.</returns>
        public async virtual Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        /// <summary>
        /// Creates an entity with given primary key without database access.
        /// </summary>
        /// <param name="id">Primary key of the entity to load</param>
        /// <returns>Entity</returns>
        public virtual TEntity Load(TPrimaryKey id)
        {
            return Get(id);
        }
        #endregion

        #region Insert
        /// <summary>
        /// Inserts a new entity.
        /// </summary>
        /// <param name="entity">Inserted entity</param>
        /// <returns>TEntity.</returns>
        public virtual TEntity Insert(TEntity entity)
        {
            return Context.Set<TEntity>().Add(entity);
        }

        /// <summary>
        /// insert as an asynchronous operation.
        /// </summary>
        /// <param name="entity">Inserted entity</param>
        /// <returns>Task&lt;TEntity&gt;.</returns>
        public virtual async Task<TEntity> InsertAsync(TEntity entity)
        {
            return await Task.FromResult(Insert(entity));
        }

        /// <summary>
        /// Inserts a new entity and gets it's Id.
        /// It may require to save current unit of work
        /// to be able to retrieve id.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Id of the entity</returns>
        public virtual TPrimaryKey InsertAndGetId(TEntity entity)
        {
            return Insert(entity).id;
        }

        /// <summary>
        /// Inserts a new entity and gets it's Id.
        /// It may require to save current unit of work
        /// to be able to retrieve id.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Id of the entity</returns>
        public virtual async Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity)
        {
            return await Task.FromResult(InsertAndGetId(entity));
        }
        #endregion

        #region InsertOrUpdate
        /// <summary>
        /// Inserts or updates given entity depending on Id's value.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>TEntity.</returns>
        public virtual TEntity InsertOrUpdate(TEntity entity)
        {
            return entity.IsTransient()
                ? Insert(entity)
                : Update(entity);
        }

        /// <summary>
        /// insert or update as an asynchronous operation.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Task&lt;TEntity&gt;.</returns>
        public virtual async Task<TEntity> InsertOrUpdateAsync(TEntity entity)
        {
            return entity.IsTransient()
                ? await InsertAsync(entity)
                : await UpdateAsync(entity);
        }

        /// <summary>
        /// Inserts or updates given entity depending on Id's value.
        /// Also returns Id of the entity.
        /// It may require to save current unit of work
        /// to be able to retrieve id.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Id of the entity</returns>
        public virtual TPrimaryKey InsertOrUpdateAndGetId(TEntity entity)
        {
            return InsertOrUpdate(entity).id;
        }

        /// <summary>
        /// insert or update and get identifier as an asynchronous operation.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Id of the entity</returns>
        public virtual async Task<TPrimaryKey> InsertOrUpdateAndGetIdAsync(TEntity entity)
        {
            return await Task.FromResult(InsertOrUpdateAndGetId(entity));
        }
        #endregion

        #region Update
        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>TEntity.</returns>
        public virtual TEntity Update(TEntity entity)
        {
            Context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            return entity;
        }
        /// <summary>
        /// update as an asynchronous operation.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Task&lt;TEntity&gt;.</returns>
        public async virtual Task<TEntity> UpdateAsync(TEntity entity)
        {
            return await Task.FromResult(Update(entity));
        }

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="id">Id of the entity</param>
        /// <param name="updateAction">Action that can be used to change values of the entity</param>
        /// <returns>Updated entity</returns>
        public virtual TEntity Update(TPrimaryKey id, Action<TEntity> updateAction)
        {
            var entity = Get(id);
            updateAction(entity);
            return entity;
        }

        /// <summary>
        /// update as an asynchronous operation.
        /// </summary>
        /// <param name="id">Id of the entity</param>
        /// <param name="updateAction">Action that can be used to change values of the entity</param>
        /// <returns>Updated entity</returns>
        public virtual async Task<TEntity> UpdateAsync(TPrimaryKey id, Func<TEntity, Task> updateAction)
        {
            var entity = await GetAsync(id);
            await updateAction(entity);
            return entity;
        }

        #endregion


        #region Delete
        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>System.Int32.</returns>
        public virtual int Delete(TEntity entity)
        {
            Context.Entry<TEntity>(entity).State = EntityState.Deleted;
            return 1;
        }
        /// <summary>
        /// delete as an asynchronous operation.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        public virtual async Task<int> DeleteAsync(TEntity entity)
        {
            return await Task.FromResult<int>(Delete(entity));
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public virtual int Delete(TPrimaryKey id)
        {
            var entity = Context.Set<TEntity>().FirstOrDefault(CreateEqualityExpressionForId(id));
            if (entity != null)
            {
                Context.Entry<TEntity>(entity).State = EntityState.Deleted;
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// delete as an asynchronous operation.
        /// </summary>
        /// <param name="id">Primary key of the entity</param>
        /// <returns>Task.</returns>
        public virtual async Task DeleteAsync(TPrimaryKey id)
        {
            var entity = await Context.Set<TEntity>().FirstOrDefaultAsync(CreateEqualityExpressionForId(id));
            if (entity != null)
            {
                Context.Entry<TEntity>(entity).State = EntityState.Deleted;
            }
        }

        /// <summary>
        /// Deletes the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>System.Int32.</returns>
        public virtual int Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var lst = Context.Set<TEntity>().Where(predicate);
            foreach (var entity in lst)
            {
                Delete(entity);
            }
            return lst.Count();
        }

        /// <summary>
        /// delete as an asynchronous operation.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        public virtual async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var lst = Context.Set<TEntity>().Where(predicate);
            foreach (var entity in lst)
            {
                await DeleteAsync(entity);
            }
            return await lst.CountAsync();
        }
        #endregion

        #region Count
        /// <summary>
        /// Gets count of all entities in this repository.
        /// </summary>
        /// <returns>Count of entities</returns>
        public virtual int Count()
        {
            return Context.Set<TEntity>().Count();
        }

        /// <summary>
        /// count as an asynchronous operation.
        /// </summary>
        /// <returns>Count of entities</returns>
        public virtual async Task<int> CountAsync()
        {
            return await Context.Set<TEntity>().CountAsync();
        }

        /// <summary>
        /// Gets count of all entities in this repository based on given <paramref name="predicate" />.
        /// </summary>
        /// <param name="predicate">A method to filter count</param>
        /// <returns>Count of entities</returns>
        public virtual int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Count(predicate);
        }
        public virtual int Count(Expression<Func<TEntity, long>> predicate, long[] list)
        {
            return Context.Set<TEntity>().WhereIn(predicate, list).Count();
        }
        public virtual int Count(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, long>> predicate, long[] list)
        {
            return Context.Set<TEntity>().Where(where).WhereIn(predicate, list).Count();
        }
        /// <summary>
        /// Gets count of all entities in this repository based on given <paramref name="predicate" />.
        /// </summary>
        /// <param name="predicate">A method to filter count</param>
        /// <returns>Count of entities</returns>
        public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().CountAsync(predicate);
        }

        /// <summary>
        /// Gets count of all entities in this repository (use if expected return value is greather than <see cref="F:System.Int32.MaxValue" />.
        /// </summary>
        /// <returns>Count of entities</returns>
        public virtual long LongCount()
        {
            return Context.Set<TEntity>().LongCount();
        }

        /// <summary>
        /// long count as an asynchronous operation.
        /// </summary>
        /// <returns>Count of entities</returns>
        public virtual async Task<long> LongCountAsync()
        {
            return await Context.Set<TEntity>().LongCountAsync();
        }

        /// <summary>
        /// Gets count of all entities in this repository based on given <paramref name="predicate" />
        /// (use this overload if expected return value is greather than <see cref="F:System.Int32.MaxValue" />).
        /// </summary>
        /// <param name="predicate">A method to filter count</param>
        /// <returns>Count of entities</returns>
        public virtual long LongCount(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().LongCount(predicate);
        }

        /// <summary>
        /// long count as an asynchronous operation.
        /// </summary>
        /// <param name="predicate">A method to filter count</param>
        /// <returns>Count of entities</returns>
        public virtual async Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Context.Set<TEntity>().LongCountAsync(predicate);
        }
        #endregion

        /// <summary>
        /// Creates the equality expression for identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Expression&lt;Func&lt;TEntity, System.Boolean&gt;&gt;.</returns>
        protected static Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
        {
            var lambdaParam = Expression.Parameter(typeof(TEntity));

            var lambdaBody = Expression.Equal(
                Expression.PropertyOrField(lambdaParam, "id"),
                Expression.Constant(id, typeof(TPrimaryKey))
                );
            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }
    }
}