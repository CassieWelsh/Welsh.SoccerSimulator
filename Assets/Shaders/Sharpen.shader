Shader "Custom/SharpenFilter"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "grey" {}
        _Amount ("Amount", Range(0.0, 5.0)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _Amount;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 offset = 1.0 / _ScreenParams.xy;

                float3 sum = float3(0.0, 0.0, 0.0);

                // Apply the sharpening filter
                sum += tex2D(_MainTex, i.uv + float2(-offset.x, -offset.y)).rgb * (-1.0);
                sum += tex2D(_MainTex, i.uv + float2(0.0, -offset.y)).rgb * (-1.0);
                sum += tex2D(_MainTex, i.uv + float2(offset.x, -offset.y)).rgb * (-1.0);
                sum += tex2D(_MainTex, i.uv + float2(-offset.x, 0.0)).rgb * (-1.0);
                sum += tex2D(_MainTex, i.uv).rgb * (8.0 + _Amount);
                sum += tex2D(_MainTex, i.uv + float2(offset.x, 0.0)).rgb * (-1.0);
                sum += tex2D(_MainTex, i.uv + float2(-offset.x, offset.y)).rgb * (-1.0);
                sum += tex2D(_MainTex, i.uv + float2(0.0, offset.y)).rgb * (-1.0);
                sum += tex2D(_MainTex, i.uv + float2(offset.x, offset.y)).rgb * (-1.0);

                return float4(sum, 1.0);
            }
            ENDCG
        }
    }
}
