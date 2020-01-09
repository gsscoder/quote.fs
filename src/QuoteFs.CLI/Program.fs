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

let printIndex(index : Index) =
    item "Index" (sprintf "%s %s" index.Ticker index.Name) 0
    indent "Price" index.Price
    indent "Change" index.Change
    true

type options = {
    [<Option('s', "symbol", HelpText = "Prints detail of a stock quote")>] symbol : string
}

[<Literal>]
let exitOK = 0
[<Literal>]
let exitFail = 1

[<EntryPoint>]
let main argv =
    let result = CommandLine.Parser.Default.ParseArguments argv
    match result  with
        | :? Parsed<options> as parsed -> 
                match parsed.Value.symbol with
                    // no quote give, let's print 5 main indexes
                    | null ->
                        let results = majorIndexes |> Seq.map (fun index ->
                            let result = getIndex index
                            match result with
                            | Ok index -> printIndex index
                            | _ -> error (sprintf "Can not gather details for index %s" index)
                            )
                        if results |> Seq.contains false then exitFail
                        else exitOK
                    | _ ->
                        let result = parsed.Value.symbol |> getStockQuote
                        match result with
                        | Ok quote ->
                            printQuote quote
                            exitOK
                        | _ -> exitFail
        | _ -> exitFail