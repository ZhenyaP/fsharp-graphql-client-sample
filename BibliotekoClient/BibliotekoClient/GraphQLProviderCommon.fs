 //Learn more about F# at http://fsharp.org
namespace BibliotekoClient

module GraphQLProviderCommon =

    open FSharp.Data.GraphQL

    type BiblProvider = GraphQLProvider<"json/introspection.json">

    let getContext() =
        let context = BiblProvider.GetContext(serverUrl = "http://localhost:7071/GraphQL")
        context
