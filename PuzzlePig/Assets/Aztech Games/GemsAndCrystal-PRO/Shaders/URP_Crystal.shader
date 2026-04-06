Shader "Custom/URP_Crystal_Blend"
{
    Properties
    {
        [HDR]_BaseColor ("Base Color", Color) = (0.5, 0.8, 1, 1)
        [HDR]_BlendColor ("Blend Color", Color) = (1, 0.5, 0.8, 1)
        
        _BlendHeight ("Blend Height", Range(-1,1)) = 0.5
        _BlendSmoothness ("Blend Smoothness", Range(0.01,1)) = 0.1

        _Alpha ("Transparency", Range(0,1)) = 0.5

        [HDR]_FresnelColor ("Fresnel Color (HDR)", Color) = (1,1,1,1)
        _FresnelPower ("Fresnel Power", Range(0,10)) = 3

        _Metallic ("Metallic", Range(0,1)) = 0.7
        _Smoothness ("Smoothness", Range(0,1)) = 0.8
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 300

        Pass
        {
            Tags { "LightMode"="UniversalForward" }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Back
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionLS : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float3 viewDirWS : TEXCOORD2;
                UNITY_FOG_COORDS(3)
            };

            float4 _BaseColor;
            float4 _BlendColor;
            float _BlendHeight;
            float _BlendSmoothness;

            float _Alpha;
            float4 _FresnelColor;
            float _FresnelPower;
            float _Metallic;
            float _Smoothness;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = UnityObjectToClipPos(float4(IN.positionOS, 1.0));
                OUT.positionLS = IN.positionOS;
                OUT.normalWS = UnityObjectToWorldNormal(IN.normalOS);

                float3 camPosWS = _WorldSpaceCameraPos;
                float3 worldPos = mul(unity_ObjectToWorld, float4(IN.positionOS, 1)).xyz;
                OUT.viewDirWS = normalize(camPosWS - worldPos);

                UNITY_TRANSFER_FOG(OUT, OUT.positionCS);
                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                float height = IN.positionLS.y;

                // Soft blend factor
                float blend = smoothstep(_BlendHeight - _BlendSmoothness * 0.5,
                                         _BlendHeight + _BlendSmoothness * 0.5,
                                         height);

                float4 baseColor = lerp(_BaseColor, _BlendColor, blend);

                // Fresnel
                float fresnel = pow(1.0 - saturate(dot(IN.viewDirWS, IN.normalWS)), _FresnelPower-5);
                float4 fresnelCol = _FresnelColor / fresnel;

                float4 finalColor = baseColor + fresnelCol;
                finalColor.a = _Alpha;

                // Simple lighting with metallic and smoothness
                float3 normal = normalize(IN.normalWS);
                float3 viewDir = normalize(IN.viewDirWS);
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                float3 halfDir = normalize(lightDir + viewDir);

                float NdotL = saturate(dot(normal, lightDir));
                float NdotH = saturate(dot(normal, halfDir));
                float spec = pow(NdotH, _Smoothness * 128.0) * _Metallic;

                float3 litColor = finalColor.rgb * NdotL + spec;
                float4 outputColor = float4(litColor, finalColor.a);

                UNITY_APPLY_FOG(IN.fogCoord, outputColor);
                return outputColor;
            }

            ENDHLSL
        }
    }
    FallBack "Universal Forward"
}
