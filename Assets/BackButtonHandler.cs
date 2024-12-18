using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.Samples;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButtonHandler : MonoBehaviour
{
    public String BackToScene;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void backScene(){
        SceneManager.LoadScene(BackToScene);
    }
}
