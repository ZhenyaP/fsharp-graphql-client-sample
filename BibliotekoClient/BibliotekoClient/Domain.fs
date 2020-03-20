// Learn more about F# at http://fsharp.org
namespace BibliotekoClient

open Chessie.ErrorHandling.Trial

module Domain =

    type Registri =
        { Id : System.Guid
          ISBN : string
          Title : string
          Authors : string list
          Summary : string
          ImageURL : System.Uri }
          with
            interface BaseDomainType
            static member Default = { Id = System.Guid.Empty
                                      ISBN = ""
                                      Title = ""
                                      Authors = list.Empty
                                      Summary = ""
                                      ImageURL = null }
    and BaseDomainType = interface end

    type Reaction =
        | Undefined = 0
        | LoveIt = 1
        | Boring = 2
        | Gripping = 3
        | Moving = 4
        | Inspiring = 5
        | Classic = 6
    
    [<Struct>]
    type Comment = private Comment of string // TODO: Add string min length restrictions
    
    module Comment =
    
        let create text =
            // TODO: add validation logic
            ok (Comment text)
    
        let value (Comment text) = text

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

    /// Recenzo is a review
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

