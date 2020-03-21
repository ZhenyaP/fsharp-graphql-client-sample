// Learn more about F# at http://fsharp.org
namespace BibliotekoClient

module GraphQLClientRequests =
    open FSharp.Data
    open FSharp.Data.GraphQL
    open Domain

    //type BiblProvider = GraphQLProvider<"http://localhost:7071/GraphQL">

    //type registriData = {
    //    registri: BiblProvider.Types.Registri
    //}

    //type registriResponse = {
    //    data: registriData
    //    documentId: int
    //    requestType: string
    //}

    type InputPetskribo = { isbn : string
                            id : System.Guid}

    type RegistriResponse = JsonProvider<"responses/getregistri_response.json">
    type RegistrisResponse = JsonProvider<"responses/getregistris_response.json">
    type BibliotekosResponse = JsonProvider<"responses/getbibliotekos_response.json">
    type AddPetskriboResponse = JsonProvider<"responses/addpetskribo_response.json">

    let asyncQueryRegistriByIsbn (isbn:string) : Async<Registri> =
        async {
            // Dispose the connection after using it.
            use connection = new GraphQLClientConnection()
            let request : GraphQLRequest =
                { Query = """
                query q($isbn: String!) {
                    registri(isbn: $isbn) {
                        id
                        isbn
                        title
                        authors
                        summary
                        imageurl
                    }
                }
                        """
                  Variables = [|("isbn", isbn |> box)|]
                  ServerUrl = "http://localhost:7071/GraphQL"
                  HttpHeaders = [||]
                  OperationName = Some "q" }
            let! response = GraphQLClient.sendRequestAsync connection request
            let getRegistriResponse = RegistriResponse.Parse(response)
            let registriEntity = getRegistriResponse.Data.Registri
            let registri = { Id = registriEntity.Id
                             ISBN = registriEntity.Isbn.ToString()
                             Title = registriEntity.Title
                             Authors = registriEntity.Authors |> Array.toList
                             Recenzos = []
                             Summary = registriEntity.Summary
                             ImageURL = registriEntity.Imageurl |> System.Uri }

            return registri
        }

    let asyncQueryRegistries () : Async<Registri list> =
        async {
            // Dispose the connection after using it.
            use connection = new GraphQLClientConnection()
            let request : GraphQLRequest =
                { Query = """
                query q {
                    registris {
                        id
                        isbn
                        title
                        authors
                        summary
                        imageurl
                    }
                }"""
                  Variables = [||]
                  ServerUrl = "http://localhost:7071/GraphQL"
                  HttpHeaders = [||]
                  OperationName = Some "q" }
            let! response = GraphQLClient.sendRequestAsync connection request
            let getRegistrisResponse = RegistrisResponse.Parse(response)
            let registris = getRegistrisResponse.Data.Registris |> Array.map(fun registri ->
                           { Id = registri.Id
                             ISBN = registri.Isbn.ToString()
                             Title = registri.Title
                             Authors = registri.Authors |> Array.toList
                             Recenzos = []
                             Summary = registri.Summary
                             ImageURL = registri.Imageurl |> System.Uri }) |> Array.toList

            return registris
          }

    let asyncQueryBibliotekos () : Async<Biblioteko list> =
        async {
            use connection = new GraphQLClientConnection()
            let request : GraphQLRequest =
                {   Query = """
query getBibliotekos {
  bibliotekos {
    id
    content {
        ...borrowedFields
        ...ownedFields
        ...lentFields
    }
    address {
        freeformaddress
        country
        countrycode
        street
        buildingnumber
        postalcode
    }
  }
}

fragment borrowedFields on Borrowed {
    id
    registri {
        id
        isbn
        title
        authors
        summary
        imageurl
    }
    logbook
    owner
}

fragment ownedFields on Owned {
    id
    registri {
        id
        isbn
        title
        authors
        summary
        imageurl
    }
    logbook
    owner
}

fragment lentFields on Lent {
    id
    registri {
        id
        isbn
        title
        authors
        summary
        imageurl
    }
    logbook
    owner
}"""
                    Variables = [||]
                    ServerUrl = "http://localhost:7071/GraphQL"
                    HttpHeaders = [||]
                    OperationName = Some "q" }
            let! response = GraphQLClient.sendRequestAsync connection request
            let getBibliotekosResponse = BibliotekosResponse.Parse(response)
            let bibliotekos =
                getBibliotekosResponse.Data.Bibliotekos
                |> Array.map (fun b ->
                    {
                        Id = b.Id
                        Address =
                            {
                                FreeformAddress = b.Address.Freeformaddress
                                Country = b.Address.Country
                                CountryCode = b.Address.Countrycode
                                Street = b.Address.Street
                                BuildingNumber = b.Address.Buildingnumber
                                PostalCode = b.Address.Postalcode
                            }
                        Content = b.Content |>
                                     Array.map(fun c ->
                                                 Owned {
                                                            Id = c.Id
                                                            Logbook = c.Logbook |> Array.toList
                                                            Owner = c.Owner
                                                            Registri =
                                                            {
                                                                Id = c.Registri.Id
                                                                ISBN = c.Registri.Isbn.ToString()
                                                                Title = c.Registri.Title
                                                                Authors = c.Registri.Authors |> Array.toList
                                                                Recenzos = []
                                                                Summary = c.Registri.Summary
                                                                ImageURL = c.Registri.Imageurl |> System.Uri
                                                            }
                                                        })
                                            |> Array.toList
                    })
                |> Array.toList

            return bibliotekos
        }

    let asyncAddPetskriboToBiblioteko (bibliotekoID: System.Guid) (petskribo : InputPetskribo) =
        async {
            // Dispose the connection after using it.
            use connection = new GraphQLClientConnection()
            let request : GraphQLRequest =
                { Query = """
mutation addPetskriboToBiblioteko(
  $bibliotekoID: String!
  $petskribo: InputPetskribo!
  $UserId: String!
) {
  addPetskribo(
    bibliotekoID: $bibliotekoID
    petskribo: $petskribo
    UserId: $UserId
  ) {
    id
    registri {
      isbn
      title
      authors
      summary
    }
    logbook
    owner
  }
}
"""
                  Variables =
                    [| ("bibliotekoID", bibliotekoID |> box)
                       ("petskribo", petskribo |> box)
                       ("UserId", "2fa50fd0-acae-415a-b664-d8f8da1470c5" |> box) |]
                  ServerUrl = "http://localhost:7071/GraphQL"
                  HttpHeaders = [||]
                  OperationName = Some "m" }
            let! response = GraphQLClient.sendRequestAsync connection request
            let addPetskriboResponse = AddPetskriboResponse.Parse(response)
            return addPetskriboResponse.Data.AddPetskribo
        }

