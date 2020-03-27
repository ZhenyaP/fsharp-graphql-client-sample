namespace BibliotekoClient

open GraphQLProviderCommon

module GraphQLProviderRunMutationHelpers =

    let addPetskribo (bibliotekoId:string) (petskribo: BiblProvider.Types.InputPetskribo) =
        printfn ""
        printfn "---------------------------------------------"
        printfn "addPetskribo mutation:"
        printfn "---------------------------------------------"
        printfn ""
        GraphQLProviderMutations.asyncAddPetskriboToBiblioteko bibliotekoId petskribo
        |> Async.RunSynchronously 
        |> printfn "%A"

    let setReaction(isbn:string) (recenzoId:string) (recenzoKind:BiblProvider.Types.ReactionEnum) =
        printfn ""
        printfn "---------------------------------------------"
        printfn "setReaction mutation:"
        printfn "---------------------------------------------"
        printfn ""
        GraphQLProviderMutations.asyncSetReaction isbn recenzoId recenzoKind
        |> Async.RunSynchronously 
        
        |> printfn "%A"

    let setComment(isbn:string) (recenzoId:string) (comment:string) =
        printfn ""
        printfn "---------------------------------------------"
        printfn "setComment mutation:"
        printfn "---------------------------------------------"
        printfn ""
        GraphQLProviderMutations.asyncSetComment isbn recenzoId comment
        |> Async.RunSynchronously 
        
        |> printfn "%A"

    let removeReaction (recenzoId:string) =
        printfn ""
        printfn "---------------------------------------------"
        printfn "removeReaction mutation:"
        printfn "---------------------------------------------"
        printfn ""
        GraphQLProviderMutations.asyncRemoveReaction recenzoId
            |> Async.RunSynchronously
            |> printfn "%A"

    let removeComment (recenzoId:string) =
        printfn ""
        printfn "---------------------------------------------"
        printfn "removeComment mutation:"
        printfn "---------------------------------------------"
        printfn ""
        GraphQLProviderMutations.asyncRemoveComment recenzoId
            |> Async.RunSynchronously 
            
            |> printfn "%A"


