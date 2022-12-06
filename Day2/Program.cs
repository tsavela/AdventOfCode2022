var input = File.ReadLines("input.txt");

var part1Score = input
    .Select(GetPreplannedMove)
    .Select(Score)
    .Sum();

Console.WriteLine($"Part 1 score: {part1Score}");

var part2Score = input
    .Select(GetReactiveMove)
    .Select(Score)
    .Sum();

Console.WriteLine($"Part 2 score: {part2Score}");

Move GetReactiveMove(string x)
{
    var codes = x.Split(" ");
    var opponentShape = GetOpponentShape(codes[0]);
    var expectedResult = GetWantedResult(codes[1]);
    var myShape = GetShapeForResult(opponentShape, expectedResult);
    return new Move(opponentShape, myShape);
}

Shape GetShapeForResult(Shape opponentsShape, GameResult expectedResult)
{
    if (expectedResult == GameResult.Draw) return opponentsShape;
    return (expectedResult, opponentsShape) switch {
        (GameResult.Win, Shape.Rock) => Shape.Papers,
        (GameResult.Win, Shape.Papers) => Shape.Scissors,
        (GameResult.Win, Shape.Scissors) => Shape.Rock,
        (GameResult.Loss, Shape.Rock) => Shape.Scissors,
        (GameResult.Loss, Shape.Papers) => Shape.Rock,
        (GameResult.Loss, Shape.Scissors) => Shape.Papers,
        _ => throw new ArgumentOutOfRangeException()
    };
}

GameResult GetWantedResult(string x) =>
    x switch
    {
        "X" => GameResult.Loss,
        "Y" => GameResult.Draw,
        "Z" => GameResult.Win,
        _ => throw new ArgumentOutOfRangeException()
    };

Move GetPreplannedMove(string x)
{
    var shapeCodes = x.Split(" ");
    return new Move(GetOpponentShape(shapeCodes[0]), GetMyShape(shapeCodes[1]));
}

Shape GetOpponentShape(string shapeCode) =>
    shapeCode switch
    {
        "A" => Shape.Rock,
        "B" => Shape.Papers,
        "C" => Shape.Scissors,
        _ => throw new ArgumentOutOfRangeException()
    };

Shape GetMyShape(string shapeCode) =>
    shapeCode switch
    {
        "X" => Shape.Rock,
        "Y" => Shape.Papers,
        "Z" => Shape.Scissors,
        _ => throw new ArgumentOutOfRangeException()
    };

int Score(Move move)
{
    return ShapeScore(move.Me) + WinningScore(CalculateGameResult(move));

    int ShapeScore(Shape myShape) =>
        myShape switch
        {
            Shape.Rock => 1,
            Shape.Papers => 2,
            Shape.Scissors => 3,
            _ => throw new ArgumentOutOfRangeException()
        };

    int WinningScore(GameResult result) =>
        result switch
        {
            GameResult.Win => 6,
            GameResult.Draw => 3,
            GameResult.Loss => 0,
            _ => throw new ArgumentOutOfRangeException()
        };

    GameResult CalculateGameResult(Move move)
    {
        if (move.Me == move.Opponent) return GameResult.Draw;
        if (move is { Me: Shape.Rock, Opponent: Shape.Scissors }) return GameResult.Win;
        if (move is { Me: Shape.Scissors, Opponent: Shape.Papers }) return GameResult.Win;
        if (move is { Me: Shape.Papers, Opponent: Shape.Rock }) return GameResult.Win;
        return GameResult.Loss;
    }
}

enum Shape
{
    Rock,
    Papers,
    Scissors
}

record Move(Shape Opponent, Shape Me);

enum GameResult
{
    Win,
    Draw,
    Loss
}