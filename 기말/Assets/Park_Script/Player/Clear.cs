using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Clear : MonoBehaviour
{
    static public bool GameClear = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerStay(Collider other)
    { 
        if(GameClear && other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("End");
        }
    }
}
