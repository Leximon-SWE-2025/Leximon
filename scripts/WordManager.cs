

using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;


public class Word
{
    public Word(string word, List<string> types, List<string> definitions) : base()
    {
        this.word = word.ToLower();
        this.types = types.Select(type => type.ToLower()).ToList();
        this.definitions = definitions;
    }

    public string word { get; init; }
    public List<string> types { get; init; }
    public List<string> definitions { get; init; }

    public override string ToString()
    {
        var builder = new StringBuilder($"{nameof(Word)} {{ {nameof(word)} = {word}, ");

        builder.Append($"{nameof(types)} = {{ {string.Join(", ", types)} }}, ");

        builder.Append($"{nameof(definitions)} = {{ {string.Join(", ", definitions)} }} ");

        builder.Append("}");
        return builder.ToString();
    }
}

public partial class WordManager : Node
{
    private List<Word> words;
    public List<Word> Words => words;



    public override void _Ready()
    {
        using (var wordsFile = FileAccess.Open("res://words/words.json", FileAccess.ModeFlags.Read))
        {
            words = JsonSerializer.Deserialize<List<Word>>(wordsFile.GetAsText());
        }

        if (OS.IsDebugBuild())
        {
            foreach (var word in words)
            {
                GD.Print(word);
            }
        }

    }
}

