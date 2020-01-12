namespace QuoteFs

open QuoteFs.Finance

[<AbstractClass;Sealed>]
type Query =
    static member MajorIndexes : seq<string> = majorIndexes |> Seq.map id
    static member MajorCrypto : seq<string> = majorCrypto |> Seq.map id
    static member GetIndexAsync ticker = getIndexAsync ticker
    static member GetIndex ticker = getIndex ticker
    static member GetStockQuoteAsync symbol = getStockQuoteAsync symbol
    static member GetStockQuote symbol = getStockQuote symbol
    static member GetPriceAsync symbol = getPriceAsync symbol
    static member GetPrice symbol = getPrice symbol
    static member GetCryptoAsync ticker = getCryptoAsync ticker
    static member GetCrypto ticker = getCrypto ticker