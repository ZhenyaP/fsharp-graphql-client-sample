 //Learn more about F# at http://fsharp.org
namespace BibliotekoClient

open Newtonsoft.Json.Linq
open Newtonsoft.Json
open System

module GraphQLProviderRequests =

    open FSharp.Data.GraphQL
    open Domain
    open Chessie.ErrorHandling

    type BiblProvider = GraphQLProvider<"json/introspection.json">

    let addPetskriboOperation = BiblProvider.Operation<"queries/addPetskribo.graphql">()
    let getBibliotekosOperation = BiblProvider.Operation<"queries/getBibliotekos.graphql">()
    let getRegistriOperation = BiblProvider.Operation<"queries/getRegistri.graphql">()
    let getRegistrisOperation = BiblProvider.Operation<"queries/getRegistris.graphql">()  
    let setReactionOperation = BiblProvider.Operation<"queries/setReaction.graphql">()
    let removeReactionOperation = BiblProvider.Operation<"queries/removeReaction.graphql">()
    let removeCommentOperation = BiblProvider.Operation<"queries/removeComment.graphql">()
    let removeRecenzoOperation = BiblProvider.Operation<"queries/removeRecenzo.graphql">()
    
    let getContext() =
        let context = BiblProvider.GetContext(serverUrl = "http://localhost:7071/GraphQL")
        context

    let asyncQueryRegistriByIsbn () : Async<Registri> =
        async {
            use runtimeContext = getContext()
            let! result = getRegistriOperation.AsyncRun(runtimeContext, isbn = "978-1617291326")
            if result.Errors.Length > 0 then failwith (sprintf "getRegistriByIsbn query operation failed with error: %A\n" result.Errors)
            let registriEntity = result.Data.Value.Registri.Value
            //TODO: Make GraphQLProvider generate Guid .NET type for GraphQL field with ID type
            // to avoid casting string to Guid
            let registri = { Id = registriEntity.Id |> System.Guid.Parse
                             ISBN = registriEntity.Isbn
                             Title = registriEntity.Title
                             Authors = registriEntity.Authors |> Array.toList
                             Summary = registriEntity.Summary
                             ImageURL = registriEntity.Imageurl }

            return registri
            }

    let asyncQueryRegistris () : Async<Registri list> =
        async {
            use runtimeContext = getContext()
            let! result = getRegistrisOperation.AsyncRun(runtimeContext)
            if result.Errors.Length > 0 then failwith (sprintf "getRegistris query operation failed with error: %A\n" result.Errors)
            let registriEntities = result.Data.Value.Registris
            let registries = registriEntities |> Array.map(fun registriEntity ->
            //TODO: Make GraphQLProvider generate Guid .NET type for GraphQL field with ID type
            // to avoid casting string to Guid
                            {   Id = registriEntity.Id |> System.Guid.Parse
                                ISBN = registriEntity.Isbn
                                Title = registriEntity.Title
                                Authors = registriEntity.Authors |> Array.toList
                                Summary = registriEntity.Summary
                                ImageURL = registriEntity.Imageurl }) |> Array.toList

            return registries
            }

    let asyncQueryBibliotekos () : Async<Biblioteko list> =
        async {
            use runtimeContext = getContext()
            let! result = getBibliotekosOperation.AsyncRun(runtimeContext)
            let bibliotekos = result.Data.Value.Bibliotekos |> Array.map(fun bibliotekoEntity ->
                            {   
                                Id = bibliotekoEntity.Id |> System.Guid.Parse
                                Address = { 
                                            FreeformAddress = bibliotekoEntity.Address.Freeformaddress
                                            Country = bibliotekoEntity.Address.Country
                                            CountryCode = bibliotekoEntity.Address.Countrycode
                                            Street = bibliotekoEntity.Address.Street
                                            BuildingNumber = bibliotekoEntity.Address.Buildingnumber
                                            PostalCode = bibliotekoEntity.Address.Postalcode 
                                          }
                                Content = bibliotekoEntity.Content 
                                        |> Array.map(
                                            fun petskriboEntity ->
                                                let ownedEntity = petskriboEntity.AsOwned()
                                                let registriEntity = ownedEntity.Registri
                                                let registri = 
                                                    {   
                                                        Id = registriEntity.Id |> System.Guid.Parse
                                                        ISBN = registriEntity.Isbn
                                                        Title = registriEntity.Title
                                                        Authors = registriEntity.Authors |> Array.toList
                                                        Summary = registriEntity.Summary
                                                        ImageURL = registriEntity.Imageurl 
                                                    }
                                                let result =
                                                    Owned {
                                                            Id = ownedEntity.Id |> System.Guid.Parse
                                                            Registri = registri
                                                            Logbook = ownedEntity.Logbook |> Array.toList
                                                            Owner = ownedEntity.Owner |> System.Guid.Parse
                                                          }
                                                result) |> Array.toList
                            })

            return bibliotekos |> Array.toList
            //return result.Data.Value.Bibliotekos.[0].Content.[0].AsBorrowed().Registri.Reviews.[0].Content.AsReaction().Reaction
        }

    let asyncAddPetskriboToBiblioteko () : Async<Petskribo> =
        async {
            use runtimeContext = getContext()            
            let! result = addPetskriboOperation.AsyncRun(runtimeContext,
                                            bibliotekoId = "b5a21dc6-8a84-4ce2-8563-74a118449693",
                                            petskribo = BiblProvider.Types.InputPetskribo(
                                                        id = "c894880c-4f74-423f-b0eb-80381e586ea9",
                                                        isbn = "978-1680502541"),
                                            uzantoId = "2fa50fd0-acae-415a-b664-d8f8da1470c5"
                                           )
            let petskriboEntity = result.Data.Value.AddPetskribo.Value
            let registriEntity = petskriboEntity.Registri
            let registri = 
                {   
                    Id = registriEntity.Id |> System.Guid.Parse
                    ISBN = registriEntity.Isbn
                    Title = registriEntity.Title
                    Authors = registriEntity.Authors |> Array.toList
                    Summary = registriEntity.Summary
                    ImageURL = registriEntity.Imageurl 
                }
            let petskribo = 
                {
                    Id = petskriboEntity.Id |> System.Guid.Parse
                    Registri = registri
                    Logbook = petskriboEntity.Logbook |> Array.toList
                    Owner = petskriboEntity.Owner |> System.Guid.Parse
                }

            return petskribo
        }

    let asyncSetReaction () : Async<Recenzo> =
        async {
            use runtimeContext = getContext()
            let! result = setReactionOperation.AsyncRun(runtimeContext,
                                                        isbn = "978-1617291326",
                                                        recenzoId = "67e8d007-002c-4541-9ca8-0983780cc4d6",
                                                        reactionKind = BiblProvider.Types.ReactionEnum.LoveIt,
                                                        uzantoId = "")  //userId variable value is set in GraphQL server
            let recenzoEntity = result.Data.Value.SetReaction.Value
            let reactionName = recenzoEntity.Content.AsReaction().Reaction.GetName()
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
                    Content = Reaction reaction
                }
                
            return recenzo
        }

    let asyncRemoveReaction () =
        async {
            use runtimeContext = getContext()
            let! result = removeReactionOperation.AsyncRun(runtimeContext, recenzoId = "67e8d007-002c-4541-9ca8-0983780cc4d6")
            
            return result.Data.Value.RemoveReaction
        }

    let asyncRemoveComment () =
        async {
            use runtimeContext = getContext()
            let! result = removeCommentOperation.AsyncRun(runtimeContext, recenzoId = "67e8d007-002c-4541-9ca8-0983780cc4d6")
            
            return result.Data.Value.RemoveComment
        }

    let asyncRemoveRecenzo () =
        async {
            use runtimeContext = getContext()
            let! result = removeRecenzoOperation.AsyncRun(runtimeContext, recenzoId = "67e8d007-002c-4541-9ca8-0983780cc4d6")
            
            return result.Data.Value.RemoveRecenzo
        }
