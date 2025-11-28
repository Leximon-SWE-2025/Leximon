

using Godot;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;


public record WordInfo(string Word, string[] Types, string[] Definitions, string[] Synonyms, string[] Antonyms)
{
    public string Word { get; init; } = Word.ToLower();
    public string[] Types { get; init; } = [.. Types.Select(type => type.ToLower())];
    public string[] Synonyms { get; init; } = [.. Synonyms.Select(type => type.ToLower())];
    public string[] Antonyms { get; init; } = [.. Antonyms.Select(type => type.ToLower())];

    public override string ToString() => $"{nameof(WordInfo)} {{ {string.Join(", ",
            $"{nameof(Word)} = {Word}",
            $"{nameof(Types)} = {{ {string.Join(", ", Types)} }}",
            $"{nameof(Definitions)} = {{ {string.Join(", ", Definitions)} }}",
            $"{nameof(Synonyms)} = {{ {string.Join(", ", Synonyms)} }}",
            $"{nameof(Antonyms)} = {{ {string.Join(", ", Antonyms)} }}"
        )} }}";
}

public partial class WordManager : Node
{
    public enum Relation
    {
        Synonym,
        Antonym,
        Neither
    }

    static private Dictionary<string, WordInfo> WordData;

    static public IEnumerable<string> Words => WordData.Keys;

    public static IEnumerable<string> GetTypes(string word) => WordData[word].Types;
    public static IEnumerable<string> GetDefinitions(string word) => WordData[word].Definitions;
    public static IEnumerable<string> GetSynonyms(string word) => WordData[word].Synonyms;
    public static IEnumerable<string> GetAntonyms(string word) => WordData[word].Antonyms;

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

        WordData = wordData.ToDictionary(word => word.Word, word => word);

        //words = [.. wordData.Select(word => word.Word)];

        //types = wordData
        //    .ToDictionary(
        //        word => word.Word,
        //        word => word.Types
        //    );
        //definitions = wordData
        //    .ToDictionary(
        //        word => word.Word,
        //        word => word.Definitions
        //    );


        if (OS.IsDebugBuild())
        {
            foreach (var word in wordData)
            {
                GD.Print(word);
            }
        }
    }

    public static Relation ClassifyRelation(string word1, string word2)
    {
        if (string.IsNullOrWhiteSpace(word1) || string.IsNullOrWhiteSpace(word2))
            throw new ArgumentException("Words must not be null or empty.");

        var w1 = word1.Trim().ToLowerInvariant();
        var w2 = word2.Trim().ToLowerInvariant();

        if (w1 == w2)
            return Relation.Neither;

        if (!WordData.TryGetValue(w1, out var info1) || !WordData.TryGetValue(w2, out var info2))
            return Relation.Neither;

        if (info1.Synonyms.Contains(w2) || info2.Synonyms.Contains(w1))
            return Relation.Synonym;

        if (info1.Antonyms.Contains(w2) || info2.Antonyms.Contains(w1))
            return Relation.Antonym;

        return Relation.Neither;
    }
}

