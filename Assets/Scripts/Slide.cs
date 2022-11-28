using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Slide
{
    private readonly KatakanaData[] _characters;

    public Slide(IEnumerable<KatakanaData> word)
    {
        _characters = word.ToArray();
    }

    public string GetKatakana()
    {
        var builder = new StringBuilder();
        foreach (var character in _characters)
        {
            builder.Append(character.Katakana);
        }

        return builder.ToString();
    }
    
    public string GetTranslation()
    {
        var builder = new StringBuilder();
        foreach (var character in _characters)
        {
            builder.Append(character.Translation);
        }

        return builder.ToString();
    }
    
    public bool ContainsKatakana(KatakanaData katakana)
    {
        return _characters.Contains(katakana);
    }
}