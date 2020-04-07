 //Learn more about F# at http://fsharp.org
namespace BibliotekoClient

module GraphQLProviderCommon =

    open FSharp.Data.GraphQL

    type BiblProvider = GraphQLProvider<"json/introspection.json">
    //type BiblProvider = GraphQLProvider<"http://localhost:7071/GraphQL">

    let getContext() =
        //let context = BiblProvider.GetContext(serverUrl = "https://bibliotekofunctions20200328112914.azurewebsites.net/GraphQL")
        //let context = BiblProvider.GetContext(serverUrl = "https://brzfrebo.azurewebsites.net/GraphQL")
        let headers = seq { "Authorization", "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImtpZCI6Ilg1ZVhrNHh5b2pORnVtMWtsMll0djhkbE5QNC1jNTdkTzZRR1RWQndhTmsifQ.eyJpc3MiOiJodHRwczovL2JyemZyZWJvLmIyY2xvZ2luLmNvbS81ZTRjMDgzZS0yNGNjLTRhZGMtOGFhMy00OTI4YjgyZDk4ZTMvdjIuMC8iLCJleHAiOjE1ODU2OTEyNzEsIm5iZiI6MTU4NTY4NzY3MSwiYXVkIjoiZDNiNTMzZjUtNjQwZS00M2EwLTg2OTAtODM0ODAzMzA2MjA0IiwibmFtZSI6ItCQ0L3QtNGA0LXQuSDQp9C10LHRg9C60LjQvSIsImlkcCI6ImZhY2Vib29rLmNvbSIsIm9pZCI6ImJkMjg5OTY3LWZjNjctNDY5OC05ODViLWUxNGI1ZGQ4YWM0MCIsInN1YiI6ImJkMjg5OTY3LWZjNjctNDY5OC05ODViLWUxNGI1ZGQ4YWM0MCIsImV4dGVuc2lvbl9QaG9uZSI6IiszODA5MTQ1MDk1OTQiLCJlbWFpbHMiOlsieHBlcmlhbmRyaUBsaXZlLnJ1Il0sInRmcCI6IkIyQ18xX1NVU0kiLCJzY3AiOiJ1c2VyX2ltcGVyc29uYXRpb24iLCJhenAiOiI1M2MyZjYxNy05NGYzLTRhZmYtOTJkNC0yMTVjYjM0NjRlMjMiLCJ2ZXIiOiIxLjAiLCJpYXQiOjE1ODU2ODc2NzF9.TAFV7L9odQucHmkXuIoPfcPDZcMd_CukQcSGORUrvahLA7uDFEjLqjhYpzZ-xsZjsyQJMOFlOZXhlZuqyBtvptEd4LhHPUiKJy3h2FYZl5zbO2ghsUgUMVr3zOKX4aqReJzhd5s8LFLlvV__3Dmksd5GoiApCadqYYnOcj7sWlxahWhtpQsmaFSWHDz_Z8iZiqFZWNcQryyVr4bPQGCNJFYm25-oJEqpbdRtoAWv0Mu_K51kgrZwROSQcXLMgYOR_oIVaGPvA55LmDSms5GYe8QpPcqxWyqMPjOXhtnA0jatVVavIXnVVvZda2qAoI1Yb0oeoGNY06t683HGQadHUg" }
        let context = BiblProvider.GetContext(serverUrl = "http://localhost:7071/GraphQL", httpHeaders = headers)
        context
