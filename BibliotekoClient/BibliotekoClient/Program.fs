// Learn more about F# at http://fsharp.org
namespace BibliotekoClient

//open GraphQLProviderOperations

module Main =

    let callViaGraphQLProvider () =
        printfn "---------------------------------------------"
        printfn "Calls Via GraphQLProvider:"
        printfn "---------------------------------------------"
        printfn ""
        printfn "Registri By ISBN:"
        GraphQLProviderRequests.asyncQueryRegistriByIsbn() |> Async.RunSynchronously |> printfn "%A"

        printfn "Registries:"
        GraphQLProviderRequests.asyncQueryRegistris() |> Async.RunSynchronously |> printfn "%A"

        (*
        printfn "Before addPetskribo mutation:"
        asyncRunQuery asyncQueryBibliotekos |> Async.RunSynchronously

        asyncRunMutation asyncAddPetskriboToBiblioteko |> Async.RunSynchronously

        printfn "After addPetskribo mutation:"
        asyncRunQuery asyncQueryBibliotekos |> Async.RunSynchronously

        runQuery queryBibliotekos
        *)

    let callViaGraphQLClient () =
        printfn "---------------------------------------------"
        printfn "Calls Via GraphQLClient:"
        printfn "---------------------------------------------"
        printfn ""
        printfn "Registri By ISBN:"
        GraphQLClientRequests.asyncQueryRegistriByIsbn "978-1617291326" |> Async.RunSynchronously |> printfn "%A"

        printfn "Registries:"
        GraphQLClientRequests.asyncQueryRegistries |> Async.RunSynchronously |> printfn "%A"
        (*
        printfn "Bibliotekos - before addPetskribo mutation:"
        GraphQLClientRequests.asyncQueryBibliotekos |> Async.RunSynchronously |> printfn "%A"

        let bibliotekoId = System.Guid "b5a21dc6-8a84-4ce2-8563-74a118449693"
        let petskribo = { isbn = "978-1680502541"
                          id = System.Guid "c894880c-4f74-423f-b0eb-80381e586ea9" }
        printfn "Add Petskribo %A to Biblioteko with id %s:" petskribo (bibliotekoId.ToString())
        GraphQLClientRequests.asyncAddPetskriboToBiblioteko bibliotekoId petskribo |> Async.RunSynchronously |> printfn "%A"

        printfn "Bibliotekos - after addPetskribo mutation:"
        GraphQLClientRequests.asyncQueryBibliotekos |> Async.RunSynchronously |> printfn "%A"        
        *)

    [<EntryPoint>]
    let main argv =
          callViaGraphQLProvider ()
          //callViaGraphQLClient ()
          0 // return an integer exit code
