 //Learn more about F# at http://fsharp.org
namespace BibliotekoClient

module GraphQLProviderRequests =

    open FSharp.Data.GraphQL
    open Domain

    type BiblProvider = GraphQLProvider<"json/introspection.json">

    let getRegistriOperation = BiblProvider.Operation<"queries/getregistri.graphql">()
    let getRegistrisOperation = BiblProvider.Operation<"queries/getregistris.graphql">()
    let getBibliotekosOperation = BiblProvider.Operation<"queries/getbibliotekos.graphql">()
    let addPetskriboOperation = BiblProvider.Operation<"queries/addpetskribo.graphql">()
    let setReactionOperation = BiblProvider.Operation<"queries/setreaction.graphql">()
    let removeReactionOperation = BiblProvider.Operation<"queries/removereaction.graphql">()

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

    let asyncQueryBibliotekos () =
        async {
            use runtimeContext = getContext()
            let! result = getBibliotekosOperation.AsyncRun(runtimeContext)

            return result.Data.Value.Bibliotekos
        }

    let asyncAddPetskriboToBiblioteko () =
        async {
            use runtimeContext = getContext()
            let! result = addPetskriboOperation.AsyncRun(runtimeContext,
                                            bibliotekoId = "b5a21dc6-8a84-4ce2-8563-74a118449693",
                                            petskribo = BiblProvider.Types.InputPetskribo(
                                                        id = "c894880c-4f74-423f-b0eb-80381e586ea9",
                                                        isbn = "978-1680502541"),
                                            userId = "2fa50fd0-acae-415a-b664-d8f8da1470c5"
                                           )

            return result.Data.Value.AddPetskribo.Value
        }

    //let prettifyJson (jsonText:string) =
    //    JValue.Parse(jsonText).ToString(Formatting.Indented)

    let asyncSetReaction () =
        async {
            use runtimeContext = getContext()
            let! result = setReactionOperation.AsyncRun(runtimeContext,
                                                        isbn = "978-1617291326",
                                                        reaction = BiblProvider.Types.InputReaction(
                                                            BiblProvider.Types.ReactionEnum.LoveIt,
                                                            "67e8d007-002c-4541-9ca8-0983780cc4d6"),
                                                        userId = "")  //userId variable value is set in GraphQL server
            
            return result.Data.Value.SetReaction.Value
        }

    let asyncRemoveReaction () =
        async {
            use runtimeContext = getContext()
            let! result = removeReactionOperation.AsyncRun(runtimeContext, reviewId = "67e8d007-002c-4541-9ca8-0983780cc4d6")
            
            return result.Data.Value.RemoveReaction
        }
