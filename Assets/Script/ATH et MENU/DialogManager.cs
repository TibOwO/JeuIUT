using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DialogManager : MonoBehaviour
{
    public Animator Animator;
    private Queue<string> sentences;
    public TMP_Text nameText;
    public TMP_Text dialogText;

    public static DialogManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Plusieurs instancx de DialogManager ont été trouvées dans la scène.");
            return;
        }
        Instance = this;

        sentences = new Queue<string>();
    }

    public void StartDialog(Dialog dialog)
    {
        Debug.Log("Starting conversation with " + dialog.name);
        Animator.SetBool("isOpen", true);

        nameText.text = dialog.name;

        sentences.Clear();

        foreach (string sentence in dialog.sentences)
        {
            sentences.Enqueue(sentence);
        }

    }
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialog();
            return;
        }
        string sentence = sentences.Dequeue();
        dialogText.text = sentence;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    public IEnumerator TypeSentence(string sentence)
    {
        dialogText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void EndDialog()
    {
        Animator.SetBool("isOpen", false);
    }

}
