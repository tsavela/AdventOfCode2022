open System
open System.Collections.Generic
open System.IO

let createStack (row:string) =
    let stack = Stack<char>()
    Seq.toArray row |> Array.iter (fun x -> stack.Push x)
    stack
    
type Move = {Count : int; From : int; To : int}

let parseMoveRow (row:string) =
    let components = row.Split ' '
    {Count = Int32.Parse components[1]; From = Int32.Parse components[3]; To = Int32.Parse components[5]}

let performMove (move:Move) (stacks:Stack<char>[]) =
    let stackFrom = stacks[move.From - 1]
    let stackTo = stacks[move.To - 1]
    for _ in seq {1..move.Count} do stackTo.Push(stackFrom.Pop())
    
let performMove2 (move:Move) (stacks:Stack<char>[]) =
    let stackFrom = stacks[move.From - 1]
    let stackTo = stacks[move.To - 1]
    seq {1..move.Count} |> Seq.map (fun x -> stackFrom.Pop()) |> Seq.rev |> Seq.iter (fun x -> stackTo.Push(x))

let moves = File.ReadLines "input-moves.txt" |> Seq.map parseMoveRow

let stacks1 = File.ReadLines "input-initial.txt" |> Seq.map createStack |> Seq.toArray
for move in moves do
    performMove move stacks1
    
let stacks2 = File.ReadLines "input-initial.txt" |> Seq.map createStack |> Seq.toArray
for move in moves do
    performMove2 move stacks2
    
let getResult (stacks:Stack<char>[]) =
    stacks |> Array.map (fun x -> x.Peek()) |> String.Concat

printfn $"First puzzle: %s{getResult stacks1}"
printfn $"Second puzzle: %s{getResult stacks2}"
exit 0