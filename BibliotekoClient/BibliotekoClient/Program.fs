// Learn more about F# at http://fsharp.org
namespace BibliotekoClient
open GraphQLClientRequests
//open GraphQLProviderOperations

module Main =

    let callViaGraphQLProvider () =
        printfn "---------------------------------------------"
        printfn "Calls Via GraphQLProvider:"
        printfn "---------------------------------------------"
        printfn ""
        printfn "Registri By ISBN:"
        GraphQLProviderRequests.asyncQueryRegistriByIsbn () |> Async.RunSynchronously |> printfn "%A"

        printfn "Registries:"
        GraphQLProviderRequests.asyncQueryRegistris () |> Async.RunSynchronously |> printfn "%A"

        printfn "Bibliotekos - Before addPetskribo mutation:"
        GraphQLProviderRequests.asyncQueryBibliotekos () |> Async.RunSynchronously |> printfn "%A"

        printfn "addPetskribo mutation:"
        GraphQLProviderRequests.asyncAddPetskriboToBiblioteko () |> Async.RunSynchronously |> printfn "%A"

        printfn "Bibliotekos - After addPetskribo mutation:"
        GraphQLProviderRequests.asyncQueryBibliotekos () |> Async.RunSynchronously |> printfn "%A"

        printfn "setReaction mutation:"
        GraphQLProviderRequests.asyncSetReaction () |> Async.RunSynchronously |> printfn "%A"

        printfn "Registries - after setReaction mutation:"
        GraphQLProviderRequests.asyncQueryRegistris () |> Async.RunSynchronously |> printfn "%A"

        printfn "Bibliotekos - After setReaction mutation:"
        GraphQLProviderRequests.asyncQueryBibliotekos () |> Async.RunSynchronously |> printfn "%A"

        printfn "removeReaction mutation:"
        GraphQLProviderRequests.asyncRemoveReaction () |> Async.RunSynchronously |> printfn "%A"
        
        printfn "Registries - after removeReaction mutation:"
        GraphQLProviderRequests.asyncQueryRegistris () |> Async.RunSynchronously |> printfn "%A"

        printfn "Bibliotekos - After removeReaction mutation:"
        GraphQLProviderRequests.asyncQueryBibliotekos () |> Async.RunSynchronously |> printfn "%A"


    let callViaGraphQLClient () =
        printfn "---------------------------------------------"
        printfn "Calls Via GraphQLClient:"
        printfn "---------------------------------------------"
        printfn ""
        printfn "Registri By ISBN:"
        GraphQLClientRequests.asyncQueryRegistriByIsbn "978-1617291326" |> Async.RunSynchronously |> printfn "%A"

        printfn "Registries:"
        GraphQLClientRequests.asyncQueryRegistries () |> Async.RunSynchronously |> printfn "%A"

        printfn "Bibliotekos - before addPetskribo mutation:"
        GraphQLClientRequests.asyncQueryBibliotekos () |> Async.RunSynchronously |> printfn "%A"

        let bibliotekoId = System.Guid "b5a21dc6-8a84-4ce2-8563-74a118449693"
        let petskribo = { isbn = "978-1680502541"
                          id = System.Guid "c894880c-4f74-423f-b0eb-80381e586ea9" }
        printfn "Add Petskribo %A to Biblioteko with id %s:" petskribo (bibliotekoId.ToString())
        GraphQLClientRequests.asyncAddPetskriboToBiblioteko bibliotekoId petskribo |> Async.RunSynchronously |> printfn "%A"

        printfn "Bibliotekos - after addPetskribo mutation:"
        GraphQLClientRequests.asyncQueryBibliotekos () |> Async.RunSynchronously |> printfn "%A"


    [<EntryPoint>]
    let main argv =
          callViaGraphQLProvider ()
          //callViaGraphQLClient ()
          System.Console.ReadKey () |> ignore
          0 // return an integer exit code
