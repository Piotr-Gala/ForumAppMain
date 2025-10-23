namespace CLI.Common;

public static class ConsoleUi
{
    public static int ReadInt(string prompt)
    {
        Console.Write(prompt);
        var input = Console.ReadLine();
        if (!int.TryParse(input, out var val))
            throw new RepositoryContracts.ValidationException("ID has to be a number.");
        return val;
    }

    public static string ReadNonEmpty(string prompt)
    {
        Console.Write(prompt);
        var s = Console.ReadLine() ?? "";
        if (string.IsNullOrWhiteSpace(s))
            throw new RepositoryContracts.ValidationException("Field cannot be empty.");
        return s.Trim();
    }

    public static void Pause()
    {
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }
}
