using System.Collections.Generic;
using UnityEngine;

public class CleManager : MonoBehaviour
{
    public static CleManager Instance;
    public HashSet<string> pickedUpItems = new HashSet<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void MarkItemAsPickedUp(string itemName)
    {
        pickedUpItems.Add(itemName);
    }

    public bool WasItemPickedUp(string itemName)
    {
        return pickedUpItems.Contains(itemName);
    }
}
