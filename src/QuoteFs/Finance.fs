namespace QuoteFs

module Finance = 

    open QuoteFs.FinancialModelingPrep
    open FSharp.Data
    open Thoth.Json.Net

    let getStockQuoteAsync(symbol) =
        let url = sprintf "%s/api/v3/company/profile/%s" baseUrl symbol
        async {
            let! response = Http.AsyncRequestString url
            return match Decode.Auto.fromString<quoteResponse> response with
                    | Ok rawQuote -> Ok (toStockQuote rawQuote)
                    | Error error -> Error error
        } |> Async.StartAsTask

    let getStockQuote(symbol) =
            getStockQuoteAsync(symbol)
            |> Async.AwaitTask
            |> Async.RunSynchronously

    let getIndexAsync(ticker) =
        let url = sprintf "%s/api/v3/majors-indexes/%s" baseUrl ticker
        async {
            let! response = Http.AsyncRequestString url
            return match Decode.Auto.fromString<indexResponse> response with
                    | Ok rawQuote -> Ok (toIndex rawQuote)
                    | Error error -> Error error
        } |> Async.StartAsTask

    let getIndex(ticker) =
        getIndexAsync(ticker)
        |> Async.AwaitTask
        |> Async.RunSynchronously

    let majorIndexes =
        [
            ".DJI";  // Dow Jones
            ".IXIC"; // Nasdaq
            ".INX"   // S&P 500
        ]