// 1. Crie um shader personalizado (ex: "AlphaBorder.shader")
Shader "Custom/AlphaBorder" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _BorderColor ("Border Color", Color) = (1, 0.843, 0, 1) // Dourado
        _BorderWidth ("Border Width", Range(0, 0.2)) = 0.05
        _BorderSharpness ("Border Sharpness", Range(1, 10)) = 5
    }
    
    SubShader {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
        
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _BorderColor;
            float _BorderWidth;
            float _BorderSharpness;
            
            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv);
                float alpha = col.a;
                
                // Detecta bordas baseado no alpha
                float border = 0;
                float2 uv = i.uv;
                
                // Amostra pixels ao redor
                float samples = 0;
                for (float x = -1; x <= 1; x += 0.5) {
                    for (float y = -1; y <= 1; y += 0.5) {
                        samples += tex2D(_MainTex, uv + float2(x, y) * _BorderWidth).a;
                    }
                }
                
                // Calcula a borda
                float edge = saturate(samples / 16.0 - alpha * _BorderSharpness);
                edge = pow(edge, 2);
                
                // Combina cor original com borda
                col.rgb = lerp(col.rgb, _BorderColor.rgb, edge * _BorderColor.a);
                col.a = max(col.a, edge);
                
                return col;
            }
            ENDCG
        }
    }
}