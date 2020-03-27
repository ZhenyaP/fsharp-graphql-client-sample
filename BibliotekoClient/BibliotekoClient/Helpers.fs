namespace BibliotekoClient

open FSharp.Data.GraphQL
open Newtonsoft.Json

module Helpers =

    let checkForErrors(result:OperationResultBase) (operationName:string) : bool =
        if result.Errors.Length > 0 then
            printf "%s operation failed with error: %A\n" operationName result.Errors
            true
        elif result.CustomData.Count > 0 then
            printf "%s operation failed with error: %A\n" operationName result.CustomData
            true
        else false

    let prettifyJson (object:obj) =
        let json = JsonConvert.SerializeObject(object, Formatting.Indented)
        json

    let printHeader (headerText : string) =
        printfn "---------------------------------------------"
        printfn "%s" headerText
        printfn "---------------------------------------------"
        printfn ""
