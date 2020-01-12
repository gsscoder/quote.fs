namespace QuoteFs

module Finance = 

    open FSharp.Data
    open Thoth.Json.Net

    let inline private apiPath () =
        let baseUrl = "https://financialmodelingprep.com"
        sprintf "%s/api/v3" baseUrl

    let majorIndexes =
        [
            ".DJI";  // Dow Jones
            ".IXIC"; // Nasdaq
            ".INX"   // S&P 500
        ]

    let majorCrypto =
        [
            "BTC"; // Bitcoin
            "ETH"; // Ethereum
            "XRP"; // Ripple
            "LTC"; // Litecoin
            "BHC"; // Bitcoin Cash
        ]

    let getIndexAsync ticker =
        let url = sprintf "%s/majors-indexes/%s" (apiPath ()) ticker
        async {
            let! response = Http.AsyncRequestString url
            return Decode.fromString Index.Decoder response
        } |> Async.StartAsTask

    let getIndex ticker =
        getIndexAsync(ticker)
        |> Async.AwaitTask
        |> Async.RunSynchronously

    let getStockQuoteAsync symbol =
        let url = sprintf "%s/company/profile/%s" (apiPath ()) symbol
        async {
            let! response = Http.AsyncRequestString url
            return Decode.fromString StockQuote.Decoder response
        } |> Async.StartAsTask

    let getStockQuote symbol =
            getStockQuoteAsync(symbol)
            |> Async.AwaitTask
            |> Async.RunSynchronously

    let getPriceAsync symbol =
        let url = sprintf "%s/stock/real-time-price/%s" (apiPath ()) symbol
        async {
            let! response = Http.AsyncRequestString url
            return Decode.fromString (Decode.field "price" Decode.float) response
        } |> Async.StartAsTask
 
    let getPrice symbol =
        getPriceAsync symbol
        |> Async.AwaitTask
        |> Async.RunSynchronously

    let getCryptoAsync ticker =
        let url = sprintf "%s/cryptocurrency/%s" (apiPath ()) ticker
        async {
            let! response = Http.AsyncRequestString url
            return Decode.fromString (Decode.field "price" Decode.float) response
        } |> Async.StartAsTask

    let getCrypto ticker =
        getCryptoAsync ticker
        |> Async.AwaitTask
        |> Async.RunSynchronously