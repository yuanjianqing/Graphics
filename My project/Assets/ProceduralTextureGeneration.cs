﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ProceduralTexture : MonoBehaviour
{
    public Material material = null;

    #region Material properties
    [SerializeField, SetProperty("textureWidth")]
    private int m_textureWidth = 512;
    public int textureWidth
    {
        get { return m_textureWidth; }
        set { m_textureWidth = value; _UpdateMaterial(); }
    }

    [SerializeField, SetProperty("circleColor")]
    private Color m_circleColor = Color.white;
    public Color circleColor
    {
        get { return m_circleColor; }
        set { m_circleColor = value; _UpdateMaterial(); }
    }

    [SerializeField, SetProperty("blurFactor")]
    private float m_blurFactor = 2.0f;
    public float blurFactor
    {
        get { return m_blurFactor; }
        set { m_blurFactor = value; _UpdateMaterial(); }
    }

    #endregion

    private Texture2D m_generatedTexture = null;

    private void Start()
    {
        if (material == null)
        {
            Renderer renderer = gameObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                Debug.LogWarning("Cannot find a renderer.");
                return;
            }
            material = renderer.sharedMaterial;
        }
        _UpdateMaterial();
    }

    private void _UpdateMaterial()
    {
        if (material != null)
        {
            m_generatedTexture = _GenerateProceduralTexture();
        }
    }

    private Texture2D _GenerateProceduralTexture()
    {
        Texture2D proceduralTexture = new Texture2D(textureWidth, textureWidth);

        //定义外圆半径的大小
        float radius = 1f;
        //定义圆环宽度
        float circleWidth = 0.2f;
        //定义模糊系数
        float edgeBlur = 1.0f / blurFactor;
        //模糊的边缘宽度
        float edgeBlurWidth = 0.2f;

        for (int w = 0; w < textureWidth; w++)
        {
            for (int h = 0; h < textureWidth; h++)
            {
                //使用纯透明的颜色初始化
                Color pixel = new Color(0, 0, 0, 0);
                {
                    //计算当前所绘制内圆到圆心距离
                    float dist = Vector2.Distance(new Vector2(w, h), new Vector2(0, 0)) - radius;

                    //模糊边界
                    Color color = new Color(circleColor.r, circleColor.g, circleColor.b, Mathf.SmoothStep(radius - circleWidth - edgeBlurWidth, radius - circleWidth, dist) - Mathf.SmoothStep(radius, edgeBlurWidth + radius, dist));

                    //与之前的颜色混合
                    pixel = _MixColor(pixel, color, color.a);
                }
                proceduralTexture.SetPixel(w, h, pixel);
            }
        }
        proceduralTexture.Apply();
        return proceduralTexture;
    }

    private Color _MixColor(Color color0, Color color1, float mixFactor)
    {
        Color mixColor = Color.white;
        mixColor.r = Mathf.Lerp(color0.r, color1.r, mixFactor);
        mixColor.g = Mathf.Lerp(color0.g, color1.g, mixFactor);
        mixColor.b = Mathf.Lerp(color0.b, color1.b, mixFactor);
        mixColor.a = Mathf.Lerp(color0.a, color1.a, mixFactor);
        return mixColor;
    }
}