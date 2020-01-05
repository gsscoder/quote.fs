namespace QuoteFs

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
}

type Index = {
    Ticker : string
    Name   : string
    Change : float
    Price  : float
}