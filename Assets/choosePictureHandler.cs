using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class choosePictureHandler : MonoBehaviour
{
    public GameObject checkCube;
    Renderer r;
    public TextMeshProUGUI checkText;
    // Start is called before the first frame update
    void Start()
    {
         r=checkCube.GetComponent<Renderer>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
     // 绑定在按钮上的不同方法
    public void OnCat1_buttonClicked()
    {
        r.material.color=Color.red;
    }

    public void OnButton2Clicked()
    {
         r.material.color=Color.green;
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
        checkText.text="in the material method";
        // Create a new material
        Material newMaterial = new Material(Shader.Find("Standard"));
        checkText.text="created new mat";
        // Apply the Image's sprite texture to the material
        newMaterial.mainTexture = image.sprite.texture;

        // Apply the new material to the checkCube's renderer
        if (checkCube != null)
        {
            checkCube.GetComponent<Renderer>().material = newMaterial;
        }
        else
        {
            Debug.LogError("checkCube is not assigned.");
        }

        // Optionally update the text to show the name of the image
        checkText.text = "Image applied: " + image.sprite.name;
    }


}
