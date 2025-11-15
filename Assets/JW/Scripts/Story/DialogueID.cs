using System.Collections.Generic;

public static class DialogueID
{
    private static readonly Dictionary<string, int> map = new Dictionary<string, int>()
    {
        { "Intro", 0 },
        { "First", 1 },
        { "Second", 2 },
        { "Third", 3 },
        { "Boss", 4 },
    };

    public static int GetID(string name)
    {
        return map[name];
    }
}