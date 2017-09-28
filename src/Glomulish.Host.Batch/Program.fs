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
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Configuration.Json
open Glomulish.Domain.Library

let logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    
let logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

[<AutoOpen>]
module configuration =

    let getConfig fileName =  
        // Adding JSON file into IConfiguration.
        let conf = new ConfigurationBuilder()
        conf.AddJsonFile(fileName, false, true)
        conf.Build()

    let getKey (conf:IConfiguration ) key =
        let value = conf.[key]
        printfn "key %s value %s" key value
        if String.IsNullOrEmpty value then 
            failwith <| sprintf "key %s not found" key
        value


[<EntryPoint>]
let main argv =

    logger.Info("Release the GLOM!!!!")

    let cts = new CancellationTokenSource()
    
    let getSuaveKey = getConfig "config/suave.json" |> getKey 

    let iPAddress = getSuaveKey "Server:IPAddress"   
    let port = getSuaveKey "Server:Port" |> int

    let binding = HttpBinding.createSimple  HTTP iPAddress port
    let conf = { defaultConfig with bindings=[binding ]; cancellationToken = cts.Token  }

    let helloText = sprintf "Hello GET !!! " 

    let app =
      choose
        [ GET >=> choose
            [ path "/hello" >=> OK   helloText
              path "/goodbye" >=> OK "Good bye GET" ]
          POST >=> choose
            [ path "/hello" >=> OK "Hello POST"
              path "/goodbye" >=> OK "Good bye POST" ] ]
    
    startWebServer  conf app
   
    
    0 // return an integer exit code



