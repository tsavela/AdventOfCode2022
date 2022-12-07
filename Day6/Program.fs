namespace Day6

open System.IO

module Day6 =
    
    let rec FindMarkerIndex markerLength (characters:char[]) position =
        if Array.distinct characters[..markerLength - 1] |> Array.length = markerLength then
            position + markerLength
        else
            FindMarkerIndex markerLength (Array.tail characters) (position + 1)
    
    let input = File.ReadAllText("input.txt").ToCharArray()
    let markerIndex length = FindMarkerIndex length input 0
    
    let firstPuzzle = markerIndex 4
    let secondPuzzle = markerIndex 14    
    
    [<EntryPoint>]
    let main args =
        printfn $"First puzzle answer:  %d{firstPuzzle}"
        printfn $"Second puzzle answer: %d{secondPuzzle}"
        0