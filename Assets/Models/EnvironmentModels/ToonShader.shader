Shader "XYZ/ToonShader"
{
    Properties
    {
        _MainColor ("Color", Color) = (0.8,0.8,0.8,1)
        _ShadowColor("Shadow Color", Color) = (0.5,0.5,0.5,1)
        _ShadowSoftness("Shadow Softness", Range(0, 1)) = 0.5
        _FogColor("Fog Color", Color) = (0.5,0.5,0.5,0.5)
    }
    SubShader
    {
        Tags 
        { 
            "RenderType"="Opaque" 
            "LightMode" = "ForwardBase"
            "PassFlags" = "OnlyDirectional"
        }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
            #pragma multi_compile_fwdbase
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 pos : SV_POSITION;
                float3 worldNormal : NORMAL;
                SHADOW_COORDS(2)
                float3 worldPos : TEXCOORD3;
            };

            float4 _MainColor;
            float4 _ShadowColor;
            float _ShadowSoftness;
            float4 _FogColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                TRANSFER_SHADOW(o)
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 normal = normalize(i.worldNormal);
                float NdotL = dot(_WorldSpaceLightPos0, normal);
                float shadow = SHADOW_ATTENUATION(i);
                float lightIntensity = smoothstep(0, _ShadowSoftness, NdotL * shadow);
                // sample the texture
                float4 col = _MainColor;
                // apply fog

                float fog = (-i.worldPos.y / 20);

                float4 finalColor = lerp(_ShadowColor, col, lightIntensity);

                UNITY_APPLY_FOG(i.fogCoord, col);
                
                return lerp(finalColor, _FogColor, clamp(fog,0,1));
            }
            ENDCG
        }
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}
