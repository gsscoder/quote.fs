module QuoteFs.CLI.Program

open QuoteFs
open QuoteFs.Finance
open QuoteFs.CLI.Console
open CommandLine

let indent label data = item label data 4

let printQuote(quote : StockQuote) =
    item "Quote" (sprintf "%s %s" quote.Symbol (shorten quote.CompanyDescription 50)) 0
    indent "Industry" quote.Industry
    indent "Price" quote.Price
    indent "Change" quote.Change
    indent "Index" quote.Index
    indent "Last Dividend" quote.LastDividend
    indent "Capitalization" quote.Capitalization
    indent "Average Volume" quote.AverageVolume
    indent "Day Min" quote.DayMin
    indent "Day Max" quote.DayMax
    indent "Web Site" quote.WebSite

let printPrice symbol price =
    item "Quote" symbol 0
    indent "Real Time Price" price

let printIndex(index : Index) =
    item "Index" (sprintf "%s %s" index.Ticker index.Name) 0
    indent "Price" index.Price
    indent "Change" index.Change
    true

type infoType =
    | Indexes
    | Symbol
    | RealTimePrice

type options = {
    [<Option('s', "symbol", HelpText ="Prints detail of a stock quote", SetName="symbol")>]
    symbol : string
    [<Option('r', "realtime-price", HelpText ="Prints real time price a stock quote", SetName="realtime-price")>]
    realTime : string
} with member this.getInfoType = 
                    if isNull this.symbol |> not
                    then Symbol
                    elif isNull this.realTime |> not
                    then RealTimePrice
                    else Indexes // no quote given, means makor indexes



[<Literal>]
let exitOK = 0
[<Literal>]
let exitFail = 1

[<EntryPoint>]
let main argv =
    let result = CommandLine.Parser.Default.ParseArguments argv
    match result  with
        | :? Parsed<options> as parsed -> 
                match parsed.Value.getInfoType with
                    | RealTimePrice ->
                        let symbol = parsed.Value.realTime
                        let result = symbol |> getPrice
                        match result with
                        | Ok price ->
                            printPrice symbol price
                            exitOK
                        | _ -> exitFail
                    | Symbol ->
                        let result = parsed.Value.symbol |> getStockQuote
                        match result with
                        | Ok quote ->
                            printQuote quote
                            exitOK
                        | _ -> exitFail
                    | _ ->
                        let results = majorIndexes |> Seq.map (fun index ->
                            let result = getIndex index
                            match result with
                            | Ok index -> printIndex index
                            | _ -> error (sprintf "Can not gather details for index %s" index)
                            )
                        if results |> Seq.contains false then exitFail
                        else exitOK
        | _ -> exitFail