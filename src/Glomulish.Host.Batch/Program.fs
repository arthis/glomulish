// Learn more about F# at http://fsharp.org

open System
open log4net
open log4net.Config
open System.Reflection
open System.IO
open System.Threading
open Suave
open Suave.Http
open Suave.Http
open Suave.Filters
open Suave.RequestErrors
open Suave.Operators
open Suave.Cookie
open Suave.Logging
open Suave.State.CookieStateStore
open Suave.State
open Suave.Authentication
open Suave.Successful
open Suave.Writers
open Suave.Web
open System.Security.Cryptography.X509Certificates

let logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    
let logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

[<EntryPoint>]
let main argv =

    logger.Info("Release the GLOM!!!!")

    let cts = new CancellationTokenSource()
    let binding = HttpBinding.createSimple  HTTP "192.168.99.100" 8083
    let conf = { defaultConfig with bindings=[binding ]; cancellationToken = cts.Token  }
    let listening, server = startWebServerAsync conf (Successful.OK "Hello World")
        
    Async.Start(server, cts.Token)
    printfn "Make requests now"
    logger.Info("THE GLOM IS UNLEASHED!!!!")
    Console.ReadKey true |> ignore

    cts.Cancel()

    logger
    0 // return an integer exit code



