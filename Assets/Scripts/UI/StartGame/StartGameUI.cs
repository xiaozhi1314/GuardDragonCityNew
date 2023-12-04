using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGameUI : MonoBehaviour
{
    public Button startGameBtn;
    // Start is called before the first frame update
    void Start()
    {
        startGameBtn.onClick.AddListener(() => {StartGameBtnClick();});
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void StartGameBtnClick(){
        SceneManager.LoadScene("SampleScene");
    }
}
