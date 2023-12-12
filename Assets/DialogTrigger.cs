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
        }

    }

    public void TriggerExit()
    {
        isInRange = false;
    }

    public void TriggerDialog()
    {
        FindObjectOfType<DialogManager>().StartDialog(dialog);
    }
}
