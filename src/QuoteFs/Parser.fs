namespace QuoteFs

module internal Parser =

    open System.Text.RegularExpressions

    let private changeRegex = Regex(@"(?<=^\(\+|-).*(?=%)", RegexOptions.Compiled)

    // parses a string formatted as "(+0.42%)" to float
    let changePercent (value : string) =
        let sign =
            match value.[1] with
            | '-' -> -1.0
            | _   -> 1.0
        let percent =
            let result = changeRegex.Match(value)
            match result.Success with
            | true -> result.Value |> float
            | _ -> 0.0
        percent * sign