open Xunit
open FsUnit.Xunit
open Fummary

let inline add x y = x + y

[<Fact>]
let ``When 2 is added to 2 expect 4``() =
    add 2 2 |> should equal 4

[<Fact>]
let ``When 2.0 is added to 2.0 expect 4.01``() =
    add 2.0 2.0 |> should (equalWithin 0.1) 4.01

[<Fact>]
let ``When ToLower(), expect lowercase letters``() =
    "FSHARP".ToLower() |> should startWith "fs"