namespace QuoteFs

open QuoteFs.Finance

[<AbstractClass;Sealed>]
type Query =
    static member GetStockQuoteAsync symbol = getStockQuoteAsync symbol
    static member GetStockQuote symbol = getStockQuote symbol
    static member GetIndexAsync ticker = getIndexAsync ticker
    static member GetIndex ticker = getIndex ticker
    static member MajorIndexes : seq<string> = majorIndexes |> Seq.map id