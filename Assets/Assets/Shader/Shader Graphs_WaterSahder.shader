Shader "Shader Graphs/WaterSahder" {
	Properties {
		_BaseColor ("BaseColor", Vector) = (0.06666667,0.1960784,0.3411765,1)
		_RippleSpeed ("RippleSpeed", Float) = 1
		_RippleDensity ("RippleDensity", Float) = 7
		_RippleSlimness ("RippleSlimness", Float) = 5
		[HDR] _RippleColor ("RippleColor", Vector) = (0.3058824,1.262745,1.87451,1)
		_tiling ("tiling", Vector) = (1,1,0,0)
		[HideInInspector] _QueueOffset ("_QueueOffset", Float) = 0
		[HideInInspector] _QueueControl ("_QueueControl", Float) = -1
		[HideInInspector] [NoScaleOffset] unity_Lightmaps ("unity_Lightmaps", 2DArray) = "" {}
		[HideInInspector] [NoScaleOffset] unity_LightmapsInd ("unity_LightmapsInd", 2DArray) = "" {}
		[HideInInspector] [NoScaleOffset] unity_ShadowMasks ("unity_ShadowMasks", 2DArray) = "" {}
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = 1;
		}
		ENDCG
	}
	Fallback "Hidden/Shader Graph/FallbackError"
	//CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
}