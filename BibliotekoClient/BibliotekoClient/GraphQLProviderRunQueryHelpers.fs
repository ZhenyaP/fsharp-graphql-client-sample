namespace BibliotekoClient

module GraphQLProviderRunQueryHelpers =

    let getRegistriByIsbn (isbn:string) =
        printfn ""
        printfn "Registri By ISBN:"
        printfn "---------------------------------------------"
        GraphQLProviderQueries.asyncQueryRegistriByIsbn isbn 
        |> Async.RunSynchronously 
        |> printfn "%A"

    let getRegistrij () =
        printfn ""
        printfn "---------------------------------------------"
        printfn "Registrij:"
        printfn "---------------------------------------------"
        printfn ""
        GraphQLProviderQueries.asyncQueryRegistrij ()
        |> Async.RunSynchronously
        |> printfn "%A"

    let getBibliotekoj () =
        printfn ""
        printfn "---------------------------------------------"
        printfn "Bibliotekos - Before addPetskribo mutation:"
        printfn "---------------------------------------------"
        printfn ""
        GraphQLProviderQueries.asyncQueryBibliotekoj ()
            |> Async.RunSynchronously
            |> printfn "%A"

