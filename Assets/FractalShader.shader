Shader "Custom/FractalShader"
{
    Properties
    {
        _MaxIterations ("Max Iterations", Float) = 100
        _Power ("Power", Float) = 8
        _Zoom ("Zoom", Float) = 1
        _Offset ("Offset", Vector) = (0,0,0,0)
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 z = float3(0, 0, 0);
                float3 c = float3((i.vertex.xy - 0.5) * _Zoom + _Offset.xy, _Offset.z);
                float4 color = float4(0, 0, 0, 1);

                for (int iter = 0; iter < _MaxIterations; iter++)
                {
                    z = pow(z, _Power) + c;
                    if (length(z) > 2.0)
                    {
                        color.rgb = smoothstep(0.0, 2.0, 2.0 / length(z));
                        break;
                    }
                }

                return color * _Color;
            }
            ENDCG
        }
    }
}
