open System
open System.IO

// use markdig?

let write item = File.AppendAllText(item |> Seq.skip 1 |> Seq.head, "# " + Path.GetFileNameWithoutExtension(item |> Seq.head))

// let createDirectory path : string = Directory.CreateDirectory(Path.GetDirectoryName(path)).FullName
// let createPath path : string = Path.Combine(createDirectory path, Path.GetFileName(path))

// ty @DaveHogan https://github.com/fsharp/FAKE/issues/996
let create path = //Directory.CreateDirectory(Path.GetDirectoryName(path))
    let directoryName = Path.GetDirectoryName(path)
    if not (System.String.IsNullOrEmpty(directoryName)) then
        Directory.CreateDirectory(directoryName) |> ignore

let isUseful line : bool = System.String.IsNullOrWhiteSpace(string line) = false && (string line).StartsWith("* [") = true && (string line).Contains("](") = true && (string line).EndsWith(")") = true

let trimStart line : string = (string line).Replace("* [", "")

// (input.[..(input.Length - 2)]) better?
let trimEnd chars : string = (string chars).Remove((string chars).Length - 1, 1)

let split line : string[] = (string line).Split([|"]("|], StringSplitOptions.RemoveEmptyEntries)

[<EntryPoint>]
let main argv =
    let x =
        File.ReadAllLines("SUMMARY.md")
        |> Seq.map (fun x -> (string x).Trim())
        |> Seq.filter isUseful
        |> Seq.map trimStart
        |> Seq.map trimEnd
        |> Seq.map split
    for item in x do
        printfn "%A" item
        create (item |> Seq.skip 1 |> Seq.head)
        write item
    // x |> Seq.map createPath |> Seq.iter write
    0 // return an integer exit code