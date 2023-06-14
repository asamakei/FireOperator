Shader "CookbookShaders/Chapter02/HalfLambertDiffuse2" 
{
	Properties 
	{
		_MainTint ("Diffuse Tint", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_MySliderValue ("Lambert", Range(0,1)) = 0.5
	}
	
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf BasicDiffuse
		#pragma target 3.0

		float _MySliderValue;

		fixed4 _MainTint;
		sampler2D _MainTex;

		inline float4 LightingBasicDiffuse (SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			float difLight = dot (s.Normal, lightDir);
			//float hLambert = difLight * 0.5 + 0.5; 
			float hLambert = difLight * _MySliderValue + (1.0 - _MySliderValue);
			
			float4 col;
			col.rgb = s.Albedo * _LightColor0.rgb * (hLambert * atten * 2);
			col.a = s.Alpha;
			return col;
		}

		struct Input 
		{
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) 
		{
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb * _MainTint;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
