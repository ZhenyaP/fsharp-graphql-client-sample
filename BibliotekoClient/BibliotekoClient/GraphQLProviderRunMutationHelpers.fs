namespace BibliotekoClient

open GraphQLProviderCommon
open Helpers

module GraphQLProviderRunMutationHelpers =

    let addPetskribo (bibliotekoId:string)
                     (petskribo: BiblProvider.Types.InputPetskribo)
                     (extraHeaderText: string) =
        printHeader (sprintf "addPetskribo mutation%s:" extraHeaderText)
        GraphQLProviderMutations.asyncAddPetskriboToBiblioteko bibliotekoId petskribo
        |> Async.RunSynchronously
        |> printfn "%A"

    let setReaction (isbn:string)
                    (recenzoId:string)
                    (recenzoKind:BiblProvider.Types.ReactionEnum)
                    (extraHeaderText: string) =
        printHeader (sprintf "setReaction mutation%s:" extraHeaderText)
        GraphQLProviderMutations.asyncSetReaction isbn recenzoId recenzoKind
        |> Async.RunSynchronously
        |> printfn "%A"

    let setComment (isbn:string)
                   (recenzoId:string)
                   (comment:string)
                   (extraHeaderText: string) =
        printHeader (sprintf "setComment mutation%s:" extraHeaderText)
        GraphQLProviderMutations.asyncSetComment isbn recenzoId comment
        |> Async.RunSynchronously
        |> printfn "%A"

    let removeReaction (recenzoId:string)
                       (extraHeaderText: string) =
        printHeader (sprintf "removeReaction mutation%s:" extraHeaderText)
        GraphQLProviderMutations.asyncRemoveReaction recenzoId
            |> Async.RunSynchronously
            |> printfn "%A"

    let removeComment (recenzoId:string)
                      (extraHeaderText: string) =
        printHeader (sprintf "removeComment mutation%s:" extraHeaderText)
        GraphQLProviderMutations.asyncRemoveComment recenzoId
            |> Async.RunSynchronously
            |> printfn "%A"

    let removeRecenzo (recenzoId:string)
                      (extraHeaderText: string) =
        printHeader (sprintf "removeRecenzo mutation%s:" extraHeaderText)
        GraphQLProviderMutations.asyncRemoveRecenzo recenzoId
            |> Async.RunSynchronously
            |> printfn "%A"


