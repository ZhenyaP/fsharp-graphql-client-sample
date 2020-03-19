// Learn more about F# at http://fsharp.org
namespace BibliotekoClient
open GraphQLClientRequests
open Newtonsoft.Json.Linq
open Newtonsoft.Json

//open GraphQLProviderOperations

module Main =

    let prettifyJson (object:obj) =
        let json = JsonConvert.SerializeObject(object, Formatting.Indented)
        json

    let callViaGraphQLProvider () =
        printfn "---------------------------------------------"
        printfn "Calls Via GraphQLProvider:"
        printfn "---------------------------------------------"
        printfn ""
        printfn "Registri By ISBN:"
        printfn "---------------------------------------------"
        GraphQLProviderRequests.asyncQueryRegistriByIsbn () |> Async.RunSynchronously |> prettifyJson |> printfn "%s"

        printfn ""
        printfn "---------------------------------------------"
        printfn "Registries:"
        printfn "---------------------------------------------"
        printfn ""
        GraphQLProviderRequests.asyncQueryRegistris () |> Async.RunSynchronously |> prettifyJson |> printfn "%s"

        printfn ""
        printfn "---------------------------------------------"
        printfn "Bibliotekos - Before addPetskribo mutation:"
        printfn "---------------------------------------------"
        printfn ""
        GraphQLProviderRequests.asyncQueryBibliotekos () |> Async.RunSynchronously |> prettifyJson |> printfn "%s"

        printfn ""
        printfn "---------------------------------------------"
        printfn "addPetskribo mutation:"
        printfn "---------------------------------------------"
        printfn ""
        GraphQLProviderRequests.asyncAddPetskriboToBiblioteko () |> Async.RunSynchronously |> prettifyJson |> printfn "%s"

        printfn ""
        printfn "---------------------------------------------"
        printfn "Bibliotekos - After addPetskribo mutation:"
        printfn "---------------------------------------------"
        printfn ""
        GraphQLProviderRequests.asyncQueryBibliotekos () |> Async.RunSynchronously |> prettifyJson |> printfn "%s"

        printfn ""
        printfn "---------------------------------------------"
        printfn "setReaction mutation:"
        printfn "---------------------------------------------"
        printfn ""
        GraphQLProviderRequests.asyncSetReaction () |> Async.RunSynchronously |> prettifyJson |> printfn "%s"

        printfn ""
        printfn "---------------------------------------------"
        printfn "Registries - after setReaction mutation:"
        printfn "---------------------------------------------"
        printfn ""
        GraphQLProviderRequests.asyncQueryRegistris () |> Async.RunSynchronously |> prettifyJson |> printfn "%s"

        printfn ""
        printfn "---------------------------------------------"
        printfn "Bibliotekos - After setReaction mutation:"
        printfn "---------------------------------------------"
        printfn ""
        GraphQLProviderRequests.asyncQueryBibliotekos () |> Async.RunSynchronously |> prettifyJson |> printfn "%s"

        printfn ""
        printfn "---------------------------------------------"
        printfn "removeReaction mutation:"
        printfn "---------------------------------------------"
        printfn ""
        GraphQLProviderRequests.asyncRemoveReaction () |> Async.RunSynchronously |> prettifyJson |> printfn "%s"
        
        printfn ""
        printfn "---------------------------------------------"
        printfn "Registries - after removeReaction mutation:"
        printfn "---------------------------------------------"
        printfn ""
        GraphQLProviderRequests.asyncQueryRegistris () |> Async.RunSynchronously |> prettifyJson |> printfn "%s"

        printfn ""
        printfn "---------------------------------------------"
        printfn "Bibliotekos - After removeReaction mutation:"
        printfn "---------------------------------------------"
        printfn ""
        GraphQLProviderRequests.asyncQueryBibliotekos () |> Async.RunSynchronously |> prettifyJson |> printfn "%s"


    let callViaGraphQLClient () =
        printfn "---------------------------------------------"
        printfn "Calls Via GraphQLClient:"
        printfn "---------------------------------------------"
        printfn ""
        printfn "Registri By ISBN:"
        GraphQLClientRequests.asyncQueryRegistriByIsbn "978-1617291326" |> Async.RunSynchronously |> prettifyJson |> printfn "%s"

        printfn "Registries:"
        GraphQLClientRequests.asyncQueryRegistries () |> Async.RunSynchronously |> prettifyJson |> printfn "%s"

        printfn "Bibliotekos - before addPetskribo mutation:"
        GraphQLClientRequests.asyncQueryBibliotekos () |> Async.RunSynchronously |> prettifyJson |> printfn "%s"

        let bibliotekoId = System.Guid "b5a21dc6-8a84-4ce2-8563-74a118449693"
        let petskribo = { isbn = "978-1680502541"
                          id = System.Guid "c894880c-4f74-423f-b0eb-80381e586ea9" }
        printfn "Add Petskribo %A to Biblioteko with id %s:" petskribo (bibliotekoId.ToString())
        GraphQLClientRequests.asyncAddPetskriboToBiblioteko bibliotekoId petskribo |> Async.RunSynchronously |> prettifyJson |> printfn "%s"

        printfn "Bibliotekos - after addPetskribo mutation:"
        GraphQLClientRequests.asyncQueryBibliotekos () |> Async.RunSynchronously |> prettifyJson |> printfn "%s"


    [<EntryPoint>]
    let main argv =
          callViaGraphQLProvider ()
          //callViaGraphQLClient ()
          System.Console.ReadKey () |> ignore
          0 // return an integer exit code
