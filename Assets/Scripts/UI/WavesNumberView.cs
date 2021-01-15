using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class WavesNumberView : MonoBehaviour
{
    [SerializeField] private TMP_Text _textPrefab;
    [SerializeField] private GameLoop _game;

    private Color _defaultColor;
    private List<TMP_Text> _letters = new List<TMP_Text>();
    private List<Vector3> _defaultLettersPosition = new List<Vector3>();
    private int _preRenderedLetters = 0;
    private RectTransform _rectTransform;
    private bool _rendered = false;
    private Coroutine _hideWork;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _defaultColor = _textPrefab.color;

        Clear();
        BuildLetters();
    }

    private void Clear()
    {
        while (_letters.Count > 0)
        {
            TMP_Text text = _letters[0];
            _letters.Remove(text);
            Destroy(text.gameObject);
        }
        _rendered = false;
        _preRenderedLetters = 0;
        _defaultLettersPosition.Clear();
    }

    private void BuildLetters()
    {
        string str = "волна_1";
        foreach (var letter in str)
        {
            TMP_Text text = Instantiate(_textPrefab, transform);
            text.raycastTarget = false;
            text.text = letter + "";
            text.OnPreRenderText += OnPreRenderText;
            _letters.Add(text);
        }
    }

    private void OnPreRenderText(TMP_TextInfo info)
    {
        _preRenderedLetters++;

        if (_preRenderedLetters == _letters.Count)
        {
            _rendered = true;
            BuildString();
        }
    }

    private void BuildString()
    {
        float offset = 0;
        float totalWidth = 0;
        foreach (var letter in _letters)
            totalWidth += letter.textBounds.size.x;

        Vector3 center = new Vector3(_rectTransform.rect.width / 2, _rectTransform.rect.height / 2, 0);
        for (int i = 0; i < _letters.Count; i++)
        {
            Vector3 position = center + new Vector3(offset - totalWidth / 2, 0, 0);
            _letters[i].transform.position = position;
            _letters[i].alpha = 0;
            _defaultLettersPosition.Add(position);

            offset += _letters[i].textBounds.size.x * 0.8f;

            if (_letters[i].text == "_")
                _letters[i].text = " ";
        }
    }

    private void OnEnable()
    {
        _game.WaveStarted += OnWaveStarted;
    }

    private void OnDisable()
    {
        _game.WaveStarted -= OnWaveStarted;
    }

    public void OnWaveStarted(int index) => StartCoroutine(TryShow(index));

    private IEnumerator TryShow(int index)
    {
        while (!_rendered)
            yield return new WaitForSeconds(0.1f);

        ShowWave(index);
    }

    private void ShowWave(int index)
    {
        _letters[_letters.Count - 1].text = (index + 1) + "";
        ShowText();
        if (_hideWork != null)
            StopCoroutine(_hideWork);

        _hideWork = StartCoroutine(HideText());
    }

    public void ShowText()
    {
        for (int i = 0; i < _letters.Count; i++)
        {
            var letter = _letters[i];
            float y = _defaultLettersPosition[i].y;
            letter.transform.position = _defaultLettersPosition[i] + Vector3.up * 300;
            letter.alpha = 0;

            letter.DOKill();
            letter.transform.DOKill();
            letter.DOColor(_defaultColor, 0.15f).SetDelay(0.05f * i);
            letter.transform.DOMoveY(y, 0.2f).SetDelay(0.05f * i);
        }
    }

    private IEnumerator HideText()
    {
        yield return new WaitForSeconds(1);

        for (int i = 0; i < _letters.Count; i++)
        {
            var letter = _letters[i];
            float y = _defaultLettersPosition[i].y;
            letter.transform.position = _defaultLettersPosition[i];
            letter.alpha = 1;
            letter.color = _defaultColor;

            letter.DOKill();
            letter.transform.DOKill();
            letter.DOFade(0, 0.2f).SetDelay(0.05f * i);
            letter.transform.DOMoveY(y - 300, 0.2f).SetDelay(0.05f * i);
        }
    }

    private void OnRectTransformDimensionsChange()
    {
        Clear();
        BuildLetters();
    }
}
