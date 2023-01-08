using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace OnionArch.WebApi.Common
{
    public static class MediatorWebAPIExtension
    {
        /// <summary>
        /// 扩展方法,为所有MediatR Contract 消息类创建WebAPI接口
        /// </summary>
        /// <param name="app"></param>
        /// <param name="assemblies">Contract 消息类所在程序集</param>
        /// <returns></returns>
        public static IEndpointRouteBuilder MapMediatorWebAPI(this IEndpointRouteBuilder app, params Assembly[] assemblies)
        {
            //为所有实现了IRequest<>的消息类创建WebAPI接口
            Type genericRequestType = typeof(IRequest<>);
            var sendMethodInfo = typeof(MediatorWebAPIExtension).GetMethod("MapMediatorSendApi", BindingFlags.NonPublic | BindingFlags.Static);
            foreach (var assembly in assemblies)
            {
                //获取该程序集中所有实现了IRequest<>的消息类类型
                var requestTypes = assembly.GetTypes().Where(type => !type.IsInterface && type.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == genericRequestType));
                foreach (var requestType in requestTypes)
                {
                    //获取IRequest<>中尖括号中的泛型参数类型。
                    var responseType = requestType.GetInterfaces().First(t => t.IsGenericType && t.GetGenericTypeDefinition() == genericRequestType).GetGenericArguments().First();
                    //反射调用泛型映射WebApi方法
                    var genericMethod = sendMethodInfo.MakeGenericMethod(requestType, responseType);
                    genericMethod.Invoke(null, new object[] { app, requestType.Name });
                }

            }
            //为所有实现了INotification的消息类创建WebAAPI接口
            Type genericNotificationType = typeof(INotification);
            var publishMethodInfo = typeof(MediatorWebAPIExtension).GetMethod("MapMediatorPublishApi", BindingFlags.NonPublic | BindingFlags.Static);
            foreach (var assembly in assemblies)
            {
                //获取该程序集中所有实现了INotification的消息类类型
                var requestTypes = assembly.GetTypes().Where(type => !type.IsInterface && genericNotificationType.IsAssignableFrom(type));
                foreach (var requestType in requestTypes)
                {
                    //反射调用泛型映射WebApi方法
                    var genericMethod = publishMethodInfo.MakeGenericMethod(requestType);
                    genericMethod.Invoke(null, new object[] { app, requestType.Name });
                }

            }

            return app;
        }


        /// <summary>
        /// 为实现了IRequest<>的消息类为映射为WebAPI接口，根据消息类名称生成对应的CRUDD Http Method。
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="app"></param>
        /// <param name="requestTypeName"></param>
        internal static void MapMediatorSendApi<TRequest, TResponse>(IEndpointRouteBuilder app, string requestTypeName) where TRequest : IRequest<TResponse>
        {
            if (requestTypeName.StartsWith("Create")) //Http Post
            {
                var uri = new Uri(requestTypeName.Replace("Create", ""), UriKind.Relative);
                app.MapPost(uri.ToString(), async ([FromServices] IMediator mediator, [FromBody] TRequest request) =>
                {
                    TResponse response = await mediator.Send(request);
                    return Results.Created(uri, response);
                }).WithName(requestTypeName).WithOpenApi();
            }
            else if (requestTypeName.StartsWith("Read")) //Http Get
            {
                var uri = new Uri(requestTypeName.Replace("Read", ""), UriKind.Relative);
                app.MapGet(uri.ToString(), async ([FromServices] IMediator mediator, [FromBody] TRequest request) =>
                {
                    TResponse response = await mediator.Send(request);
                    return Results.Ok(response);
                }).WithName(requestTypeName).WithOpenApi();
            }
            else if (requestTypeName.StartsWith("Update")) //Http Put
            {
                var uri = new Uri(requestTypeName.Replace("Update", ""), UriKind.Relative);
                app.MapPut(uri.ToString(), async ([FromServices] IMediator mediator, [FromBody] TRequest request) =>
                {
                    TResponse response = await mediator.Send(request);
                    return Results.Ok(response);
                }).WithName(requestTypeName).WithOpenApi();
            }
            else if (requestTypeName.StartsWith("Delete")) //Http Delete
            {
                var uri = new Uri(requestTypeName.Replace("Delete", ""), UriKind.Relative);
                app.MapDelete(uri.ToString(), async ([FromServices] IMediator mediator, [FromBody] TRequest request) =>
                {
                    TResponse response = await mediator.Send(request);
                    return Results.NoContent();
                }).WithName(requestTypeName).WithOpenApi();
            }
            else  //如不匹配则生成MediatR Send WebAPI接口
            {
                app.MapPost("/mediatr/send/" + requestTypeName, async ([FromServices] IMediator mediator, [FromBody] TRequest request) =>
                {
                    TResponse response = await mediator.Send(request);
                    return Results.Ok(response);
                }).WithName(requestTypeName).WithOpenApi();
            }
        }

        /// <summary>
        /// 为实现了INotification的消息类映射WebAPI接口。
        /// </summary>
        /// <typeparam name="TNotification"></typeparam>
        /// <param name="app"></param>
        /// <param name="requestTypeName"></param>
        internal static void MapMediatorPublishApi<TNotification>(IEndpointRouteBuilder app, string requestTypeName) where TNotification : INotification
        {
            app.MapPost("/mediatr/publish/" + requestTypeName, async ([FromServices] IMediator mediator, [FromBody] TNotification notification) =>
            {
                await mediator.Publish(notification);
                return Results.Ok();
            }).WithName(requestTypeName).WithOpenApi();
        }
    }
}
