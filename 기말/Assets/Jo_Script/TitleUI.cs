using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
    public GameObject htp;
    public Button htpButton;
    void Start()
    {
        htp.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            htp.SetActive(false);
        }
    }
    public void HtpButtonClicked(){
        htp.SetActive(true);
    }

    public void StartButtonClicked(){
        SceneManager.LoadScene("MainScene");
    }
}
