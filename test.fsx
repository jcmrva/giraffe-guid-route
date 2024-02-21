
#r "nuget: FsHttp"
open FsHttp

// expected & got 200
let tryGuid =
    http {
        GET $"http://localhost:5000/try-a-guid/{System.Guid.NewGuid()}"
    }
    |> Request.send

// expected & got 200
let tryShortGuid =
    let giraffeIssueExample = "IxC0lRDiNkqa_xsaUWbT-Q"
    http {
        GET $"http://localhost:5000/try-a-guid/{giraffeIssueExample}"
    }
    |> Request.send

// expected 404, got 500
let tryInvalidValue =
    let fuzzerInput = "8b3557db-fa-c0c90785ec0b"
    http {
        GET $"http://localhost:5000/try-a-guid/{fuzzerInput}"
    }
    |> Request.send

// expected & got 404
let tryMisc =
    let whatever = "$$$$$$$$$$$$$$$$$$$$$$"
    http {
        GET $"http://localhost:5000/try-a-guid/{whatever}"
    }
    |> Request.send

