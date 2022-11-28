using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class KatakanaController : MonoBehaviour
{
    private static KatakanaController _instance;
    public static KatakanaController Instance()
    {
        if (_instance == null)
        {
            _instance = new GameObject("Katakana Controller").AddComponent<KatakanaController>();
        }

        return _instance;
    }
    public HashSet<KatakanaData> Katakana { get; private set; }

    private GameSettings _settings;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        LoadKatakana();
    }

    public IEnumerable<KatakanaData> GetRandomKatakana(int count, Slide[] currentSlides)
    {
        var result = new KatakanaData[count];
        var blocked = new List<KatakanaData>();
        
        for (var i = 0; i < count; i++)
        {
            var possible =
                from katakana in Katakana
                from slide in currentSlides
                where slide != null && !slide.ContainsKatakana(katakana)
                where !blocked.Contains(katakana)
                select katakana;
            
            if (!possible.Any())
            {
                result[i] = Katakana.ElementAt(Random.Range(0, Katakana.Count));
            }
            else
            {
                result[i] = possible.ElementAt(Random.Range(0, possible.Count()));
            }
            blocked.Add(result[i]);
        }
        return result;
    }

    public void ApplySettings(GameSettings settings)
    {
        _settings = settings;
    }

    public GameSettings GetSettings()
    {
        return _settings;
    }

    private void LoadKatakana()
    {
        const string katakanaFile = "ア a イ i ウ u エ e オ o " +
                                    "カ ka キ ki ク ku ケ ke コ ko " +
                                    "ガ ga ギ gi グ gu ゲ ge ゴ go " +
                                    "サ sa シ shi ス su セ se ソ so " +
                                    "ザ za ジ ji ズ zu ゼ ze ゾ zo " +
                                    "タ ta チ chi ツ tsu テ te ト to " +
                                    "ダ da デ de ド do " +
                                    "ナ na ニ ni ヌ nu ネ ne ノ no " +
                                    "ハ ha ヒ hi フ fu ヘ he ホ ho " +
                                    "バ ba ビ bi ブ bu ベ be ボ bo " +
                                    "パ pa ピ pi プ pu ペ pe ポ po " +
                                    "マ ma ミ mi ム mu メ me モ mo " +
                                    "ヤ ya ユ yu ヨ yo " +
                                    "ラ ra リ ri ル ru レ re ロ ro " +
                                    "ワ wa ヲ wo ン n";
        

        Katakana = new HashSet<KatakanaData>();
        
        string katakana = null;
        string translation = null;
        
        string[] parsedLine = katakanaFile.Split(' ');
        
        for (var i = 0; i < parsedLine.Length; i++)
        {
            string cur = parsedLine[i];

            // If both fields are filled out, create a data object and clear the fields
            if (katakana != null && translation != null)
            {
                Katakana.Add(new KatakanaData(katakana, translation));
                katakana = null;
                translation = null;
            }
            
            // If even, its a katakana
            if (i % 2 == 0)
            {
                katakana = cur;
            }
            // If odd, its a translation
            else
            {
                translation = cur;
            }
        }
    }
}