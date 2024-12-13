using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//挂在笔上
public class painterPalette : MonoBehaviour
{
    public List<GameObject> colors;
    public Renderer penRenderer;
    public GameObject pen;
    // Start is called before the first frame update
    void Start()
    {
        penRenderer=pen.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // 检查碰撞到的物体是否在 ObjToDraw 列表中
        if (colors.Contains(other.gameObject))
        {
            // 获取被碰到物体的渲染器组件
            Renderer renderer = other.gameObject.GetComponent<Renderer>();
            // SpriteRenderer spriteRenderer = other.gameObject.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                // 获取调色板色块的颜色
              Color color = renderer.material.color;
               penRenderer.material.color=color;
            }
        }
    }
}
