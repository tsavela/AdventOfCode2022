open System.IO

let input = File.ReadAllLines("input.txt")

let splitToTuple (separator:char) (str:string) = (str.Split(separator)[0],str.Split(separator)[1]) 

let toRanges rangeString =
    let (startIndexString, endIndexString) = splitToTuple '-' rangeString
    ((startIndexString |> int), (endIndexString|> int))
    
let parseRow item =
    let items1, items2 = splitToTuple ',' item
    ((toRanges items1), (toRanges items2))
    
let parseInput (s:string[]) = s |> Array.map parseRow

let isCompletelyOverlapping (range1, range2) =
    let range1Start, range1End = range1
    let range2Start, range2End = range2
    (range1Start <= range2Start && range1End >= range2End) || (range2Start <= range1Start && range2End >= range1End)

let isPartiallyOverlapping (range1, range2) =
    let range1Start, range1End = range1
    let range2Start, range2End = range2
    range1Start <= range2End && range1End >= range2Start
    
let ranges = parseInput input
let firstPuzzle = ranges |> Seq.filter isCompletelyOverlapping |> Seq.length
let secondPuzzle = ranges |> Seq.filter isPartiallyOverlapping |> Seq.length

printfn $"First puzzle: %d{firstPuzzle}"
printfn $"Second puzzle: %d{secondPuzzle}"

exit 0