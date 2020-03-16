 //Learn more about F# at http://fsharp.org
namespace BibliotekoClient

module GraphQLProviderRequests =

    open FSharp.Data.GraphQL
    open Domain

    type BiblProvider = GraphQLProvider<"http://localhost:7071/GraphQL">
        
    let getRegistriOperation = BiblProvider.Operation<"getregistri.graphql">()
    let getRegistrisOperation = BiblProvider.Operation<"getregistris.graphql">()

    let asyncQueryRegistriByIsbn () : Async<Registri> =    
        async {         
            let! result = getRegistriOperation.AsyncRun(isbn = "978-1617291326")
            if result.Errors.Length > 0 then failwith (sprintf "getRegistriByIsbn query operation failed with error: %A\n" result.Errors)
            let registriEntity = result.Data.Value.Registri.Value
            //TODO: Make GraphQLProvider generate Guid .NET type for GraphQL field with ID type
            // to avoid casting string to Guid
            let registri = {    Id = registriEntity.Id |> System.Guid.Parse
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

    //let asyncRunQuery<'T when 'T :> BaseDomainType> (query: Async<BiblProvider.Operations.Q.OperationResult * 'T>) =
    (*
    let asyncRunQuery (query: Async<BiblProvider.Operations.Q.OperationResult>) =
        async {
            let! result = query
            //let! result, domainEntity = query
            //let registri = result.Data.Value.Registri.Value
            //printfn "%s: %A\n" typeof<'T>.DeclaringType.Name domainEntity
            //sprintf "%A" (match result.Data with | Some x -> x | None -> failwith "result.Data is None") |> prettifyJson |> printfn "Data: %s\n"
            printfn "Data: %A\n" result.Data
            printfn "Errors: %A\n" result.Errors
            printfn "Custom data: %A\n" result.CustomData
        }
    *)

    (*
    let asyncQueryBibliotekos =
        async {
            use context = getContext()
            let operation = BiblProvider.Operation<"getbibliotekos.graphql">()            
            let! result = operation.AsyncRun(context)
            let! result = operation.AsyncRun()
            return result
            }

    let asyncAddPetskriboToBiblioteko =
        async {
            use context = getContext()
            let operation = BiblProvider.Operation<"addpetskribo.graphql">()
            let! result = operation.AsyncRun(//context, 
                                            bibliotekoID = "b5a21dc6-8a84-4ce2-8563-74a118449693", 
                                            petskribo = BiblProvider.Types.InputPetskribo(
                                                        id = "c894880c-4f74-423f-b0eb-80381e586ea9", isbn = "978-1680502541"),
                                            UserId = "2fa50fd0-acae-415a-b664-d8f8da1470c5"
                                           )
           return result
        }

    let prettifyJson (jsonText:string) =
        JValue.Parse(jsonText).ToString(Formatting.Indented)
*)
    
