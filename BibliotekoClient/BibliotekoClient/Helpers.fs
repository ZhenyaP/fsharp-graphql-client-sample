namespace BibliotekoClient

open FSharp.Data.GraphQL
open Newtonsoft.Json

module Helpers =

    let checkForErrors(result:OperationResultBase) (operationName:string) =
        if result.Errors.Length > 0 then printf "%s operation failed with error: %A\n" operationName result.Errors
        if result.CustomData.Count > 0 then printf "%s operation failed with error: %A\n" operationName result.CustomData

    let prettifyJson (object:obj) =
        let json = JsonConvert.SerializeObject(object, Formatting.Indented)
        json

    let printHeader (headerText : string) = 
        printfn "---------------------------------------------"
        printfn "%s" headerText
        printfn "---------------------------------------------"
        printfn ""
