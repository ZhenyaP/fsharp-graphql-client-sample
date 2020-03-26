 //Learn more about F# at http://fsharp.org
namespace BibliotekoClient

open System
open GraphQLProviderCommon
open Domain
open Chessie.ErrorHandling

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
                                            //bibliotekoId = "b5a21dc6-8a84-4ce2-8563-74a118449693",
                                            //petskribo = BiblProvider.Types.InputPetskribo(
                                            //            id = "c894880c-4f74-423f-b0eb-80381e586ea9",
                                            //            isbn = "978-1680502541"),
                                            uzantoId = ""
                                           )
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
                                                        //isbn = "978-1617291326",
                                                        //recenzoId = "67e8d007-002c-4541-9ca8-0983780cc4d6",
                                                        //reactionKind = BiblProvider.Types.ReactionEnum.LoveIt,
                                                        uzantoId = "")  //userId variable value is set in GraphQL server
            let recenzoEntity = result.Data.Value.SetReaction
            //let reactionName = recenzoEntity.Content.AsReaction().Reaction.GetName()
            let reactionName = recenzoEntity.Content.AsReaction().Reaction.GetValue()
            let reaction = Enum.Parse(typedefof<Reaction>, reactionName) :?> Reaction

                //match recenzoEntity.Content with 
                //    | c when c.IsComment() -> 
                //        let result = Comment (Comment.create (c.AsComment().Comment) |> returnOrFail)
                //        result
                //    | c when c.IsReaction() ->
                //        let reactionName = c.AsReaction().Reaction.GetName()
                //        let reaction = Enum.Parse(typedefof<Reaction>, reactionName) :?> Reaction
                //            //match reactionEntity.GetName() with
                //            //    | BiblProvider.Types.ReactionEnum.Boring -> Reaction.Boring
                //            //    | BiblProvider.Types.ReactionEnum.Classic -> Reaction.Classic
                //            //    | BiblProvider.Types.ReactionEnum.Gripping -> Reaction.Gripping
                //            //    | BiblProvider.Types.ReactionEnum.Inspiring -> Reaction.Inspiring
                //            //    | BiblProvider.Types.ReactionEnum.LoveIt -> Reaction.LoveIt
                //            //    | BiblProvider.Types.ReactionEnum.Moving -> Reaction.Moving
                //        let result = Reaction reaction

                //        result
                //    | c when c.IsReactionAndComment() ->
                //        let reactionAndCommentEntity = c.AsReactionAndComment()
                //        let comment = Comment.create (reactionAndCommentEntity.Comment) |> returnOrFail
                //        let reactionName = reactionAndCommentEntity.Reaction.GetName()
                //        let reaction = Enum.Parse(typedefof<Reaction>, reactionName) :?> Reaction
                //        let result = ReactionAndComment (reaction, comment)

                //        result
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
            let recenzoEntity = result.Data.Value.SetComment
            let comment = Comment.create (recenzoEntity.Content.AsComment().Comment) |> returnOrFail

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
                            //recenzoId = "67e8d007-002c-4541-9ca8-0983780cc4d6"
            )
            
            return result.Data.Value.RemoveReaction
        }

    let asyncRemoveComment (recenzoId: string) : Async<bool> =
        async {
            use runtimeContext = getContext()
            let! result = removeCommentOperation.AsyncRun(runtimeContext,
                                    recenzoId = recenzoId
                                    //recenzoId = "67e8d007-002c-4541-9ca8-0983780cc4d6"
                                    )
            
            return result.Data.Value.RemoveComment
        }

    let asyncRemoveRecenzo (recenzoId: string) : Async<bool> =
        async {
            use runtimeContext = getContext()
            let! result = removeRecenzoOperation.AsyncRun(runtimeContext,
                                    recenzoId = recenzoId
                                    //recenzoId = "67e8d007-002c-4541-9ca8-0983780cc4d6"
                                    )
            
            return result.Data.Value.RemoveRecenzo
        }
