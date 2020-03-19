 //Learn more about F# at http://fsharp.org
namespace BibliotekoClient

module GraphQLProviderRequests =

    open FSharp.Data.GraphQL
    open Domain

    type BiblProvider = GraphQLProvider<"http://localhost:7071/GraphQL">

    let getRegistriOperation = BiblProvider.Operation<"queries/getregistri.graphql">()
    let getRegistrisOperation = BiblProvider.Operation<"queries/getregistris.graphql">()
    let getBibliotekosOperation = BiblProvider.Operation<"queries/getbibliotekos.graphql">()
    let addPetskriboOperation = BiblProvider.Operation<"queries/addpetskribo.graphql">()
    let setReactionOperation = BiblProvider.Operation<"queries/setreaction.graphql">()
    let removeReactionOperation = BiblProvider.Operation<"queries/removereaction.graphql">()

    let asyncQueryRegistriByIsbn () : Async<Registri> =
        async {
            let! result = getRegistriOperation.AsyncRun(isbn = "978-1617291326")
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
            let! result = getRegistrisOperation.AsyncRun()
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

    let asyncQueryBibliotekos =
        async {
            let! result = getBibliotekosOperation.AsyncRun()
            return result
            }

    let asyncAddPetskriboToBiblioteko =
        async {
            //use context = getContext()
            let! result = addPetskriboOperation.AsyncRun(//context,
                                            bibliotekoId = "b5a21dc6-8a84-4ce2-8563-74a118449693",
                                            petskribo = BiblProvider.Types.InputPetskribo(
                                                        id = "c894880c-4f74-423f-b0eb-80381e586ea9",
                                                        isbn = "978-1680502541"),
                                            userId = "2fa50fd0-acae-415a-b664-d8f8da1470c5"
                                           )
            return result
        }

    //let prettifyJson (jsonText:string) =
    //    JValue.Parse(jsonText).ToString(Formatting.Indented)

    let asyncSetReaction =
        async {
            let! result = setReactionOperation.AsyncRun(
                                            reviewId = "B745CA41-CDA1-43CB-AFE6-AF5F69099DA7",
                                            isbn = "978-1617291326",
                                            reaction = BiblProvider.Types.ReactionEnum.LoveIt,
                                            userId = "2fa50fd0-acae-415a-b664-d8f8da1470c5"
                                            )
            return result
        }

    let asyncRemoveReaction =
        async {
            let! result = removeReactionOperation.AsyncRun(reviewId = "B745CA41-CDA1-43CB-AFE6-AF5F69099DA7")
            return result
        }
