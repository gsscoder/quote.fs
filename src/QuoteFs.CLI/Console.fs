// based on https://gist.github.com/devhawk/4719d1b369170b206cd88b9da16e1b8a
module QuoteFs.CLI.Console

open System
open System.Globalization

let programName = AppDomain.CurrentDomain.FriendlyName

let consoleColor (fc : ConsoleColor) = 
    let current = Console.ForegroundColor
    Console.ForegroundColor <- fc
    { new IDisposable with
          member x.Dispose() = Console.ForegroundColor <- current }

let cprintf color str = Printf.kprintf (fun s -> use c = consoleColor color in printf "%s" s) str
let cprintfn color str = Printf.kprintf (fun s -> use c = consoleColor color in printfn "%s" s) str

let error message =
    cprintfn ConsoleColor.Red "%s: %s" programName message
    false

let br n =
    [0..n - 1] |> Seq.iter (fun _ -> printf "\n")

let default' = CultureInfo.GetCultureInfo "en-US"
let truncated =
    let truncated' = CultureInfo "en-US"
    truncated'.NumberFormat.CurrencyDecimalDigits <- 0
    truncated'

let currency (c : CultureInfo) (value : 'T when 'T :> IFormattable) =
    value.ToString("C", c)

let item label (data : obj) indent =
    [0..indent-1] |> Seq.iter (fun _ -> printf " ")
    cprintf ConsoleColor.Yellow "%s:" label
    match data with
        | :? string -> cprintf ConsoleColor.Green " %s" (data :?> string)
        | :? float -> cprintf ConsoleColor.Cyan " %s" ((data :?> float) |> currency default')
        | :? int32 ->  cprintf ConsoleColor.Cyan "%s" ((data :?> int32) |> currency truncated)
        | _ ->  cprintf ConsoleColor.Cyan " %s" ((data :?> int64) |> currency truncated)
    br 1

let shorten (str : string) n =
    let str' = str.Trim()
    let len = str'.Length
    match len with
    | _ when len > n - 3 -> sprintf "%s..." (str'.Substring(0, n - 3))
    | _ -> str