namespace QuoteFs

open QuoteFs.Finance

[<AbstractClass;Sealed>]
type Query =
    static member MajorIndexes : seq<string> = majorIndexes |> Seq.map id
    static member GetIndexAsync ticker = getIndexAsync ticker
    static member GetIndex ticker = getIndex ticker
    static member GetStockQuoteAsync symbol = getStockQuoteAsync symbol
    static member GetStockQuote symbol = getStockQuote symbol