var input = System.IO.File.ReadAllLines("input.txt");

Directory currentDirectory = null!;
var rootDirectory = new Directory { Name = "/" };
var allDirectories = new Dictionary<string, Directory>()
{
    { "/", rootDirectory }
};

// Parse all input
var rowPointer = 0;
while (!IsEndOfInput()) ParseNextCommand();

// Calculate total directory sizes
CalculateDirectorySize(rootDirectory);

// Puzzle 1
var firstPuzzle = allDirectories.Values.Select(d => d.Size).Where(size => size <= 100000).Sum();
Console.WriteLine($"First puzzle: {firstPuzzle}");

// Puzzle 2
var freeSpace = 70000000 - rootDirectory.Size;
var minimumToDelete = 30000000 - freeSpace;
var secondPuzzle = allDirectories.Values.Select(d => d.Size).Where(size => size >= minimumToDelete).Min();
Console.WriteLine($"Second puzzle: {secondPuzzle}");

void CalculateDirectorySize(Directory directory)
{
    var fileSizes = directory.Files.Values.Sum(f => f.Size);
    var childDirectories = directory.Directories.Values;
    foreach (var childDirectory in childDirectories)
    {
        CalculateDirectorySize(childDirectory);
    }

    var childDirectorySizes = childDirectories.Sum(d => d.Size);
    directory.Size = fileSizes + childDirectorySizes;
}

void ParseNextCommand()
{
    var command = ReadNextRow();
    if (command.StartsWith("$ cd"))
    {
        HandleCdCommand(command);
    }
    else if (command == "$ ls")
    {
        HandleLsCommand();
    }
}

void HandleCdCommand(string command)
{
    var destinationDirectoryName = command.Split(' ')[2];
    if (destinationDirectoryName == "/") currentDirectory = rootDirectory;
    else if (destinationDirectoryName == "..") currentDirectory = currentDirectory.Parent!;
    else currentDirectory = currentDirectory.Directories[destinationDirectoryName];
}

void HandleLsCommand()
{
    while (!IsEndOfInput() && !PeekNextRow().StartsWith("$"))
    {
        var data = ReadNextRow().Split(' ');
        var name = data[1];
        if (data[0] == "dir")
        {
            if (!currentDirectory.Directories.TryGetValue(name, out var directory))
            {
                directory = new Directory { Name = name, Parent = currentDirectory };
                currentDirectory.Directories.Add(name, directory);
                AddToAllDirectories(directory);
            }
        }
        else
        {
            var fileSize = int.Parse(data[0]);
            if (!currentDirectory.Files.TryGetValue(name, out var file))
            {
                file = new File { Name = name, Size = fileSize };
                currentDirectory.Files.Add(name, file);
            }
        }
    }
}

string ReadNextRow() => input[rowPointer++];
string PeekNextRow() => input[rowPointer];
bool IsEndOfInput() => rowPointer == input.Length;

void AddToAllDirectories(Directory directory)
{
    var current = directory;
    var pathToTop = new List<Directory>();
    while (current.Parent != null)
    {
        pathToTop.Add(current);
        current = current.Parent;
    }

    var fullyQualifiedName = string.Join("/", pathToTop.Select(d => d.Name));
    allDirectories.Add(fullyQualifiedName, directory);
}

class Directory
{
    public required string Name;
    public Directory? Parent;
    public readonly Dictionary<string, File> Files = new();
    public readonly Dictionary<string, Directory> Directories = new();
    public int? Size;
}

class File
{
    public required string Name;
    public required int Size;
}