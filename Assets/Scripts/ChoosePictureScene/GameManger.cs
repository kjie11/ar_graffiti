using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public  GameObject pictureprefab;  
    public String contentClass;
    public static Material savedMaterial;  
    // public int data=0;
    
    private static GameManager instance;  
    

    public GameObject Pictureprefab{
        get{return pictureprefab;}
    }

    public String ContentClass{
        get{return contentClass;}
    }


    // 通过 GameManager.Instance 访问单例实例
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                // 查找现有的 GameManager 实例
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    instance = new GameObject("GameManager").AddComponent<GameManager>();
                }
            }
            return instance;  // 返回唯一实例
        }
    }

   
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  
        }
        else
        {
            Destroy(gameObject);  
        }
    }
}
