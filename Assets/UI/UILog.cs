using UnityEngine;
using UnityEngine.UI;

public class UILog : MonoBehaviour
{
    public enum Alignment
    {
        Left,
        Right
    }

    public static UILog Instance;

    [SerializeField]
    private Text leftText = null;
    [SerializeField]
    private Text rightText = null;
    [SerializeField]
    private string textToTrim = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void Display(PapuanState enteredState, Alignment alignment)
    {
        var name = enteredState.ToString();
        name = name.Remove(name.IndexOf(textToTrim), textToTrim.Length);
        name = name.Remove(name.IndexOf("State"), 5);

        if (alignment == Alignment.Left)
        {
            leftText.text = name;
        }
        else
        {
            rightText.text = name;
        }
    }
}