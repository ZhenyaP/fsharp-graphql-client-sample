namespace BibliotekoClient

open FSharp.Data.GraphQL

module Helpers =

    let checkForErrors(result:OperationResultBase) (operationName:string) =
        if result.Errors.Length > 0 then failwith (sprintf "%s operation failed with error: %A\n" operationName result.Errors)
        if result.CustomData.Count > 0 then failwith (sprintf "%s operation failed with error: %A\n" operationName result.CustomData)

