// Learn more about F# at http://fsharp.org
namespace BibliotekoClient

open Chessie.ErrorHandling.Trial
open GraphQLProviderCommon

module Domain =

    [<Struct>]
    type Comment = private Comment of string // TODO: Add string min length restrictions

    module Comment =

        let create text =
            // TODO: add validation logic
            ok (Comment text)

        let value (Comment text) = text

    type Registri =
        { Id : System.Guid
          ISBN : string
          Title : string
          Authors : string list
          Recenzos : Recenzo list
          Summary : string
          ImageURL : System.Uri }
          with
            interface BaseDomainType
            static member Default = { Id = System.Guid.Empty
                                      ISBN = ""
                                      Title = ""
                                      Authors = list.Empty
                                      Recenzos = list.Empty
                                      Summary = ""
                                      ImageURL = null }
    and BaseDomainType = interface end

    and Recenzo =
        { Id : System.Guid
          /// Reviewer
          Recenzorer : string
          Content : RecenzoContent }

    /// <remarks>
    /// a review can be either
    /// (a) both reaction and comment
    /// (b) only reaction
    /// (c) only comment
    /// </remarks>
    and RecenzoContent =
        | Reaction of Reaction : Reaction
        | Comment of Comment : Comment
        | ReactionAndComment of Reaction : Reaction * Comment : Comment

    and Reaction =
        | Undefined = 0
        | LoveIt = 1
        | Boring = 2
        | Gripping = 3
        | Moving = 4
        | Inspiring = 5
        | Classic = 6

    type PetskriboPossession =
        | Borrowed of Petskribo
        | Owned of Petskribo
        | Lent of Petskribo

    and Petskribo =
        { Id : System.Guid
          Registri : Registri
          Logbook : string list
          Owner : System.Guid }

    and Address =
        { FreeformAddress : string
          Country : string
          CountryCode : string
          Street : string
          BuildingNumber : string
          PostalCode : string }

    and Biblioteko =
        { Id : System.Guid
          Content : PetskriboPossession list
          Address : Address }

