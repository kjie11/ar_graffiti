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
        SceneManager.LoadScene("PathSpawn");
        // GameManager.Instance.pictureprefab.SetActive(false);
    }

    public void ChangeScene3(){
        SceneManager.LoadScene("SpraySpawn");
        // GameManager.Instance.pictureprefab.SetActive(false);
    }
    public void ChangeScene4(){
        SceneManager.LoadScene("fillColor");
        // GameManager.Instance.pictureprefab.SetActive(false);
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

    public void check(){
        Renderer r=checkCube.GetComponent<Renderer>();
        r.material.color=Color.red;
    }
}
