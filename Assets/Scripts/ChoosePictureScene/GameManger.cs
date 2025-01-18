using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public  GameObject pictureprefab;  
    public String contentClass;
    public static Material savedMaterial;  
    public  Vector3 finalPosition;
    public  Quaternion finalRotation;
    public bool hasSpawned;
    public bool isCombination=false;
    public bool drawPathCheck=false;
    public bool dragFillColorCheck=false;
    public bool complete=false;//两个任务都做完
    // public int data=0;
    public GameObject selectedPicture; //之前的choosedPicture
    private static GameManager instance;  
    

    public GameObject Pictureprefab{
        get{return pictureprefab;}
    }

    public String ContentClass{
        get{return contentClass;}
    }

    public Vector3 FinalPosition{
        get{return finalPosition;}
    }

    public Quaternion FinalRotation{
        get{return finalRotation;}
    }
    public bool HasSpawned{
        get{return hasSpawned;}
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
