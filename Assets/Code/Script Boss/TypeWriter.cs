using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypeWriter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI uiText;
    public float delay = 0.2f;
    public bool IsTyping { get; private set; }

    void Awake()
    {
        uiText.text = string.Empty;
    }

    public void SetText(string text)
    {
        if (IsTyping)
        {
            return; 
        }
        StartCoroutine(ShowLetterByLetter(text));
    }

    IEnumerator ShowLetterByLetter(string originalText)
    {
        IsTyping = true;
        uiText.text = string.Empty;

        for (int i = 0; i <= originalText.Length; ++i)
        {
            uiText.text = originalText.Substring(0, i);
            yield return new WaitForSeconds(delay);
        }

        IsTyping = false;
    }
}
