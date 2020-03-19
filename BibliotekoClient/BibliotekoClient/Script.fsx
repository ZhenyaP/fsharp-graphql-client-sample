#r @"C:\Users\Andrii\Dev\GitHub\FSharp.Data.GraphQL\src\FSharp.Data.GraphQL.Client\bin\Debug\net461\FSharp.Data.GraphQL.Shared.dll"
#r @"C:\Users\Andrii\Dev\GitHub\FSharp.Data.GraphQL\src\FSharp.Data.GraphQL.Client\bin\Debug\net461\FSharp.Data.GraphQL.Client.dll"
#load @"Domain.fs"
#load @"GraphQLProviderRequests.fs"

GraphQLProviderRequests.asyncQueryRegistriByIsbn() |> Async.RunSynchronously |> printfn "%A"
