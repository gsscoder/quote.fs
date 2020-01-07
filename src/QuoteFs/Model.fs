#nowarn "58"
namespace QuoteFs

open Thoth.Json.Net
open QuoteFs.Parser

type StockQuote = {
    Symbol             : string
    Volatility         : float
    Executive          : string
    Change             : float
    ChangesPercent     : float
    CompanyName        : string
    CompanyDescription : string
    Index              : string
    ImageUrl           : string
    Industry           : string
    LastDividend       : float
    Capitalization     : int64
    Price              : float
    DayMin             : float
    DayMax             : float
    Sector             : string
    AverageVolume      : int32
    WebSite            : string
} with static member Decoder : Decoder<StockQuote> =
    Decode.object
        (fun get -> {
            Symbol             = get.Required.Field "symbol" Decode.string
            Volatility         = get.Required.At ["profile"; "beta"] Decode.string |> float
            Executive          = get.Required.At ["profile"; "ceo"] Decode.string
            Change             = get.Required.At ["profile"; "changes"] Decode.float
            ChangesPercent     = get.Required.At ["profile"; "changesPercentage"] Decode.string |> changePercent
            CompanyName        = get.Required.At ["profile"; "companyName"] Decode.string
            CompanyDescription = get.Required.At ["profile"; "description"] Decode.string
            Index              = get.Required.At ["profile"; "exchange"] Decode.string
            ImageUrl           = get.Required.At ["profile"; "image"] Decode.string
            Industry           = get.Required.At ["profile"; "industry"] Decode.string
            LastDividend       = get.Required.At ["profile"; "lastDiv"] Decode.string |> float
            Capitalization     = get.Required.At ["profile"; "mktCap"] Decode.string |> float |> int64
            Price              = get.Required.At ["profile"; "price"] Decode.float
            DayMin             = (get.Required.At ["profile"; "range"] Decode.string).Split('-').[0] |> float
            DayMax             = (get.Required.At ["profile"; "range"] Decode.string).Split('-').[1] |> float
            Sector             = get.Required.At ["profile";  "sector"] Decode.string
            AverageVolume      = get.Required.At ["profile"; "volAvg"] Decode.string |> int32
            WebSite            = get.Required.At ["profile"; "website"] Decode.string
            })

type Index = {
    Ticker : string
    Name   : string
    Change : float
    Price  : float
} with static member Decoder : Decoder<Index> =
    Decode.object (fun get -> {
                    Ticker = get.Required.Field "ticker" Decode.string
                    Name = get.Required.Field "indexName" Decode.string
                    Price = get.Required.Field "price" Decode.float
                    Change = get.Required.Field "changes" Decode.float
                    })
