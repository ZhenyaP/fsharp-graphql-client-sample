 //Learn more about F# at http://fsharp.org
namespace BibliotekoClient

module GraphQLProviderCommon =

    open FSharp.Data.GraphQL

    type BiblProvider = GraphQLProvider<"json/introspection.json">

    let getContext() =
        let context = BiblProvider.GetContext(serverUrl = "https://bibliotekofunctions20200328112914.azurewebsites.net/GraphQL")
        //let context = BiblProvider.GetContext(serverUrl = "https://brzfrebo.azurewebsites.net/GraphQL")
        //let context = BiblProvider.GetContext(serverUrl = "http://localhost:7071/GraphQL")
        context
