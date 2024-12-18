using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//暂时没用
public class MenuHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
     public void ChangeScene1(){
        SceneManager.LoadScene("FreeDrawCanvas");
    }

    public void ChangeScene5(){
        SceneManager.LoadScene("DragFillColor");
    }
}
