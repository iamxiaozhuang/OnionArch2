using MediatR;
using OnionArch.Domain.Common.Base;
using OnionArch.Domain.Common.Paged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Domain.Common.Database
{
    public class RepositoryService<TEntity> where TEntity : AggregateRoot<TEntity>
    {
        private readonly IMediator _mediator;

        public RepositoryService(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// 查询单个实体
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
            return await _mediator.Send(new ReadEntitiesRequest<TEntity, TOrder>(whereLambda,orderbyLambda,isAsc));
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
