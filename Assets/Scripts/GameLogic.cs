using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private TMP_Text text = default;
    [SerializeField] private TMP_Text translation = default;
    [SerializeField] private TMP_Text hint = default;

    private KatakanaController _controller;
    private Slide[] _slides;

    private HashSet<KatakanaData> _available;
    
    private void Start()
    {
        _controller = KatakanaController.Instance();
        
        _available = new HashSet<KatakanaData>(_controller.Katakana);
        
        StartCoroutine(RunGame());
    }

    private void Update()
    {
        var isChecking = Input.GetKey(KeyCode.Space) || Input.touchCount > 0;
        
        translation.enabled = isChecking;
        Time.timeScale = isChecking ? 0 : 1;
    }

    private IEnumerator RunGame()
    {
        yield return StartCountdown(3);

        yield return CycleThroughSlides();

        yield return ShowAnswers();
        
        SceneManager.LoadScene("Main");
    }

    private IEnumerator ShowAnswers()
    {
        translation.gameObject.SetActive(false);
        var result = new StringBuilder();

        for (var i = 0; i < _slides.Length; i++)
        {
            result.Append($"Slide {i}: {_slides[i].GetKatakana()} = {_slides[i].GetTranslation()}\n");
        }

        result.Append("\n\nPress Enter to continue.");

        text.fontSize = 40f;
        text.text = result.ToString();
        
        yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Return) || Input.touchCount > 0);
    }

    private IEnumerator StartCountdown(int seconds)
    {
        for (var i = seconds; i > 0; i--)
        {
            text.text = i.ToString();
            yield return new WaitForSeconds(1);
        }

        hint.enabled = false;
    }

    private IEnumerator CycleThroughSlides()
    {
        GameSettings settings = _controller.GetSettings();
        _slides = new Slide[settings.SlideCount];
        var slideIndex = 0;
        
        foreach (var stage in settings.stages)
        {
            for (var i = 0; i < stage.slideCount; i++)
            {
                var katakanaBuilder = new StringBuilder();
                var translationBuilder = new StringBuilder();
                var word = new List<KatakanaData>();

                for (var j = 0; j < stage.characterCount; j++)
                {
                    if (_available.Count <= 0) _available = new HashSet<KatakanaData>(_controller.Katakana);
                    
                    var character = _available.ElementAt(Random.Range(0, _available.Count));
                    _available.Remove(character);
                    
                    katakanaBuilder.Append(character.Katakana);
                    translationBuilder.Append(character.Translation);
                    word.Add(character);
                }

                _slides[slideIndex] = new Slide(word);
                slideIndex++;
                
                text.text = katakanaBuilder.ToString();
                translation.text = translationBuilder.ToString(); 

                yield return new WaitForSeconds(stage.slideTime);    
            }
        }
    }
}