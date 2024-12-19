using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public GameObject  checkCube;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeScene1(){
        SceneManager.LoadScene("DirectlySpawn");
        // GameManager.Instance.pictureprefab.SetActive(false);
    }

    public void ChangeScene2(){
        
        // GameManager.Instance.pictureprefab.SetActive(false);

         if(GameManager.Instance.contentClass=="Cat"){
                SceneManager.LoadScene("PathSpawn");
        }
        else if(GameManager.Instance.contentClass=="Flower"){
                SceneManager.LoadScene("PathSpawn_square");
        }
        else{
                SceneManager.LoadScene("PathSpawn_triangle");
        }
    }

    public void ChangeScene3(){
        
        if(GameManager.Instance.contentClass=="Cat"){
                SceneManager.LoadScene("SpraySpawn");
        }
        else if(GameManager.Instance.contentClass=="Flower"){
                SceneManager.LoadScene("SpraySpawn_flower");
        }
        else{
                SceneManager.LoadScene("SpraySpawn_tree");
        }
    }
    public void ChangeScene4(){
        
         if(GameManager.Instance.contentClass=="Cat"){
                SceneManager.LoadScene("fillColor");
        }
        else if(GameManager.Instance.contentClass=="Flower"){
                SceneManager.LoadScene("fillColor");
        }
        else{
                SceneManager.LoadScene("fillColor");
        }
    }
    public void ChangeScene5(){
        
        if(GameManager.Instance.contentClass=="Cat"){
                SceneManager.LoadScene("DragFillColor_cat");
        }
        else if(GameManager.Instance.contentClass=="Flower"){
                SceneManager.LoadScene("DragFillColor");
        }
        else{
                SceneManager.LoadScene("DragFillColor_tree");
        }
        // GameManager.Instance.pictureprefab.SetActive(false);
    }

    public void ChangeChoosePicture(){
        SceneManager.LoadScene("ChoosePicture");
    }

    public void changeCombination(String contentClass){
        GameManager.Instance.contentClass=contentClass;
        GameManager.Instance.isCombination=true;
        SceneManager.LoadScene("Combination");
    }

    public void check(){
        Renderer r=checkCube.GetComponent<Renderer>();
        r.material.color=Color.red;
    }
}
