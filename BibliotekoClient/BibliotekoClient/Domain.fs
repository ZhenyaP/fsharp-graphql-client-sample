// Learn more about F# at http://fsharp.org
namespace BibliotekoClient

module Domain =

    type Registri =    
        { Id : System.Guid 
          ISBN : string
          Title : string
          Authors : string list
          Summary : string
          ImageURL : string }
          with
            interface BaseDomainType
            static member Default = { Id = System.Guid.Empty
                                      ISBN = ""
                                      Title = ""
                                      Authors = list.Empty
                                      Summary = ""
                                      ImageURL = "" }
    and BaseDomainType = interface end

    type PetskriboPosession =
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
          Content : PetskriboPosession list
          Address : Address }