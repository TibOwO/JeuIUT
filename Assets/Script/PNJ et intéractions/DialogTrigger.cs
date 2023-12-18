using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public Dialog dialog;
    public bool isInRange = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isInRange)
        {
            TriggerDialog();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = true;
            Debug.Log("Le joueur est dans la zone de dialogue");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TriggerExit();
        }
    }

    public void TriggerExit()
    {
        isInRange = false;
        Debug.Log("Le joueur a quitté la zone de dialogue");
    }

    public void TriggerDialog()
    {
        FindObjectOfType<DialogManager>().StartDialog(dialog);
    }
}
