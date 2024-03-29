﻿using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
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
                    //获取Contract的MediatorWebAPIConfigAttribute
                    var mediatorWebAPIConfig = requestType.CustomAttributes.First(t => t.AttributeType == mediatorWebAPIConfigType);
                    //获取IRequest<>中尖括号中的泛型参数类型。
                    var responseType = requestType.GetInterfaces().First(t => t.IsGenericType && t.GetGenericTypeDefinition() == genericRequestType).GetGenericArguments().First();
                    //反射调用泛型映射WebApi方法
                    var genericMethod = sendMethodInfo.MakeGenericMethod(requestType, responseType);
                    genericMethod.Invoke(null, new object[] { app, requestType, mediatorWebAPIConfig });
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
        internal static void MapMediatorRequestApi<TRequest, TResponse>(IEndpointRouteBuilder app,Type requestType, CustomAttributeData mediatorWebAPIConfig) where TRequest : IRequest<TResponse>
        {
            //获取Contract的MediatorWebAPIConfigAttribute中设置的HttpMethod值
            var httpMethodTypedArgument = mediatorWebAPIConfig.NamedArguments.FirstOrDefault(t => t.MemberName == "HttpMethod").TypedValue;
            var httpMethod = (HttpMethodToGenerate)httpMethodTypedArgument.Value;
            var httpUrlTypedArgument = mediatorWebAPIConfig.NamedArguments.FirstOrDefault(t => t.MemberName == "HttpUrl").TypedValue;
            var httpUrl = (string)httpUrlTypedArgument.Value;
            var summaryTypedArgument = mediatorWebAPIConfig.NamedArguments.FirstOrDefault(t => t.MemberName == "Summary").TypedValue;
            var summary = (string)summaryTypedArgument.Value;
            var descriptionTypedArgument = mediatorWebAPIConfig.NamedArguments.FirstOrDefault(t => t.MemberName == "Description").TypedValue;
            var description = (string)descriptionTypedArgument.Value;

            var openApiOperationId = requestType.FullName;
            var openApiTags = new List<OpenApiTag> { new() { Name = requestType.FullName.Substring(0, requestType.FullName.LastIndexOf(".")) } };
            switch (httpMethod)
            {
                case HttpMethodToGenerate.Post:
                    app.MapPost(httpUrl, async ([FromServices] IMediator mediator, [FromBody] TRequest request) =>
                    {
                        TResponse response = await mediator.Send(request);
                        return Results.Created(httpUrl, response);
                    }).WithOpenApi(operation => new(operation)
                    {
                        OperationId = openApiOperationId,
                        Tags = openApiTags,
                        Summary = summary,
                        Description = description,
                    });
                    break;
                case HttpMethodToGenerate.Get:
                    app.MapGet(httpUrl, async ([FromServices] IMediator mediator, [AsParameters] TRequest request) =>
                    {
                        TResponse response = await mediator.Send(request);
                        return Results.Ok(response);
                    }).WithOpenApi(operation => new(operation)
                    {
                        OperationId = openApiOperationId,
                        Tags = openApiTags,
                        Summary = summary,
                        Description = description,
                    });
                    break;
                case HttpMethodToGenerate.Put:
                    app.MapPut(httpUrl, async ([FromServices] IMediator mediator, [FromBody] TRequest request) =>
                    {
                        TResponse response = await mediator.Send(request);
                        return Results.Ok(response);
                    }).WithOpenApi(operation => new(operation)
                    {
                        OperationId = openApiOperationId,
                        Tags = openApiTags,
                        Summary = summary,
                        Description = description,
                    });
                    break;
                case HttpMethodToGenerate.Patch:
                    app.MapPatch(httpUrl, async ([FromServices] IMediator mediator, [FromBody] TRequest request) =>
                    {
                        TResponse response = await mediator.Send(request);
                        return Results.Ok(response);
                    }).WithOpenApi(operation => new(operation)
                    {
                        OperationId = openApiOperationId,
                        Tags = openApiTags,
                        Summary = summary,
                        Description = description,
                    });
                    break;
                case HttpMethodToGenerate.Delete:
                    app.MapDelete(httpUrl, async ([FromServices] IMediator mediator, [FromBody] TRequest request) =>
                    {
                        TResponse response = await mediator.Send(request);
                        return Results.NoContent();
                    }).WithOpenApi(operation => new(operation)
                    {
                        OperationId = openApiOperationId,
                        Tags = openApiTags,
                        Summary = summary,
                        Description = description,
                    });
                    break;
                default:
                    app.MapPost("/mediatr/request/" + requestType.Name, async ([FromServices] IMediator mediator, [FromBody] TRequest request) =>
                    {
                        TResponse response = await mediator.Send(request);
                        return Results.Ok(response);
                    }).WithOpenApi(operation => new(operation)
                    {
                        OperationId = openApiOperationId,
                        Tags = openApiTags,
                        Summary = summary,
                        Description = description,
                    });
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
