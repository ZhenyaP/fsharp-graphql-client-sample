// Learn more about F# at http://fsharp.org
namespace BibliotekoClient
open GraphQLClientRequests
open GraphQLProviderCommon
open GraphQLProviderRunQueryHelpers
open GraphQLProviderRunMutationHelpers
open Helpers

//open GraphQLProviderOperations

module Main =

    let [<Literal>] beforeAddPetskribo = "Before addPetskribo mutation"
    let [<Literal>] afterAddPetskribo = "After addPetskribo mutation"
    let [<Literal>] afterSetReaction = "After setReaction mutation"
    let [<Literal>] afterSetComment = "After setComment mutations"
    let [<Literal>] afterSetReactionAndComment = "After setReaction/setComment mutations"
    let [<Literal>] afterRemoveReaction = "After removeReaction mutation"
    let [<Literal>] afterRemoveComment = "After removeComment mutation"
    let [<Literal>] afterRemoveRecenzo = "After removeRecenzo mutation"

    let invalidCallsViaGraphQLProvider () =
        printHeader "Invalid calls Via GraphQLProvider:"

        getRegistriByIsbn "123" ""

        let bibliotekoId = "aaa"
        let petskribo = BiblProvider.Types.InputPetskribo(id = "111", isbn = "222")
        addPetskribo bibliotekoId petskribo ""

        let isbn = "123"
        let recenzoId = "e3f5df8f-b8a5-461b-880a-facac7670848"
        let recenzoKind = BiblProvider.Types.ReactionEnum.Classic
        setReaction isbn recenzoId recenzoKind ""

        let isbn = "111"
        let recenzoId = "e8f5df8f-b8a5-461b-880a-facac7670848"
        let comment = "It is so gripping and cool"
        setComment isbn recenzoId comment ""

        let recenzoId = "123"
        removeComment recenzoId ""
        let recenzoId = "e57abb99-b368-4a39-ba61-4d922231a8b5"
        removeReaction recenzoId ""
        let recenzoId = "17ff91d1-945d-4358-bcef-ebe7a91df8ca"
        removeRecenzo recenzoId ""

        // Here we are correctly adding a new review with reaction, and after that
        // we are trying to remove a comment from this review, and it should fail
        let isbn = "9781680502541"
        let recenzoId = "7dabec3d-1370-466e-a7a7-993ef3177297"
        let recenzoKind = BiblProvider.Types.ReactionEnum.Gripping
        setReaction isbn recenzoId recenzoKind ""
        removeComment recenzoId ""

        // Here we are correctly adding a new review with comment, and after that
        // we are trying to remove a reaction from this review, and it should fail
        let isbn = "978-1617291326"
        let recenzoId = "72f992de-3705-4610-861e-9447cb1e8d82"
        let comment = "It is so boring"
        setComment isbn recenzoId comment ""
        removeReaction recenzoId ""


    let callViaGraphQLProvider () =
        printHeader "Calls Via GraphQLProvider:"

        getRegistriByIsbn "978-1617291326" ""

        getRegistrij beforeAddPetskribo
        getBibliotekoj beforeAddPetskribo

        let bibliotekoId = "b5a21dc6-8a84-4ce2-8563-74a118449693"
        let petskribo = BiblProvider.Types.InputPetskribo(
                            id = "c894880c-4f74-423f-b0eb-80381e586ea9",
                            isbn = "978-1680502541")
        addPetskribo bibliotekoId petskribo ""

        getBibliotekoj afterAddPetskribo

        let isbn = "9781680502541"
        let recenzoId = "e8f5df8f-b8a5-461b-880a-facac7670848"
        let recenzoKind = BiblProvider.Types.ReactionEnum.Gripping
        setReaction isbn recenzoId recenzoKind ""

        let isbn = "9781680502541"
        let recenzoId = "e8f5df8f-b8a5-461b-880a-facac7670848"
        let comment = "It is so gripping and cool"
        setComment isbn recenzoId comment ""

        getRegistrij afterSetReactionAndComment
        getBibliotekoj afterSetReactionAndComment

        let recenzoId = "e8f5df8f-b8a5-461b-880a-facac7670848"
        removeReaction recenzoId ""

        getRegistrij afterRemoveReaction
        getBibliotekoj afterRemoveReaction

        removeComment recenzoId ""

        getRegistrij afterRemoveComment
        getBibliotekoj afterRemoveComment

        let isbn = "978-1617291326"
        let recenzoId = "72f992de-3705-4610-861e-9447cb1e8d82"
        let comment = "It is so boring"
        setComment isbn recenzoId comment ""

        getRegistrij afterSetComment
        getBibliotekoj afterSetComment

        removeRecenzo recenzoId ""

        getRegistrij afterRemoveRecenzo
        getBibliotekoj afterRemoveRecenzo

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
