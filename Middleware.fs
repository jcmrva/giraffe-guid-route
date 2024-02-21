[<AutoOpen>]
module giraffe_guidroute.Middleware

open System
open System.Threading.Tasks
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Logging
open Giraffe


let fmtErrorHandler (ex : Exception) (logger : ILogger) =
    logger.LogError(ex, "An invalid guid value was matched on a guid route.")
    clearResponse >=> setStatusCode 400 >=> text ex.Message

// adapted from GiraffeErrorHandlerMiddleware
type UnrecognizedGuidHandlerMiddleware (next          : RequestDelegate,
                                 //errorHandler  : ErrorHandler,
                                 loggerFactory : ILoggerFactory) =

    do if isNull next then raise (ArgumentNullException("next"))

    member __.Invoke (ctx : HttpContext) =
        task {
            try return! next.Invoke ctx
            with
            | :? FormatException as ex ->
                let logger = loggerFactory.CreateLogger<UnrecognizedGuidHandlerMiddleware>()
                try
                    let func = (Some >> Task.FromResult)
                    let! _ = fmtErrorHandler ex logger func ctx
                    return ()
                with ex2 ->
                    logger.LogError(EventId(0), ex,  "An unhandled exception has occurred while executing the request.")
                    logger.LogError(EventId(0), ex2, "An exception was thrown attempting to handle the original exception.")
        }

