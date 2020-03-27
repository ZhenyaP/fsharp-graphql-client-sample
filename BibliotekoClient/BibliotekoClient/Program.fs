// Learn more about F# at http://fsharp.org
namespace BibliotekoClient
open GraphQLClientRequests
open GraphQLProviderCommon
open Newtonsoft.Json
open GraphQLProviderRunQueryHelpers
open GraphQLProviderRunMutationHelpers
open Helpers

//open GraphQLProviderOperations

module Main =    

    let invalidCallsViaGraphQLProvider () =
        printHeader "Invalid calls Via GraphQLProvider:"
        
        getRegistriByIsbn "123"
        
        let bibliotekoId = "aaa"
        let petskribo = BiblProvider.Types.InputPetskribo(id = "111", isbn = "222")
        addPetskribo bibliotekoId petskribo
        
        let isbn = "123"
        let recenzoId = "e3f5df8f-b8a5-461b-880a-facac7670848"
        let recenzoKind = BiblProvider.Types.ReactionEnum.Classic
        setReaction isbn recenzoId recenzoKind
        
        let isbn = "111"
        let recenzoId = "e8f5df8f-b8a5-461b-880a-facac7670848"
        let comment = "It is so gripping and cool"
        setComment isbn recenzoId comment

        let recenzoId = "123"
        removeComment recenzoId
        removeReaction recenzoId

    let callViaGraphQLProvider () =
        printHeader "Calls Via GraphQLProvider:"
        
        getRegistriByIsbn "978-1617291326"

        getRegistrij ()
        getBibliotekoj ()

        let bibliotekoId = "b5a21dc6-8a84-4ce2-8563-74a118449693"
        let petskribo = BiblProvider.Types.InputPetskribo(
                            id = "c894880c-4f74-423f-b0eb-80381e586ea9",
                            isbn = "978-1680502541")
        addPetskribo bibliotekoId petskribo

        getBibliotekoj ()

        let isbn = "9781680502541"
        let recenzoId = "e8f5df8f-b8a5-461b-880a-facac7670848"
        let recenzoKind = BiblProvider.Types.ReactionEnum.Gripping
        setReaction isbn recenzoId recenzoKind

        let isbn = "9781680502541"
        let recenzoId = "e8f5df8f-b8a5-461b-880a-facac7670848"
        let comment = "It is so gripping and cool"
        setComment isbn recenzoId comment

        getRegistrij ()
        getBibliotekoj ()

        let recenzoId = "e8f5df8f-b8a5-461b-880a-facac7670848"
        removeReaction recenzoId
        
        getRegistrij ()
        getBibliotekoj ()

        removeComment recenzoId
        
        getRegistrij ()
        getBibliotekoj ()

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
          invalidCallsViaGraphQLProvider()
          //callViaGraphQLProvider ()
          //callViaGraphQLClient ()
          System.Console.ReadKey () |> ignore
          0 // return an integer exit code
