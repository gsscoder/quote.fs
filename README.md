# QuoteFs

F# library for querying real time stock market data including indexes such as NASDAQ and the S&P500. This is a replacement for [YFinance.fs](https://github.com/gsscoder/yfinance.fs), since Yahoo service on which it's based is now shutdown. It gathers data from [Financial Modeling Prep](https://financialmodelingprep.com/).

## Build and run the CLI app

**NOTE**: .NET Core 3.0 or higher is required.
```sh
# clone the repository
$ git clone https://github.com/gsscoder/quote.fs.git

# build the CLI app
$ paket install && dotnet restore
$ cd quote.fs/src/QuoteFs
$ dotnet build -c release

# execute the CLI app
./artifacts/QuoteFs.CLI/Release/netcoreapp3.0/quote --help
quote 0.1.0-pre
Copyright © Giacomo Stelluti Scala, 2019

  -s, --symbol    Prints detail of stock quote (identified by its symbol)

  --help          Display this help screen.

  --version       Display version information.
```

## C# Sample

`Query` static class is provided to supply a more convenient **C#** interface. You can use [CSharpx](https://github.com/gsscoder/csharpx) to consume in a more accessible and functional way `FSharpResult<T, TError>` type from **C#**.

```csharp
using CSharpx.FSharp;
using QuoteFs;

foreach (var ticker in Query.MajorIndexes) {
    var result = Query.GetIndex(ticker);
    result.March(
        index => Console.WriteLine($"{index.Name} {index.Price}"),
        error => Console.WriteLine($"Trouble: {error}"));
}
```

## F# Interactive

```fsharp
> #r @"...\quote.fs\packages\FSharp.Data\lib\netstandard2.0\FSharp.Data.dll";;
> #r @"...\quote.fs\packages\Thoth.Json.Net\lib\netstandard2.0\Thoth.Json.Net.dll";;
> #r @"...\quote.fs\artifacts\QuoteFs\Debug\netstandard2.0\QuoteFs.dll";;
...
> open QuoteFs.Finance;;
> getStockQuote "MSFT";;
[<Struct>]
val it : Result<QuoteFs.StockQuote,string> =
  Ok
    { Symbol = "MSFT"
      Volatility = 1.216475
      Executive = "Satya Nadella"
      Change = -0.63
      ChangesPercent = -0.39
      CompanyName = "Microsoft Corporation"
      CompanyDescription =
                          "Microsoft Corp is a technology company. It develop, license, and support a wide range of software products and services. Its business is organized into three segments: Productivity and Business Processes, Intelligent Cloud, and More Personal Computing."
      Index = "Nasdaq Global Select"
      ImageUrl = "https://financialmodelingprep.com/images-New-jpg/MSFT.jpg"
      Industry = "Application Software"
      LastDividend = 1.84
      Capitalization = 1217973655000L
      Price = 158.74
      DayMin = 90.28
      DayMax = 120.98
      Sector = "Technology"
      AverageVolume = 31780446
      WebSite = "http://www.microsoft.com" }
> getIndex ".DJI";;
[<Struct>]
val it : Result<QuoteFs.Index,string> = Ok { Ticker = ".DJI"
                                             Name = "Dow Jones"
                                             Change = 81.5508
                                             Price = 28634.9 }
```

## Libraries

- [FSharp.Data](https://github.com/fsharp/FSharp.Data)
- [Thoth.Json.Net](https://github.com/thoth-org/Thoth.Json)
- [CommandLineParser](https://github.com/commandlineparser/commandline)

## Tools

- [Paket](https://github.com/fsprojects/Paket)

### Notes
- This is a pre-release.