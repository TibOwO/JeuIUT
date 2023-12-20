using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypeWriter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI uiText;
    public float delay = 0.2f;

    void Awake()
    {
        uiText.text = string.Empty;
    }

    public void SetText(string text)
    {
        StartCoroutine(ShowLetterByLetter(text));
    }

    IEnumerator ShowLetterByLetter(string originalText)
    {
        for (int i = 0; i <= originalText.Length; ++i)
        {
            uiText.text = originalText.Substring(0, i);
            yield return new WaitForSeconds(delay);
        }
    }

    // Reste du script...
}
