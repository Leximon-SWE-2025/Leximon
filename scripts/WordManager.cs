

using Godot;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;


public enum Relation
{
    None, Synonym, Antonym,
}

public class RelationJsonConverter : JsonConverter<Relation>
{
    public override Relation Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        reader.GetString().ToLowerInvariant() switch // gets the string in a normalized case
        {// we only expect synonyms and antonyms to be written and read, but for robustness, anything else converts to Relation.None
            "synonym" => Relation.Synonym,
            "antonym" => Relation.Antonym,
            _ => Relation.None,
        };

    public override void Write(Utf8JsonWriter writer, Relation value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value.ToString().ToLowerInvariant());

}

public record CategoryInfo(string Type, Relation Relation);

public record WordInfo(string Word, HashSet<CategoryInfo> Types, string[] Definitions)
{
    public string Word { get; init; } = Word.ToLower().Trim();

    public override string ToString() => $"{nameof(WordInfo)} {{ {string.Join(", ",
            $"{nameof(Word)} = {Word}",
            $"{nameof(Types)} = {{ {string.Join(", ", Types)} }}",
            $"{nameof(Definitions)} = {{ {string.Join(", ", Definitions)} }}"
        )} }}";
}

public partial class WordManager : Node
{
    static private Dictionary<string, WordInfo> WordData;

    static private Dictionary<string, string[]> SynonymsList;
    static private Dictionary<string, string[]> AntonymsList;

    static public IEnumerable<string> Words => WordData.Keys;

    public static IEnumerable<string> GetTypes(string word) => WordData[word].Types.Select(t => t.Type);
    public static IEnumerable<string> GetDefinitions(string word) => WordData[word].Definitions;
    public static IEnumerable<string> GetSynonyms(string word) => SynonymsList[word];
    public static IEnumerable<string> GetAntonyms(string word) => AntonymsList[word];

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

        Dictionary<string, List<string>> tempSynonymList = new();
        Dictionary<string, List<string>> tempAntonymList = new();

        foreach (var word in WordData.Values)
        {
            foreach (var type in word.Types)
            {
                tempSynonymList.TryAdd(type.Type, []);
                tempAntonymList.TryAdd(type.Type, []);
                switch (type.Relation)
                {
                    case Relation.None:
                        if (OS.IsDebugBuild())
                        {
                            GD.PrintErr($"Word: {word} had type with no relation");
                        }
                        break;
                    case Relation.Synonym:
                        tempSynonymList[type.Type].Add(word.Word);
                        break;
                    case Relation.Antonym:
                        tempAntonymList[type.Type].Add(word.Word);
                        break;
                }
            }
        }

        var key_converter = (KeyValuePair<string, List<string>> kv) => kv.Key;
        var value_converter = (KeyValuePair<string, List<string>> kv) => kv.Value.ToArray();
        SynonymsList = tempSynonymList.ToDictionary(key_converter, value_converter);
        AntonymsList = tempAntonymList.ToDictionary(key_converter, value_converter);


        if (OS.IsDebugBuild())
        {
            foreach (var word in wordData)
            {
                GD.Print(word);
            }
        }
    }
}

