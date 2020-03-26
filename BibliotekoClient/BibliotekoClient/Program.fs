// Learn more about F# at http://fsharp.org
namespace BibliotekoClient
open GraphQLClientRequests
open GraphQLProviderCommon
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
        GraphQLProviderQueries.asyncQueryRegistriByIsbn "978-1617291326" 
            |> Async.RunSynchronously 
            //|> prettifyJson 
            |> printfn "%A"

        printfn ""
        printfn "---------------------------------------------"
        printfn "Registries:"
        printfn "---------------------------------------------"
        printfn ""
        GraphQLProviderQueries.asyncQueryRegistris ()
            |> Async.RunSynchronously 
            //|> prettifyJson 
            |> printfn "%A"

        printfn ""
        printfn "---------------------------------------------"
        printfn "Bibliotekos - Before addPetskribo mutation:"
        printfn "---------------------------------------------"
        printfn ""
        GraphQLProviderQueries.asyncQueryBibliotekoj ()
            |> Async.RunSynchronously 
            //|> prettifyJson 
            |> printfn "%A"

        printfn ""
        printfn "---------------------------------------------"
        printfn "addPetskribo mutation:"
        printfn "---------------------------------------------"
        printfn ""
        let bibliotekoId = "b5a21dc6-8a84-4ce2-8563-74a118449693"
        let petskribo = BiblProvider.Types.InputPetskribo(
                            id = "c894880c-4f74-423f-b0eb-80381e586ea9",
                            isbn = "978-1680502541")
        GraphQLProviderMutations.asyncAddPetskriboToBiblioteko bibliotekoId petskribo
            |> Async.RunSynchronously 
            //|> prettifyJson 
            |> printfn "%A"

        printfn ""
        printfn "---------------------------------------------"
        printfn "Bibliotekos - After addPetskribo mutation:"
        printfn "---------------------------------------------"
        printfn ""
        GraphQLProviderQueries.asyncQueryBibliotekoj ()
            |> Async.RunSynchronously 
            //|> prettifyJson 
            |> printfn "%A"

        printfn ""
        printfn "---------------------------------------------"
        printfn "setReaction mutation:"
        printfn "---------------------------------------------"
        printfn ""
        let isbn = "9781680502541"
        let recenzoId = "e8f5df8f-b8a5-461b-880a-facac7670848"
        let recenzoKind = BiblProvider.Types.ReactionEnum.Gripping
        GraphQLProviderMutations.asyncSetReaction isbn recenzoId recenzoKind
            |> Async.RunSynchronously 
            //|> prettifyJson 
            |> printfn "%A"

        printfn ""
        printfn "---------------------------------------------"
        printfn "setComment mutation:"
        printfn "---------------------------------------------"
        printfn ""
        let isbn = "9781680502541"
        let recenzoId = "e8f5df8f-b8a5-461b-880a-facac7670848"
        let comment = "It is so gripping and cool"
        GraphQLProviderMutations.asyncSetComment isbn recenzoId comment
            |> Async.RunSynchronously 
            //|> prettifyJson 
            |> printfn "%A"

        printfn ""
        printfn "---------------------------------------------"
        printfn "Registris - after setReaction/setComment mutations:"
        printfn "---------------------------------------------"
        printfn ""
        GraphQLProviderQueries.asyncQueryRegistris ()
            |> Async.RunSynchronously 
            //|> prettifyJson 
            |> printfn "%A"

        printfn ""
        printfn "---------------------------------------------"
        printfn "Bibliotekos - After setReaction/setComment mutations:"
        printfn "---------------------------------------------"
        printfn ""
        GraphQLProviderQueries.asyncQueryBibliotekoj ()
            |> Async.RunSynchronously 
            //|> prettifyJson 
            |> printfn "%A"

        printfn ""
        printfn "---------------------------------------------"
        printfn "removeReaction mutation:"
        printfn "---------------------------------------------"
        printfn ""
        let recenzoId = "e8f5df8f-b8a5-461b-880a-facac7670848"
        GraphQLProviderMutations.asyncRemoveReaction recenzoId
            |> Async.RunSynchronously
            |> printfn "%A"
        
        printfn ""
        printfn "---------------------------------------------"
        printfn "Registris - after removeReaction mutation:"
        printfn "---------------------------------------------"
        printfn ""
        GraphQLProviderQueries.asyncQueryRegistris ()
            |> Async.RunSynchronously 
            //|> prettifyJson 
            |> printfn "%A"

        printfn ""
        printfn "---------------------------------------------"
        printfn "Bibliotekos - After removeReaction mutation:"
        printfn "---------------------------------------------"
        printfn ""
        GraphQLProviderQueries.asyncQueryBibliotekoj ()
            |> Async.RunSynchronously 
            //|> prettifyJson 
            |> printfn "%A"

        printfn ""
        printfn "---------------------------------------------"
        printfn "removeComment mutation:"
        printfn "---------------------------------------------"
        printfn ""
        let recenzoId = "e8f5df8f-b8a5-461b-880a-facac7670848"
        GraphQLProviderMutations.asyncRemoveComment recenzoId
            |> Async.RunSynchronously 
            //|> prettifyJson 
            |> printfn "%A"
        
        printfn ""
        printfn "---------------------------------------------"
        printfn "Registris - after removeReaction/removeComment mutations:"
        printfn "---------------------------------------------"
        printfn ""
        GraphQLProviderQueries.asyncQueryRegistris ()
            |> Async.RunSynchronously 
            //|> prettifyJson 
            |> printfn "%A"

        printfn ""
        printfn "---------------------------------------------"
        printfn "Bibliotekos - After removeReaction/removeComment mutations:"
        printfn "---------------------------------------------"
        printfn ""
        GraphQLProviderQueries.asyncQueryBibliotekoj ()
            |> Async.RunSynchronously 
            //|> prettifyJson 
            |> printfn "%A"


    let callViaGraphQLClient () =
        printfn "---------------------------------------------"
        printfn "Calls Via GraphQLClient:"
        printfn "---------------------------------------------"
        printfn ""
        printfn "Registri By ISBN:"
        GraphQLClientRequests.asyncQueryRegistriByIsbn "978-1617291326" |> Async.RunSynchronously //|> prettifyJson |> printfn "%A"

        printfn "Registries:"
        GraphQLClientRequests.asyncQueryRegistries () |> Async.RunSynchronously //|> prettifyJson |> printfn "%A"

        printfn "Bibliotekos - before addPetskribo mutation:"
        GraphQLClientRequests.asyncQueryBibliotekos () |> Async.RunSynchronously //|> prettifyJson |> printfn "%A"

        let bibliotekoId = System.Guid "b5a21dc6-8a84-4ce2-8563-74a118449693"
        let petskribo = { isbn = "978-1680502541"
                          id = System.Guid "c894880c-4f74-423f-b0eb-80381e586ea9" }
        printfn "Add Petskribo %A to Biblioteko with id %s:" petskribo (bibliotekoId.ToString())
        GraphQLClientRequests.asyncAddPetskriboToBiblioteko bibliotekoId petskribo |> Async.RunSynchronously //|> prettifyJson |> printfn "%A"

        printfn "Bibliotekos - after addPetskribo mutation:"
        GraphQLClientRequests.asyncQueryBibliotekos () |> Async.RunSynchronously //|> prettifyJson |> printfn "%A"


    [<EntryPoint>]
    let main argv =
          callViaGraphQLProvider ()
          //callViaGraphQLClient ()
          System.Console.ReadKey () |> ignore
          0 // return an integer exit code
