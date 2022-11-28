public readonly struct KatakanaData
{
    public readonly string Katakana;
    public readonly string Translation;

    public KatakanaData(string katakana, string translation)
    {
        Katakana = katakana;
        Translation = translation;
    }

    public override string ToString()
    {
        return $"{Katakana}::{Translation}";
    }
}