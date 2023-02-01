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
        public async Task<TEntity> Create(TEntity entity)
        {
            return await _mediator.Send(new CreateEntityRequest<TEntity>(entity));
        }
        /// <summary>
        /// 创建多个实体
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task Create(params TEntity[] entities)
        {
            await _mediator.Send(new CreateEntitiesRequest<TEntity>(entities));
        }

        /// <summary>
        /// 删除单个实体
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<TEntity> Delete(Guid Id)
        {
            return await _mediator.Send(new DeleteEntityRequest<TEntity>(Id));
        }
        /// <summary>
        /// 删除多个实体
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public async Task<int> Delete(Expression<Func<TEntity, bool>> whereLambda)
        {
            return await _mediator.Send(new DeleteEntitiesRequest<TEntity>(whereLambda));
        }

         /// <summary>
        /// 获取单个实体以更新实体字段
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<TEntity> GetEntity(Guid Id)
        {
            return await _mediator.Send(new GetEntityRequest<TEntity>(Id));
        }


        /// <summary>
        /// 查询单个实体(不支持更新实体)
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<TEntity> ReadEntity(Guid Id)
        {
            return await _mediator.Send(new ReadEntityRequest<TEntity>(Id));
        }
        /// <summary>
        /// 查询多个实体
        /// </summary>
        /// <typeparam name="TOrder"></typeparam>
        /// <param name="whereLambda"></param>
        /// <param name="orderbyLambda"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public async Task<IQueryable<TEntity>> ReadEntities<TOrder>(Expression<Func<TEntity, bool>> whereLambda, Expression<Func<TEntity, TOrder>> orderbyLambda, bool isAsc = true)
        {
            return await _mediator.Send(new ReadEntitiesRequest<TEntity, TOrder>(whereLambda, orderbyLambda, isAsc));
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
        public async Task<PagedResult<TEntity>> ReadPagedEntities<TOrder>(Expression<Func<TEntity, bool>> whereLambda, PagedOption pagedOption, Expression<Func<TEntity, TOrder>> orderbyLambda, bool isAsc = true)
        {
            return await _mediator.Send(new ReadPagedEntitiesRequest<TEntity, TOrder>(whereLambda, pagedOption, orderbyLambda, isAsc));
        }

        /// <summary>
        /// 判断是否有存在
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public async Task<bool> IsExist(Expression<Func<TEntity, bool>> whereLambda)
        {
            return await _mediator.Send(new IsExistRequest<TEntity>(whereLambda));
        }
    }
}
