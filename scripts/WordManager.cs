

using Godot;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;


public record WordInfo(string Word, string[] Types, string[] Definitions)
{
    public string Word { get; init; } = Word.ToLower();
    public string[] Types { get; init; } = [.. Types.Select(type => type.ToLower())];

    public override string ToString()
    {
        var builder = new StringBuilder($"{nameof(WordInfo)} {{ {nameof(Word)} = {Word}, ");

        builder.Append($"{nameof(Types)} = {{ {string.Join(", ", Types)} }}, ");

        builder.Append($"{nameof(Definitions)} = {{ {string.Join(", ", Definitions)} }} ");

        builder.Append('}');
        return builder.ToString();
    }
}

public partial class WordManager : Node
{
    static private HashSet<string> words;
    static public HashSet<string> Words => words;

    static private Dictionary<string, string[]> types;
    static public Dictionary<string, string[]> Types => types;

    static private Dictionary<string, string[]> definitions;
    static public Dictionary<string, string[]> Definitions => definitions;

    static private readonly JsonSerializerOptions jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        AllowTrailingCommas = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
    };

    public static string TitleCaseWord(string word) => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(word);
    public override void _Ready()
    {
        using var wordsFile = FileAccess.Open("res://words/words.json", FileAccess.ModeFlags.Read);

        var wordData = JsonSerializer.Deserialize<WordInfo[]>(wordsFile.GetAsText(), options: jsonOptions);

        words = [.. wordData.Select(word => word.Word)];

        types = wordData
            .ToDictionary(
                word => word.Word,
                word => word.Types
            );
        definitions = wordData
            .ToDictionary(
                word => word.Word,
                word => word.Definitions
            );


        if (OS.IsDebugBuild())
        {
            foreach (var word in wordData)
            {
                GD.Print(word);
            }
        }
    }
}

