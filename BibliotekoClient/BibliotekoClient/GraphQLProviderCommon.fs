 //Learn more about F# at http://fsharp.org
namespace BibliotekoClient

open System
open System.Net

module GraphQLProviderCommon =

    open FSharp.Data.GraphQL

    type BiblProvider = GraphQLProvider<"json/introspection.json">
    //type BiblProvider = GraphQLProvider<"http://localhost:7071/GraphQL">

    let private createContext endpointUrl accessToken =
        let authorizationHeaderName = Enum.GetName(typeof<HttpRequestHeader>, HttpRequestHeader.Authorization)
        let headers = seq { yield (authorizationHeaderName, "Bearer " + accessToken) }

        let context = BiblProvider.GetContext(serverUrl = endpointUrl, httpHeaders = headers)
        context

    let getContext() =
        let context = createContext "http://localhost:7071/GraphQL" "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJvaWQiOiI2Nzc5RTVCQS0zODA3LTQ5MTQtOURFRi1GMDREMDVEQjRFQTAiLCJuYmYiOjE1ODcxMTQ5NjUsImV4cCI6MTY1MDE4Njk2NSwiaWF0IjoxNTg3MTE0OTY1LCJpc3MiOiJodHRwOi8vbG9jYWxob3N0IiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdCJ9.j0-0e1Hey3Nx9HjIoRex_VuTmzzOpgah4ocYIN_7UYM"
        context
