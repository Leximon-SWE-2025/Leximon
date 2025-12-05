

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

public static class RelationExtension
{
    public static double DamageMultiplier(this Relation relation) => relation switch
    {
        Relation.None => 0,
        Relation.Synonym => 0.25,
        Relation.Antonym => 1,
        _ => throw new NotImplementedException(),
    };

    public static double DefenseMultiplier(this Relation relation) => relation switch
    {
        Relation.None => 0,
        Relation.Synonym => 1,
        Relation.Antonym =>0.25,
        _ => throw new NotImplementedException(),
    };
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

public record CategoryInfo(string Type, [property: JsonConverter(typeof(RelationJsonConverter))] Relation Relation)
{
    public override string ToString() => $"{nameof(CategoryInfo)} {{ {string.Join(", ",
            $"{nameof(Type)} = {Type}",
            $"{nameof(Relation)} = {Relation}"
        )} }}";
}

public record WordInfo(string Word, CategoryInfo[] Types, string[] Definitions)
{
    public string Word { get; init; } = Word.ToLower().Trim();
    public string[] Definitions { get; init; } = Definitions ?? [];
    public CategoryInfo[] Types { get; init; } = Types ?? [];

    public override string ToString() => $"{nameof(WordInfo)} {{ {string.Join(", ",
          $"{nameof(Word)} = {Word}",
          $"{nameof(Types)} = {{ {string.Join(", ", [.. Types.Select(t => t.ToString())])} }}",
          $"{nameof(Definitions)} = {{ {string.Join(", ", Definitions)} }}"
      )} }}";
}

public partial class WordManager : Node
{


    static private Dictionary<string, WordInfo> WordData;

    //static private Dictionary<string, string[]> SynonymsList;
    //static private Dictionary<string, string[]> AntonymsList;

    static public IEnumerable<string> Words => WordData.Keys;

    public static IEnumerable<Move> AllMoves => WordData.Keys.Select(word => new Move(word));

    public static IEnumerable<string> AllTypes => WordData.Values.SelectMany(wd => wd.Types.Select(t => t.Type)).Distinct(); // Gets all unique the types

    //public static IEnumerable<string> GetTypes(string word) => WordData[word].Types.Select(t => t.Type);
    public static IEnumerable<string> GetDefinitions(string word) => WordData[word].Definitions;

    private static IEnumerable<string> GetOfRelation(string word, Relation relation) => WordData[word].Types.Where(t => t.Relation == relation).Select(t => t.Type);
    public static IEnumerable<string> GetSynonyms(string word) => GetOfRelation(word, Relation.Synonym);
    public static IEnumerable<string> GetSynonyms(WordInfo word) => GetSynonyms(word);
    public static IEnumerable<string> GetAntonyms(string word) => GetOfRelation(word, Relation.Antonym);
    public static IEnumerable<string> GetAntonyms(WordInfo word) => GetAntonyms(word);

    public static IEnumerable<string> GetTypeAntonyms(string type) => WordData.Values.Where(d => d.Types.Any(t => t.Type == type && t.Relation == Relation.Antonym)).Select(w => w.Word);

    public static IEnumerable<string> GetTypeSynonyms(string type) => WordData.Values.Where(d => d.Types.Any(t => t.Type == type && t.Relation == Relation.Synonym)).Select(w => w.Word);

    public static IEnumerable<string> RandomTypes(int count = 1) => AllTypes.Random(count);
    public static IEnumerable<Move> RandomMoves(int count = 5) => RandomWords(count).Select(w => new Move(w));
    public static IEnumerable<string> RandomWords(int count = 5)
    {
        var wordsCopy = Words.ToArray();
        Random.Shared.Shuffle(wordsCopy);

        return wordsCopy.Take(count);
    }

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

        //Dictionary<string, List<string>> tempSynonymList = new();
        //Dictionary<string, List<string>> tempAntonymList = new();

        //foreach (var word in WordData.Values)
        //{
        //    foreach (var type in word.Types)
        //    {
        //        tempSynonymList.TryAdd(type.Type, []);
        //        tempAntonymList.TryAdd(type.Type, []);
        //        switch (type.Relation)
        //        {
        //            case Relation.None:
        //                if (OS.IsDebugBuild())
        //                {
        //                    GD.PrintErr($"Word: {word} had type with no relation");
        //                }
        //                break;
        //            case Relation.Synonym:
        //                tempSynonymList[type.Type].Add(word.Word);
        //                break;
        //            case Relation.Antonym:
        //                tempAntonymList[type.Type].Add(word.Word);
        //                break;
        //        }
        //    }
        //}

        //var key_converter = (KeyValuePair<string, List<string>> kv) => kv.Key;
        //var value_converter = (KeyValuePair<string, List<string>> kv) => kv.Value.ToArray();
        //SynonymsList = tempSynonymList.ToDictionary(key_converter, value_converter);
        //AntonymsList = tempAntonymList.ToDictionary(key_converter, value_converter);


        if (OS.IsDebugBuild())
        {
            foreach (var word in wordData)
            {
                GD.Print(word);
            }
        }
    }

    public static bool AreSynonyms(string word1, string word2)
    {
        var synonyms1 = GetSynonyms(word1);
        var synonyms2 = GetSynonyms(word2);

        if (synonyms1.Intersect(synonyms2).Any())
        {
            return true;
        }
        return false;
    }
    public static bool AreAntonyms(string word1, string word2)
    {
        var synonyms1 = GetSynonyms(word1);
        var synonyms2 = GetSynonyms(word2);
        var antonyms1 = GetAntonyms(word1);
        var antonyms2 = GetAntonyms(word2);

        if (synonyms1.Intersect(antonyms2).Any() || synonyms2.Intersect(antonyms1).Any())
        {
            return true;
        }
        return false;
    }

    public static Relation ClassifyRelation(string word, string type)
    {
        if (GetSynonyms(word).Contains(type))
        {
            return Relation.Synonym;
        }
        else if (GetAntonyms(word).Contains(type))
        {
            return Relation.Antonym;
        }



        return Relation.None;



        //if (string.IsNullOrWhiteSpace(word1) || string.IsNullOrWhiteSpace(word2))
        //    throw new ArgumentException("Words must not be null or empty.");

        //var w1 = word1.Trim().ToLowerInvariant();
        //var w2 = word2.Trim().ToLowerInvariant();

        //if (w1 == w2)
        //    return Relation.None;
        //if (!WordData.TryGetValue(w1, out _) || !WordData.TryGetValue(w2, out _))
        //    return Relation.None;

        //if (AreSynonyms(w1, w2))
        //    return Relation.Synonym;

        //if (AreAntonyms(w1, w2))
        //    return Relation.Antonym;

        //return Relation.None;
    }
}

