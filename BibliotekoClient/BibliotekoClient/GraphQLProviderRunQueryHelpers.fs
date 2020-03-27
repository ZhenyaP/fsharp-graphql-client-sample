namespace BibliotekoClient

open Helpers

module GraphQLProviderRunQueryHelpers =

    let getRegistriByIsbn (isbn:string) (extraHeaderText: string) =
        printHeader (sprintf "Registri query%s:" extraHeaderText)
        GraphQLProviderQueries.asyncQueryRegistriByIsbn isbn
        |> Async.RunSynchronously
        |> printfn "%A"

    let getRegistrij (extraHeaderText: string) =
        printHeader (sprintf "Registrij query%s:" extraHeaderText)
        GraphQLProviderQueries.asyncQueryRegistrij ()
        |> Async.RunSynchronously
        |> printfn "%A"

    let getBibliotekoj (extraHeaderText: string) =
        printHeader (sprintf "Bibliotekoj query%s:" extraHeaderText)
        GraphQLProviderQueries.asyncQueryBibliotekoj ()
            |> Async.RunSynchronously
            |> printfn "%A"

