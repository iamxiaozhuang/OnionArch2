using MediatR;
using OnionArch.Domain.Common.Entities;
using OnionArch.Domain.Common.Paged;
using System.Linq.Expressions;

namespace OnionArch.Domain.Common.Repositories
{
    public class RepositoryService<TEntity> where TEntity : BaseEntity
    {
        private readonly IMediator _mediator;

        public RepositoryService(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// 创建单个实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<TEntity> Add(TEntity entity)
        {
            return await _mediator.Send(new AddEntityRequest<TEntity>(entity));
        }
        /// <summary>
        /// 创建多个实体
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task Add(params TEntity[] entities)
        {
            await _mediator.Send(new AddEntitiesRequest<TEntity>(entities));
        }

        /// <summary>
        /// 删除单个实体
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<TEntity> Remove(Guid Id)
        {
            return await _mediator.Send(new RemoveEntityRequest<TEntity>(Id));
        }
        /// <summary>
        /// 删除多个实体
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public async Task<int> Remove(Expression<Func<TEntity, bool>> whereLambda)
        {
            return await _mediator.Send(new RemoveEntitiesRequest<TEntity>(whereLambda));
        }

         /// <summary>
        /// 获取单个实体以更新实体字段
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<TEntity> Edit(Guid Id)
        {
            return await _mediator.Send(new EditEntityRequest<TEntity>(Id));
        }

        public async Task<IQueryable<TEntity>> Edit(Expression<Func<TEntity, bool>> whereLambda)
        {
            return await _mediator.Send(new EditEntitiesRequest<TEntity>(whereLambda));
        }


        /// <summary>
        /// 查询单个实体(不支持更新实体)
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<TModel> Query<TModel>(Guid Id)
        {
            return await _mediator.Send(new QueryEntityRequest<TEntity,TModel>(Id));
        }
        /// <summary>
        /// 查询多个实体
        /// </summary>
        public async Task<IQueryable<TModel>> Query<TModel>(Expression<Func<TEntity, bool>> whereLambda)
        {
            return await _mediator.Send(new QueryEntitiesRequest<TEntity,TModel>(whereLambda));
        }
        /// <summary>
        /// 分页查询多个实体
        /// </summary>
        /// <typeparam name="TOrder"></typeparam>
        /// <param name="whereLambda"></param>
        /// <param name="pageOption"></param>
        /// <param name="orderbyLambda"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public async Task<PagedResult<TModel>> Query<TOrder,TModel>(Expression<Func<TEntity, bool>> whereLambda, PagedOption pagedOption, Expression<Func<TEntity, TOrder>> orderbyLambda, bool isAsc = true)
        {
            return await _mediator.Send(new QueryPagedEntitiesRequest<TEntity, TOrder,TModel>(whereLambda, pagedOption, orderbyLambda, isAsc));
        }

        /// <summary>
        /// 判断是否有存在
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public async Task<bool> Any(Expression<Func<TEntity, bool>> whereLambda)
        {
            return await _mediator.Send(new AnyEntitiesRequest<TEntity>(whereLambda));
        }
    }
}
