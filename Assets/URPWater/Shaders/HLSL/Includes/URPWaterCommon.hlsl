//-----------------------------------------------------------------------------
// Copyright(C) Yan Verde - All Rights Reserved
// Copyright protected under Unity Asset Store EULA
// Refer to https://unity3d.com/legal/as_terms for more informations
// -----------------------------------------------------------------------------
// URP Water
// Author : Yan Verde
// Date : April 10, 2021
// -----------------------------------------------------------------------------


#ifndef URPWATER_COMMON_INCLUDED
#define URPWATER_COMMON_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

Varyings vert(Attributes v)
{
	Varyings OUT;

	UNITY_SETUP_INSTANCE_ID(v);
	UNITY_TRANSFER_INSTANCE_ID(v, OUT);
	UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

	ComputeWaves(v);

	float3 worldPos = TransformObjectToWorld(v.vertex.xyz);

	#if _WORLD_UV
		v.texcoord.xy = worldPos.xz * 0.1;
	#endif

	#if _DYNAMIC_EFFECTS_ON
		OUT.projectionUV = ProjectionUV(_DynamicEffectsParams, worldPos);
		float4 dynamicTex = SAMPLE_TEXTURE2D_LOD(_DynamicEffectsTexture, URPWater_linear_clamp_sampler, OUT.projectionUV, 0);
		float displacement = dynamicTex.a * _DynamicDisplacement;

		v.vertex.y += displacement;
		worldPos.y += displacement;
	#endif

	OUT.pos = TransformObjectToHClip(v.vertex.xyz);
	OUT.color = v.color;

	VertexNormalInputs vertexTBN = GetVertexNormalInputs(v.normal, v.tangent);

	OUT.normal = float4(vertexTBN.normalWS, worldPos.x);
	OUT.tangent = float4(vertexTBN.tangentWS, worldPos.y);
	OUT.bitangent = float4(vertexTBN.bitangentWS, worldPos.z);

	OUT.worldNormal = OUT.normal.xyz;
	OUT.screenCoord = ComputeScreenPos(OUT.pos);
	OUT.screenCoord.z = ComputePixelDepth(worldPos); // ComputeEyeDepth




	//NormalMap UVa

	#if _NORMALSMODE_FLOWMAP
		OUT.texcoord.xy = v.texcoord.xy * _NormalMapATilings.xy;
		OUT.texcoord.zw = v.texcoord.xy * _FlowTiling.xy + _FlowTiling.zw;
	#else
		OUT.texcoord = DualAnimatedUV(v.texcoord, _NormalMapATilings, _NormalMapASpeeds);
	#endif

	#if _NORMALSMODE_DUAL || _FOAM_ON
		OUT.texcoord1 = float4(0, 0, 0, 0);
	#endif

	#if _NORMALSMODE_DUAL
		OUT.texcoord1.xy = AnimatedUV(v.texcoord.xy, _NormalMapBTilings.xy, _NormalMapBSpeeds.xy).xy;
	#endif

	#if _NORMAL_FAR_ON
		OUT.texcoord2 = DualAnimatedUV(v.texcoord, _NormalMapFarTilings, _NormalMapFarSpeeds);
	#endif

	#if _NORMAL_FAR_ON || _DISPLACEMENTMODE_GERSTNER
		OUT.texcoord3 = float4(0, 0, 0, 0);
		OUT.texcoord3.x = saturate(OUT.screenCoord.z / _NormalFarDistance);

		#if _DISPLACEMENTMODE_GERSTNER
		OUT.texcoord3.y = v.waveHeight;
		#endif
	#endif


	// FoamUV
	#if _FOAM_ON
		OUT.texcoord1.zw = v.texcoord.xy * _FoamTiling.xy;
	#endif

	UNITY_TRANSFER_FOG(OUT, OUT.pos);

	return OUT;
}

FRONT_FACE_TYPE vFace : FRONT_FACE_SEMANTIC;


#if _DOUBLE_SIDED_ON
float4 frag(Varyings IN, FRONT_FACE_TYPE vFace : FRONT_FACE_SEMANTIC) : SV_Target
#else
float4 frag(Varyings IN) : SV_Target
#endif
{
	UNITY_SETUP_INSTANCE_ID(IN);

	GlobalData data;
	InitializeGlobalData(data, IN);

	#if _DOUBLE_SIDED_ON
	data.vFace = IS_FRONT_VFACE(vFace, true, false);
	#endif

	ComputeDynamicData(data, IN);
	ComputeNormals(data, IN);
	ComputeRefractionData(data);
	ComputeScattering(data, IN);
	ComputeFoam(data, IN);
	ComputeLighting(data, IN);
	ComputeReflections(data, IN);

	//return saturate(data.debug);

	UNITY_APPLY_FOG(IN.fogCoord, data.finalColor);

	ComputeAlpha(data, IN);

	float4 output;
	output.rgb = data.finalColor.rgb;
	output.a = 1;


	return output;

}


#endif