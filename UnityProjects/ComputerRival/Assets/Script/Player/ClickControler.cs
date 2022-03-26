using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickControler : Controler
{

    [Header("标志")]


    [Header("鼠标的位置")]
    public Vector2 mousePos = Vector2.zero;

    //鼠标hitInfo
    RaycastHit hitInfo;

    private void Awake()
    {
        BaseAwake();
    }

    void Update()
    {
        PhysicCheck(transform);
        BaseUpdate(); 
        if(direction != 0)
        {
            if(Mathf.Abs(transform.position.x - mousePos.x) < 0.02f)
            {
                direction = 0;
            }
        }
        SetCursorTexture();
        MouseControl();
    }

    void SetCursorTexture()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hitInfo))
        {
            //切换鼠标贴图
            /*switch (hitInfo.collider.gameObject.tag)
            {
                case "Ground": Cursor.SetCursor(target, new Vector2(16, 16), CursorMode.Auto); break;
            }
            */
        }
    }

    void MouseControl()
    {
        if (Input.GetMouseButtonDown(0))
        {

            //如果hit打中了物体，那么就把角色移动到物体处，否则就移动到鼠标坐标点处
            if (hitInfo.collider != null)
            {
                Debug.Log("12234");
                mousePos = hitInfo.collider.transform.position;
            }
            else
            {
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            direction = transform.position.x < mousePos.x ? 1 : -1;
        }
    }
}
