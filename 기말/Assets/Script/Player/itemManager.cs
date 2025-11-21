using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager itemManager;
    private Dictionary<string, int> item = new Dictionary<string, int>();


    public static bool HasGun = false;

    private void Awake()
    {
        if (itemManager == null)
        {
            itemManager = this;
        }
        else
        {
            Debug.LogError("¡ﬂ∫πµ» ¿ŒΩ∫≈œΩ∫");
            Destroy(gameObject);
        }
    }

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            DisplayInventory();
        }
        
    }


    public void getItem(string itemName, int quantity)
    {
        if (item.ContainsKey(itemName))
        {
            Debug.Log(itemName + "»πµÊ");
            item[itemName] += quantity;
        }
        else
        {
            Debug.Log(itemName + "»πµÊ");
            item.Add(itemName, quantity);
        }
    }

    public void RemoveItem(string itemName, int quantity)
    {
        if (item.ContainsKey(itemName))
        {
            if (item[itemName] < quantity)
            {
                Debug.Log("ºˆ∑Æ∫Œ¡∑");
            }
            else 
            {
                item[itemName] -= quantity;
                Debug.Log("¡¶∞≈µ ");
            }

            if (item[itemName] == 0)
            {
                item.Remove(itemName);
            }
            
        }

    }

    private void DisplayInventory()
    {
        string inventoryStatus = "";
        foreach (KeyValuePair<string, int> item in item)
        {
            inventoryStatus += $"{item.Key}: {item.Value}∞≥\n";
        }
        Debug.Log(inventoryStatus);
    }

}
