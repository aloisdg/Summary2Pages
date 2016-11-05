namespace Fummary

module Common =
    open System

    let private delimiter = "]("
    let private head = "* ["
    let private tail = ")"

    let private isUseful (line : string) : bool =
        not (String.IsNullOrWhiteSpace(line))
        && line.StartsWith(head)
        && line.Contains(delimiter)
        && line.EndsWith(tail)

    // (input.[..(input.Length - 2)]) better?
    let private trim (line : string) : string =
        line.Substring(head.Length).Remove(line.Length - (tail.Length + head.Length), tail.Length)

    let private split (line : string) : string[] =
        line.Split([|delimiter|], StringSplitOptions.RemoveEmptyEntries)

    let parse (lines : seq<string>) : seq<string[]> =
        lines
        |> Seq.map (fun x -> (string x).Trim())
        |> Seq.filter isUseful
        |> Seq.map trim
        |> Seq.map split