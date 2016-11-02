open System
open System.IO

let write item =
    File.AppendAllText(item |> Seq.skip 1 |> Seq.head, "# " + Path.GetFileNameWithoutExtension(item |> Seq.head))

// ty @DaveHogan https://github.com/fsharp/FAKE/issues/996
let create path =
    let directoryName = Path.GetDirectoryName(path)
    if not (System.String.IsNullOrEmpty(directoryName)) then
        Directory.CreateDirectory(directoryName) |> ignore

let isUseful line : bool =
    System.String.IsNullOrWhiteSpace(string line) = false
    && (string line).StartsWith("* [") = true
    && (string line).Contains("](") = true
    && (string line).EndsWith(")") = true

// (input.[..(input.Length - 2)]) better?
let trim line : string =
    (string line).Substring(3).Remove((string line).Length - (1 + 3), 1)

let split line : string[] =
    (string line).Split([|"]("|], StringSplitOptions.RemoveEmptyEntries)

[<EntryPoint>]
let main argv =
    let x =
        File.ReadAllLines("SUMMARY.md")
        |> Seq.map (fun x -> (string x).Trim())
        |> Seq.filter isUseful
        |> Seq.map trim
        |> Seq.map split
    for item in x do
        printfn "%A" item
        create (item |> Seq.skip 1 |> Seq.head)
        write item
    0 // return an integer exit code