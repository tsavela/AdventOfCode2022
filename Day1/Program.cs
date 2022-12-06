var input = File.ReadLines("input.txt");

var sumPerElf = SumPerElf(input);

var sumTopElf = sumPerElf.Max();

Console.WriteLine($"Calories for top elf: {sumTopElf}");

var sumTopThree = sumPerElf.OrderDescending().Take(3).Sum();
Console.WriteLine($"Sum of top three: {sumTopThree}");

IEnumerable<int> SumPerElf(IEnumerable<string> list)
{
    var sum = 0;
    foreach (var line in list)
    {
        if (line == "")
        {
            yield return sum;
            sum = 0;
        }
        else
        {
            sum += int.Parse(line);
        }
    }
    yield return sum;
} 