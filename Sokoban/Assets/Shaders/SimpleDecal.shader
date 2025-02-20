Shader "Custom/SimpleDecalDepthFade"
{
    Properties
    {
        _DecalTex ("Decal Texture", 2D) = "white" {}
        _FadeStart ("Fade Start Distance", Float) = 0.0
        _FadeEnd ("Fade End Distance", Float) = 10.0
        _Cutoff ("Alpha Cutoff", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "Queue" = "Overlay" "IgnoreProjector" = "True" }
        Pass
        {
            Tags { "LightMode" = "Always" }
            Cull Back
            ZWrite On
            ZTest LEqual
            Fog { Mode Off }

            // Используем более мягкое смешивание, чтобы избежать лишнего освещения
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            uniform float4x4 unity_Projector;
            sampler2D _DecalTex;
            float _FadeStart;
            float _FadeEnd;
            float _Cutoff;
            
            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 projPos : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };
            
            v2f vert(appdata_full v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.projPos = mul(unity_Projector, v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }
            
            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.projPos.xy / i.projPos.w;
                fixed4 col = tex2D(_DecalTex, uv);
                
                // Вычисляем расстояние от камеры до декали
                float d = distance(_WorldSpaceCameraPos.xyz, i.worldPos);
                
                // Линейное затухание в диапазоне от _FadeStart до _FadeEnd
                float fade = saturate((_FadeEnd - d) / (_FadeEnd - _FadeStart));
                col.a *= fade; // Применяем затухание альфы
                
                // Отбрасываем фрагменты с альфой ниже Cutoff
                clip(col.a - _Cutoff);
                
                // Добавляем небольшую компенсацию для цвета, чтобы избежать изменения яркости
                col.rgb *= col.a; 

                return col;
            }
            ENDCG
        }
    }
}
