 //Learn more about F# at http://fsharp.org
namespace BibliotekoClient

open Newtonsoft.Json.Linq
open Newtonsoft.Json
open System
open Domain
open GraphQLProviderCommon

module GraphQLProviderQueries =

    let getBibliotekosOperation = BiblProvider.Operation<"queries/getBibliotekos.graphql">()
    let getRegistriOperation = BiblProvider.Operation<"queries/getRegistri.graphql">()
    let getRegistrisOperation = BiblProvider.Operation<"queries/getRegistris.graphql">()  

    let asyncQueryRegistriByIsbn (isbn: string) : Async<Registri> =
        async {
            use runtimeContext = getContext()
            let! result = getRegistriOperation.AsyncRun(runtimeContext,
                                                        isbn = isbn
                                                            //isbn = "978-1617291326"
                                                        )
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
