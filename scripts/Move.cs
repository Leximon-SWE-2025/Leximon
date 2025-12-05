public readonly record struct Move(string Word)
{
    public string Word { get; init; } = Word.ToLowerInvariant().Trim();

    public static implicit operator Move(string word) => new(word);
    public static implicit operator string(Move move) => move.Word;
}

