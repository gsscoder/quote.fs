namespace QuoteFs

module internal FinancialModelingPrep =

    open System.Text.RegularExpressions

    let [<Literal>] baseUrl = "https://financialmodelingprep.com"

    type quoteResponse = {
            symbol  : string
            profile : profile
        } 
        and profile = {
            beta              : string
            ceo               : string
            changes           : float
            changesPercentage : string
            companyName       : string
            description       : string
            exchange          : string
            image             : string
            industry          : string
            lastDiv           : string
            mktCap            : string
            price             : float
            range             : string
            sector            : string
            volAvg            : string
            website           : string
        }

    type indexResponse = {
        changes   : float
        indexName : string
        price     : float
        ticker    : string
    }

    let changeRegex = Regex(@"(?<=^\(\+|-).*(?=%)", RegexOptions.Compiled)

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

    let toStockQuote (response) = {
        Symbol             = response.symbol
        Volatility         = response.profile.beta |> float
        Executive          = response.profile.ceo
        Change             = response.profile.changes
        ChangesPercent     = response.profile.changesPercentage |> changePercent
        CompanyName        = response.profile.companyName
        CompanyDescription = response.profile.description
        Index              = response.profile.exchange
        ImageUrl           = response.profile.image
        Industry           = response.profile.industry
        LastDividend       = response.profile.lastDiv |> float
        Capitalization     = response.profile.mktCap |> float |> int64
        Price              = response.profile.price |> float
        DayMin             = response.profile.range.Split('-').[0] |> float
        DayMax             = response.profile.range.Split('-').[1] |> float
        Sector             = response.profile.sector
        AverageVolume      = response.profile.volAvg |> int32
        WebSite            = response.profile.website
        }

    let toIndex (response) = {
        Ticker = response.ticker
        Name = response.indexName
        Price = response.price
        Change = response.changes
    }