Shader "AZTECHGAMES/Wireframe" 
{
	Properties 
	{
		_Color ("Line Color", Color) = (1,1,1,1)
		_MainTex ("Main Texture", 2D) = "white" {}
		_Thickness ("Thickness", Float) = 1
	}

	SubShader 
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		Pass
		{

			Blend SrcAlpha OneMinusSrcAlpha
			Cull Off
			LOD 200
			
			CGPROGRAM
				#pragma target 5.0
				#include "UnityCG.cginc"
				#include "AztechGames Wireframe Functions.cginc"
				#pragma vertex vert
				#pragma fragment frag
				#pragma geometry geom

				// Vertex Shader
				AZTECH_v2g vert(appdata_base v)
				{
					return AZTECH_vert(v);
				}
				
				// Geometry Shader
				[maxvertexcount(3)]
				void geom(triangle AZTECH_v2g p[3], inout TriangleStream<AZTECH_g2f> triStream)
				{
					AZTECH_geom( p, triStream);
				}
				
				// Fragment Shader
				float4 frag(AZTECH_g2f input) : COLOR
				{	
					float4 col = AZTECH_frag(input);
					if( col.a < 0.5f ) discard;
					else col.a = 1.0f;
					
					return col;
				}
			
			ENDCG
		}
	} 
}
