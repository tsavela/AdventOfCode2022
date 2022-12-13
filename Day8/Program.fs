let trees = array2D (System.IO.File.ReadAllLines("input.txt")
                     |> Array.map (fun x -> x.ToCharArray()
                                            |> Array.map (fun c -> int (string c))))
    
// Puzzle 1    
let isTreeVisible (trees:int[,]) row column =
    let treeHeight = trees[row,column]
    let isHighestFromLeft = treeHeight > (Seq.max trees[row,0..column-1])
    let isHighestFromRight = treeHeight > (Seq.max trees[row,column+1..])
    let isHighestFromTop = treeHeight > (Seq.max trees[0..row-1,column])
    let isHighestFromBottom= treeHeight > (Seq.max trees[row+1..,column])
    isHighestFromBottom || isHighestFromTop || isHighestFromLeft || isHighestFromRight

let rec calculateTotalVisibleTreesFromOutside trees =
    let forestWidth = Array2D.length1 trees
    let mutable total = 0
    
    total <- (forestWidth - 1) * 4 // Edge trees
    
    for row in seq {1..forestWidth - 2} do
        for column in seq {1..forestWidth - 2} do
            total <- total + if isTreeVisible trees row column then 1 else 0
    total

printfn $"First puzzle: %d{calculateTotalVisibleTreesFromOutside trees}"

// Puzzle 2
let rec getNumberOfVisibleTreesFromATree (treeLine : int array) =
    let rec getNumberOfVisibleTreesFromATreeRec originalTreeHeight localMax (treeLine : int list) =
        match treeLine with
            | [] -> 1
            | closestTree :: tail when closestTree > localMax -> 1 + getNumberOfVisibleTreesFromATreeRec originalTreeHeight closestTree tail
            | closestTree :: tail -> getNumberOfVisibleTreesFromATreeRec originalTreeHeight localMax tail
    
    getNumberOfVisibleTreesFromATreeRec (Array.head treeLine) -1 (Array.toList (Array.tail treeLine)) - 1

let getSightRangeValue (trees:int[,]) row column =
    let sightRangeLeft = getNumberOfVisibleTreesFromATree (Array.rev trees[row,..column])
    let sightRangeRight = getNumberOfVisibleTreesFromATree trees[row,column..]
    let sightRangeUp = getNumberOfVisibleTreesFromATree (Array.rev trees[0..row,column])
    let sightRangeDown = getNumberOfVisibleTreesFromATree trees[row..,column]
    sightRangeLeft * sightRangeRight * sightRangeUp * sightRangeDown

let rec getMaximumSightRangeValue trees =
    let forestWidth = Array2D.length1 trees
    let mutable maxSightValue = 0
    
    for row in seq {1..forestWidth - 2} do
        for column in seq {1..forestWidth - 2} do
            maxSightValue <- System.Math.Max((getSightRangeValue trees row column), maxSightValue)
    maxSightValue
    
printfn $"Second puzzle: %d{getMaximumSightRangeValue trees}" // TODO: Wrong answer
