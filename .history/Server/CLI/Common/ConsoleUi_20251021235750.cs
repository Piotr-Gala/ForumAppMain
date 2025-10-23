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

    public static string ReadRaw(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine() ?? string.Empty;
    }

    // Zwraca null, jeśli user wciśnie Enter (czyli "zachowaj")
    public static string? ReadOptional(string prompt)
    {
        Console.Write(prompt);
        var s = Console.ReadLine();
        if (s == null) return null;
        s = s.Trim();
        return s.Length == 0 ? null : s;
    }

    public static void Pause()
    {
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }
}
