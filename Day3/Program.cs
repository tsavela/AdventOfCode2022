var input = File.ReadLines("input.txt");

var sumOfIndividualPriorities = input.Select(GetCompartmentItems)
    .Select(GetItemInBothCompartments)
    .Select(GetPriority)
    .Sum();

Console.WriteLine($"Total priority: {sumOfIndividualPriorities}");

var sumOfGroupPriorities = GetGroupRucksacks(input)
    .Select(FindCommonItem)
    .Select(GetPriority)
    .Sum();

Console.WriteLine($"Group priority: {sumOfGroupPriorities}");

IEnumerable<GroupRucksacks> GetGroupRucksacks(IEnumerable<string> items)
{
    return items.Select((x, i) => (Index: i, Items: x.ToCharArray())).GroupBy(key => key.Index / 3, value => value.Items)
        .Select(x => new GroupRucksacks(x.ElementAt(0), x.ElementAt(1), x.ElementAt(2)));
}

char FindCommonItem(GroupRucksacks rucksacks) =>
    rucksacks.Rucksack1.Distinct().Single(item => rucksacks.Rucksack2.Contains(item) && rucksacks.Rucksack3.Contains(item));

int GetPriority(char item) =>
    char.IsLower(item) ? item - 'a' + 1 : item - 'A' + 27;

char GetItemInBothCompartments(Rucksack rucksack) =>
    rucksack.Compartment1Items.Distinct().Single(item => rucksack.Compartment2Items.Contains(item));

Rucksack GetCompartmentItems(string x) =>
    new Rucksack(x.Substring(0, x.Length / 2).ToCharArray(), x.Substring(x.Length / 2).ToCharArray());
    
record Rucksack(char[] Compartment1Items, char[] Compartment2Items);

record GroupRucksacks(char[] Rucksack1, char[] Rucksack2, char[] Rucksack3);