 //Learn more about F# at http://fsharp.org
namespace BibliotekoClient

open System
open GraphQLProviderCommon
open Domain
open Chessie.ErrorHandling
open BibliotekoClient.Helpers

module GraphQLProviderMutations =

    let addPetskriboOperation = BiblProvider.Operation<"queries/addPetskribo.graphql">()
    let setReactionOperation = BiblProvider.Operation<"queries/setReaction.graphql">()
    let setCommentOperation = BiblProvider.Operation<"queries/setComment.graphql">()
    let removeReactionOperation = BiblProvider.Operation<"queries/removeReaction.graphql">()
    let removeCommentOperation = BiblProvider.Operation<"queries/removeComment.graphql">()
    let removeRecenzoOperation = BiblProvider.Operation<"queries/removeRecenzo.graphql">()

    let asyncAddPetskriboToBiblioteko (bibliotekoId : string) 
                                      (petskribo : BiblProvider.Types.InputPetskribo) 
                                      : Async<bool> =
        async {
            use runtimeContext = getContext()            
            let! result = addPetskriboOperation.AsyncRun(runtimeContext,
                                            bibliotekoId = bibliotekoId,
                                            petskribo = petskribo,
                                            uzantoId = ""
                                           )
            checkForErrors result "addPetskribo"
            return true
            //let petskriboEntity = result.Data.Value.AddPetskribo.Value
            //let registriEntity = petskriboEntity.Registri
            //let registri = 
            //    {   
            //        Id = registriEntity.Id |> System.Guid.Parse
            //        ISBN = registriEntity.Isbn
            //        Title = registriEntity.Title
            //        Authors = registriEntity.Authors |> Array.toList
            //        Summary = registriEntity.Summary
            //        ImageURL = registriEntity.Imageurl 
            //    }
            //let petskribo = 
            //    {
            //        Id = petskriboEntity.Id |> System.Guid.Parse
            //        Registri = registri
            //        Logbook = petskriboEntity.Logbook |> Array.toList
            //        Owner = petskriboEntity.Owner |> System.Guid.Parse
            //    }

            //return petskribo
        }

    let asyncSetReaction (isbn : string) 
                         (recenzoId : string) 
                         (reactionKind : BiblProvider.Types.ReactionEnum)
                         : Async<Recenzo> =
        async {
            use runtimeContext = getContext()
            let! result = setReactionOperation.AsyncRun(runtimeContext,
                                                        isbn = isbn,
                                                        recenzoId = recenzoId,
                                                        reactionKind = reactionKind,
                                                        uzantoId = "")  //userId variable value is set in GraphQL server
            checkForErrors result "setReaction"
            let recenzoEntity = result.Data.Value.SetReaction
            let reactionName = 
                match recenzoEntity.Content with
                | c when c.IsReaction() -> c.AsReaction().Reaction.GetValue()
                | c when c.IsReactionAndComment() -> c.AsReactionAndComment().Reaction.GetValue()
                | _ -> raise <| System.NotSupportedException ()
            let reaction = Enum.Parse(typedefof<Reaction>, reactionName) :?> Reaction

            let recenzo = 
                { 
                    Id = recenzoEntity.Id |> System.Guid.Parse
                    Recenzorer = recenzoEntity.Recenzorer
                    Content = RecenzoContent.Reaction reaction
                }
                
            return recenzo
        }

    let asyncSetComment (isbn: string) 
                        (recenzoId: string) 
                        (comment: string) 
                        : Async<Recenzo> =
        async {
            use runtimeContext = getContext()
            let! result = setCommentOperation.AsyncRun(runtimeContext,
                                                        isbn = isbn,
                                                        recenzoId = recenzoId,
                                                        comment = comment,
                                                        uzantoId = "")  //userId variable value is set in GraphQL server
            checkForErrors result "setComment"
            let recenzoEntity = result.Data.Value.SetComment
            let commentText = 
                match recenzoEntity.Content with
                | c when c.IsComment() -> c.AsComment().Comment
                | c when c.IsReactionAndComment() -> c.AsReactionAndComment().Comment
                | _ -> raise <| System.NotSupportedException ()
            let comment = Comment.create commentText |> returnOrFail

            let recenzo = 
                { 
                    Id = recenzoEntity.Id |> System.Guid.Parse
                    Recenzorer = recenzoEntity.Recenzorer
                    Content = Comment comment 
                }
                
            return recenzo
        }

    let asyncRemoveReaction (recenzoId: string) : Async<bool> =
        async {
            use runtimeContext = getContext()
            let! result = removeReactionOperation.AsyncRun(runtimeContext,
                            recenzoId = recenzoId
            )
            checkForErrors result "removeReaction"
            return result.Data.Value.RemoveReaction
        }


    let asyncRemoveComment (recenzoId: string) : Async<bool> =
        async {
            use runtimeContext = getContext()
            let! result = removeCommentOperation.AsyncRun(runtimeContext,
                                    recenzoId = recenzoId
                                    )
            checkForErrors result "removeComment"
            return result.Data.Value.RemoveComment
        }

    let asyncRemoveRecenzo (recenzoId: string) : Async<bool> =
        async {
            use runtimeContext = getContext()
            let! result = removeRecenzoOperation.AsyncRun(runtimeContext,
                                    recenzoId = recenzoId
                                    )
            checkForErrors result "removeRecenzo"
            return result.Data.Value.RemoveRecenzo
        }
