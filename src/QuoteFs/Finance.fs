namespace QuoteFs

module Finance = 

    open FSharp.Data
    open Thoth.Json.Net

    let [<Literal>] baseUrl = "https://financialmodelingprep.com"

    let majorIndexes =
        [
            ".DJI";  // Dow Jones
            ".IXIC"; // Nasdaq
            ".INX"   // S&P 500
        ]

    let getIndexAsync ticker =
        let url = sprintf "%s/api/v3/majors-indexes/%s" baseUrl ticker
        async {
            let! response = Http.AsyncRequestString url
            return Decode.fromString Index.Decoder response
        } |> Async.StartAsTask

    let getIndex ticker =
        getIndexAsync(ticker)
        |> Async.AwaitTask
        |> Async.RunSynchronously

    let getStockQuoteAsync symbol =
        let url = sprintf "%s/api/v3/company/profile/%s" baseUrl symbol
        async {
            let! response = Http.AsyncRequestString url
            return Decode.fromString StockQuote.Decoder response
        } |> Async.StartAsTask

    let getStockQuote symbol =
            getStockQuoteAsync(symbol)
            |> Async.AwaitTask
            |> Async.RunSynchronously