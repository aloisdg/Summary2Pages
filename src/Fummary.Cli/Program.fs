open Fummary
open System
open System.IO

let write (item : seq<string>) =
    let fileInfo = new FileInfo(item |> Seq.last)
    if (not fileInfo.Exists)
    || fileInfo.Length = (int64 0) then
        File.AppendAllText(item |> Seq.last, "# " + Path.GetFileNameWithoutExtension(item |> Seq.head))

// ty @DaveHogan https://github.com/fsharp/FAKE/issues/996
let create (path : string) =
    let directoryName = Path.GetDirectoryName(path)
    if not (String.IsNullOrEmpty(directoryName)) then
        Directory.CreateDirectory(directoryName) |> ignore

[<EntryPoint>]
let main argv =
    let lines = File.ReadAllLines("SUMMARY.md") |> Seq.ofArray
    let summary = Fummary.Common.parse lines

    for item in summary do
        printfn "%A" item
        create (item |> Seq.last)
        write item
    0 // return an integer exit code