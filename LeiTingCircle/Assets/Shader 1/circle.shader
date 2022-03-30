Shader "Custom/NewSurfaceShader"
{
    Properties
    {
        _Radius("Radius", float) = 1
        _BlurWidth("BlurWidth", float) = 0.2
        _CircleWidth("CircleWidth", float) = 0.5
        _MainTex ("Texture", 2D) = "white" {}
        _SeperateDis("SeperateDis", float)  = 0.01

    }
    SubShader
    {
        Tags{"Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "TransparentCutout"}
        
        Pass
        {
            Tags {"LightMode" = "ForwardBase"}

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #include "Lighting.cginc"

            float _Radius;
            float _BlurWidth;
            float _CircleWidth;
            float4 _MainTex_ST;
            float _SeperateDis;
            
            struct a2v
            {
                float4 vertex : POSITION;
                float2 uv : Texcoord0;
            };
            struct v2f
            {
                float2 uv : texcoord;
                float4 vertex : SV_POSITION;
            };
            v2f vert(a2v v) 
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                _SeperateDis /= 10;
                float l1 = length(i.uv - 0.5 - _SeperateDis);
                float s11 = smoothstep(_Radius - _CircleWidth, _Radius - _CircleWidth + _BlurWidth, l1);
                float s12 = smoothstep(_Radius, _Radius + _BlurWidth , l1);

                float l2 = length(i.uv - 0.5);
                float s21 = smoothstep(_Radius - _CircleWidth, _Radius - _CircleWidth + _BlurWidth, l2);
                float s22 = smoothstep(_Radius, _Radius + _BlurWidth , l2);

                float l3 = length(i.uv - 0.5 + _SeperateDis);
                float s31 = smoothstep(_Radius - _CircleWidth, _Radius - _CircleWidth + _BlurWidth, l3);
                float s32 = smoothstep(_Radius, _Radius + _BlurWidth , l3);

                clip(s11 - s12 + s21 - s22 + s31 - s32 - 0.4);


                return fixed4(s11 - s12, s21 - s22, s31 - s32, (s11 - s12 + s21 - s22 + s31 - s32)/3);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
