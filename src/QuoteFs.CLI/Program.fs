module QuoteFs.CLI.Program

open QuoteFs
open QuoteFs.Finance
open QuoteFs.CLI.Console
open CommandLine

let indent label data = item label data 4

let printQuote(quote : StockQuote) =
    item "Quote" (sprintf "%s %s" (quote.Symbol.ToUpper()) (shorten quote.CompanyDescription 50)) 0
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

let printItem header (name : string) desc value =
    item header (name.ToUpper ()) 0
    indent desc value

let printPrice symbol price =
    printItem "Quote" symbol "Real Time Price" price

let printCrypto ticker price =
    printItem "Cryptocurrency" ticker "Price" price

let printIndex(index : Index) =
    item "Index" (sprintf "%s %s" index.Ticker index.Name) 0
    indent "Price" index.Price
    indent "Change" index.Change
    true

type infoType =
    | Indexes
    | Symbol
    | RealTimePrice
    | Crypto

type options = {
    [<Option('s', "symbol", HelpText = "Prints the detail of a stock quote", SetName = "symbol")>]
    symbol : string
    [<Option('r', "realtime-price", HelpText = "Prints the real time price a stock quote", SetName = "realtime-price")>]
    realTime : string
    [<Option('c', "crypto", HelpText = "Prints the price a cryptocurrency", SetName = "crypto")>]
    crypto : string
} with member this.getInfoType = 
                    if isNull this.symbol |> not
                    then Symbol
                    elif isNull this.realTime |> not
                    then RealTimePrice
                    elif isNull this.crypto |> not
                    then Crypto
                    else Indexes // no quote given, means makor indexes

[<Literal>]
let exitOK = 0
[<Literal>]
let exitFail = 1

let subCommand input fc fp =
    match fc input with
    | Ok data ->
        fp data
        exitOK
    | _ ->
        exitFail

[<EntryPoint>]
let main argv =
    let result = CommandLine.Parser.Default.ParseArguments argv
    match result  with
        | :? Parsed<options> as parsed -> 
                match parsed.Value.getInfoType with
                    | Symbol ->
                        subCommand parsed.Value.symbol getStockQuote printQuote
                    | RealTimePrice ->
                        subCommand parsed.Value.realTime getPrice (printPrice parsed.Value.realTime)
                    | Crypto ->
                        match parsed.Value.crypto.ToLower() with
                        | "major" ->
                            let results = majorCrypto |> Seq.map (fun crypto ->
                                let result = getCrypto crypto
                                match result with
                                | Ok price ->
                                    printCrypto crypto price
                                    true
                                | _ -> error (sprintf "Can not gather details for cryptocurrency %s" crypto)
                                )
                            if results |> Seq.contains false then exitFail
                            else exitOK
                        | _ -> subCommand parsed.Value.crypto getCrypto (printCrypto parsed.Value.crypto)
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