using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager itemManager;
    private Dictionary<string, int> item = new Dictionary<string, int>();


    public bool HasGun = false;

    int QuestItemCount;

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


    public static ItemManager Instance
    {
        get
        {
            if(itemManager == null)
            {
                return null;
            }
            return itemManager;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            DisplayInventory();
        }

        if(QuestItemCount == 4)
        {
           Debug.Log("ƒ˘Ω∫∆Æ æ∆¿Ã≈€ ∏µŒ »πµÊ");
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

        if (item.ContainsKey("Quest"))
        {
            QuestItemCount += quantity;
            Debug.Log("Qitem");
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
