open Xunit
open FsUnit.Xunit
open Fummary

[<Fact>]
let ``When ask for empty expect empty``() =
    let empty = System.String.Empty
    Seq.length (Fummary.Common.parse [empty]) |> should equal 0

[<Fact>]
let ``When ask for useless expect empty``() =
    let useless = "# Hello"
    Seq.length (Fummary.Common.parse [useless]) |> should equal 0

[<Fact>]
let ``When ask for file expect content and path``() =
    let file = "* [Introduction](Introduction.md)"
    let content = "Introduction"
    let path = "Introduction.md"
    Seq.length (Fummary.Common.parse [file]) |> should equal 1
    (Fummary.Common.parse [file] |> Seq.head |> Seq.head) |> should equal content
    (Fummary.Common.parse [file] |> Seq.head |> Seq.last) |> should equal path

[<Fact>]
let ``When ask for file in folder expect content and path``() =
    let file = "* [Creating explorers](create_explorers/create_explorers.md)"
    let content = "Creating explorers"
    let path = "create_explorers/create_explorers.md"
    Seq.length (Fummary.Common.parse [file]) |> should equal 1
    (Fummary.Common.parse [file] |> Seq.head |> Seq.head) |> should equal content
    (Fummary.Common.parse [file] |> Seq.head |> Seq.last) |> should equal path

[<Fact>]
let ``When ask for file in folder and file expect sequence of content and path``() =
    let file = "* [Introduction](Introduction.md)"
    let folder = "* [Creating explorers](create_explorers/create_explorers.md)"
    let fileContent = "Introduction"
    let folderContent = "Creating explorers"
    let filePath = "Introduction.md"
    let folderPath = "create_explorers/create_explorers.md"
    Seq.length (Fummary.Common.parse [file; folder]) |> should equal 2
    (Fummary.Common.parse [file] |> Seq.head |> Seq.head) |> should equal fileContent
    (Fummary.Common.parse [file] |> Seq.head |> Seq.last) |> should equal filePath
    (Fummary.Common.parse [folder] |> Seq.last |> Seq.head) |> should equal folderContent
    (Fummary.Common.parse [folder] |> Seq.last |> Seq.last) |> should equal folderPath

[<Fact>]
let ``When ask for bad format line expect FormatException``() =
    let file = "* [Introd](uction](Introduction.md)"
    (Fummary.Common.parse [file]) |> should throw typeof<System.FormatException>

// [<Fact>]
// let ``When 2.0 is added to 2.0 expect 4.01``() =
//     add 2.0 2.0 |> should (equalWithin 0.1) 4.01

// [<Fact>]
// let ``When ToLower(), expect lowercase letters``() =
//     "FSHARP".ToLower() |> should startWith "fs"