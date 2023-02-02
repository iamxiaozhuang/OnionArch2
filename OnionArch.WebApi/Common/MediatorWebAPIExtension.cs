using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using OnionArch.Application.Common.HTTP;
using System;
using System.Reflection;
using System.Reflection.Metadata;

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
            Type mediatorWebAPIConfigType = typeof(MediatorWebAPIConfigAttribute);
            Type httpMethodToGenerateType = typeof(HttpMethodToGenerate);
            var sendMethodInfo = typeof(MediatorWebAPIExtension).GetMethod("MapMediatorRequestApi", BindingFlags.NonPublic | BindingFlags.Static);
            foreach (var assembly in assemblies)
            {
                //获取该程序集中所有实现了IRequest<>的消息类类型
                var requestTypes = assembly.GetTypes().Where(type => !type.IsInterface && type.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == genericRequestType) && type.CustomAttributes.Any(t => t.AttributeType == mediatorWebAPIConfigType));
                foreach (var requestType in requestTypes)
                {
                    //获取Contract的MediatorWebAPIConfigAttribute中设置的HttpMethod值
                    var httpMethodTypedArgument = requestType.CustomAttributes.First(t => t.AttributeType == mediatorWebAPIConfigType)
                        .NamedArguments.First(t => t.MemberName == "HttpMethod").TypedValue;
                    var httpMethod = (HttpMethodToGenerate)httpMethodTypedArgument.Value;
                    var httpUrlTypedArgument = requestType.CustomAttributes.First(t => t.AttributeType == mediatorWebAPIConfigType)
                        .NamedArguments.First(t => t.MemberName == "HttpUrl").TypedValue;
                    var httpUrl = (string)httpUrlTypedArgument.Value;
                    //获取IRequest<>中尖括号中的泛型参数类型。
                    var responseType = requestType.GetInterfaces().First(t => t.IsGenericType && t.GetGenericTypeDefinition() == genericRequestType).GetGenericArguments().First();
                    //反射调用泛型映射WebApi方法
                    var genericMethod = sendMethodInfo.MakeGenericMethod(requestType, responseType);
                    genericMethod.Invoke(null, new object[] { app, requestType, httpMethod, httpUrl });
                }

            }
            //为所有实现了INotification的消息类创建WebAAPI接口
            Type genericNotificationType = typeof(INotification);
            var publishMethodInfo = typeof(MediatorWebAPIExtension).GetMethod("MapMediatorNotificationApi", BindingFlags.NonPublic | BindingFlags.Static);
            foreach (var assembly in assemblies)
            {
                //获取该程序集中所有实现了INotification的消息类类型
                var notificationTypes = assembly.GetTypes().Where(type => !type.IsInterface && genericNotificationType.IsAssignableFrom(type) && type.CustomAttributes.Any(t => t.AttributeType == mediatorWebAPIConfigType));
                foreach (var notificationType in notificationTypes)
                {
                    //获取Contract的MediatorWebAPIConfigAttribute中设置的HttpMethod值
                    var httpMethodTypedArgument = notificationType.CustomAttributes.First(t => t.AttributeType == mediatorWebAPIConfigType)
                        .NamedArguments.First(t => t.MemberName == "HttpMethod").TypedValue;
                    var httpMethod = (HttpMethodToGenerate)httpMethodTypedArgument.Value;
                    if (httpMethod != HttpMethodToGenerate.Default) continue;
                    //反射调用泛型映射WebApi方法
                    var genericMethod = publishMethodInfo.MakeGenericMethod(notificationType);
                    genericMethod.Invoke(null, new object[] { app, notificationType });
                }

            }

            return app;
        }



        /// <summary>
        /// 为实现了IRequest<>的消息类为映射为WebAPI接口。
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="app"></param>
        /// <param name="requestType"></param>
        /// <param name="httpMethod"></param>
        /// <param name="htttpUrl"></param>
        internal static void MapMediatorRequestApi<TRequest, TResponse>(IEndpointRouteBuilder app,Type requestType, HttpMethodToGenerate httpMethod, string htttpUrl) where TRequest : IRequest<TResponse>
        {
            switch (httpMethod)
            {
                case HttpMethodToGenerate.Post:
                    app.MapPost(htttpUrl, async ([FromServices] IMediator mediator, [FromBody] TRequest request) =>
                    {
                        TResponse response = await mediator.Send(request);
                        return Results.Created(htttpUrl, response);
                    }).WithName(htttpUrl).WithOpenApi();
                    break;
                case HttpMethodToGenerate.Get:
                    app.MapGet(htttpUrl, async ([FromServices] IMediator mediator, TRequest request) =>
                    {
                        TResponse response = await mediator.Send(request);
                        return Results.Ok(response);
                    }).WithName(htttpUrl).WithOpenApi();
                    break;
                case HttpMethodToGenerate.Put:
                    app.MapPut(htttpUrl, async ([FromServices] IMediator mediator, [FromBody] TRequest request) =>
                    {
                        TResponse response = await mediator.Send(request);
                        return Results.Ok(response);
                    }).WithName(htttpUrl).WithOpenApi();
                    break;
                case HttpMethodToGenerate.Patch:
                    app.MapPatch(htttpUrl, async ([FromServices] IMediator mediator, [FromBody] TRequest request) =>
                    {
                        TResponse response = await mediator.Send(request);
                        return Results.Ok(response);
                    }).WithName(htttpUrl).WithOpenApi();
                    break;
                case HttpMethodToGenerate.Delete:
                    app.MapDelete(htttpUrl, async ([FromServices] IMediator mediator,  TRequest request) =>
                    {
                        TResponse response = await mediator.Send(request);
                        return Results.NoContent();
                    }).WithName(htttpUrl).WithOpenApi();
                    break;
                default:
                    app.MapPost("/mediatr/request/" + requestType.Name, async ([FromServices] IMediator mediator, [FromBody] TRequest request) =>
                    {
                        TResponse response = await mediator.Send(request);
                        return Results.Ok(response);
                    }).WithName(requestType.Name).WithOpenApi();
                    break;

            }
        }

        /// <summary>
        /// 为实现了INotification的消息类映射WebAPI接口。
        /// </summary>
        /// <typeparam name="TNotification"></typeparam>
        /// <param name="app"></param>
        /// <param name="requestTypeName"></param>
        internal static void MapMediatorNotificationApi<TNotification>(IEndpointRouteBuilder app, Type notificationType) where TNotification : INotification
        {
            app.MapPost("/mediatr/notification/" + notificationType.Name, async ([FromServices] IMediator mediator, [FromBody] TNotification notification) =>
            {
                await mediator.Publish(notification);
                return Results.Ok();
            }).WithName(notificationType.Name).WithOpenApi();
        }
    }
}
