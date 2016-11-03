namespace Fummary

module Common =
    open System
    open System.IO

    let delimiter = "]("
    let head = "* ["
    let tail = ")"

    let write (item : seq<string>) =
        File.AppendAllText(item |> Seq.last, "# " + Path.GetFileNameWithoutExtension(item |> Seq.head))

    // ty @DaveHogan https://github.com/fsharp/FAKE/issues/996
    let create (path : string) =
        let directoryName = Path.GetDirectoryName(path)
        if not (String.IsNullOrEmpty(directoryName)) then
         Directory.CreateDirectory(directoryName) |> ignore

    let isUseful (line : string) : bool =
        not (String.IsNullOrWhiteSpace(line))
        && line.StartsWith(head)
        && line.Contains(delimiter)
        && line.EndsWith(tail)

    // (input.[..(input.Length - 2)]) better?
    let trim (line : string) : string =
        line.Substring(head.Length).Remove(line.Length - (tail.Length + head.Length), tail.Length)

    let split (line : string) : string[] =
        line.Split([|delimiter|], StringSplitOptions.RemoveEmptyEntries)

    let run =
        let x =
            File.ReadAllLines("SUMMARY.md")
            |> Seq.map (fun x -> (string x).Trim())
            |> Seq.filter isUseful
            |> Seq.map trim
            |> Seq.map split
        for item in x do
            printfn "%A" item
            create (item |> Seq.last)
            write item