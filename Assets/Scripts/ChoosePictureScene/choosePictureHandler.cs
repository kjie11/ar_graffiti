using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// public class GameManager : MonoBehaviour
// {
//     public static Material savedMaterial;  // 静态变量保存材质
//     public  static GameObject Pictureprefab;
//     // 确保在场景切换时保持数据
//     private static GameManager instance; 

//     private void Awake()
//     {
//         // if (instance == null)
//         // {
//         //     instance = this;  // 设置唯一的 GameManager 实例
//         //     DontDestroyOnLoad(gameObject);  // 保证跨场景保留
//         // }
//         // else
//         // {
//         //     Destroy(gameObject);  // 如果已有实例，销毁新创建的 GameManager
//         // }

//         DontDestroyOnLoad(gameObject); 
//     }
// }

public class choosePictureHandler : MonoBehaviour
{
    public GameObject choosedPicture;
    Renderer r;
    public TextMeshProUGUI checkText;
    // public GameObject spawnGameObj;
    // public GameObject spawnPrefab;
    // public SceneAsset nextScene;
    public String sceneName;
    public String contentClass;
     public TextMeshProUGUI graffitiTitle;
    // Start is called before the first frame update
    void Awake(){
        DontDestroyOnLoad(choosedPicture); 
    }
    void Start()
    {
        
         r=choosedPicture.GetComponent<Renderer>();
        // string sceneName = nextScene.name;
        GameManager.Instance.selectedPicture=choosedPicture;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    public void OnButtonClick()
    {
        r.material.color = Color.blue;
        // 获取当前脚本所在物体的 Transform
        Transform contentTransform = transform.Find("Content");
        if (contentTransform != null)
        {
            Transform backgroundTransform = contentTransform.Find("Background");
            if (backgroundTransform != null)
            {
                Transform elementsTransform = backgroundTransform.Find("Elements");
                if (elementsTransform != null)
                {
                    // 查找 Elements 下的 Image 组件
                    Image image = elementsTransform.GetComponentInChildren<Image>();
                    if (image != null)
                    {
                        
                         CreateMaterialFromImage(image);
                        //  GameManager.Instance.pictureprefab = choosedPicture;
                         GameManager.Instance.contentClass=contentClass;
                         GameManager.Instance.selectedPicture=choosedPicture;
                         GameManager.Instance.pictureprefab = GameManager.Instance.selectedPicture;
                         if(contentClass=="Cat"){
                            graffitiTitle.text="Cat";
                         }
                         else if(contentClass=="Flower"){
                            graffitiTitle.text="Flower";
                         }
                         else{
                            graffitiTitle.text="Tree";
                         }
                        //  GameManager.Instance.data=1;
                        //  spawnGameObj.SetActive(true);
                        //  startSpawn();
                        checkText.text=""+GameManager.Instance.contentClass;
                        //  SceneManager.LoadScene(sceneName);
                    }
                    else
                    {
                        r.material.color = Color.gray;
                    }
                }
                else
                {
                    Debug.Log("Elements not found under Background.");
                }
            }
            else
            {
                Debug.Log("Background not found under Content.");
            }
        }
        else
        {
            Debug.Log("Content not found.");
        }
    }

   private void CreateMaterialFromImage(Image image)
    {
        // checkText.text="in the material method";
        // Create a new material
        Material newMaterial = new Material(Shader.Find("Standard"));
        // checkText.text="created new mat";
        // Apply the Image's sprite texture to the material
        newMaterial.mainTexture = image.sprite.texture;
         GameManager.savedMaterial = newMaterial;

        // Apply the new material to the checkCube's renderer
        if (choosedPicture != null)
        {
            choosedPicture.GetComponent<Renderer>().material = newMaterial;
            GameManager.Instance.selectedPicture.GetComponent<Renderer>().material=newMaterial;
        }
        else
        {
            Debug.LogError("checkCube is not assigned.");
        }

        
    }


}
