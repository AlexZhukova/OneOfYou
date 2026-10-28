void AdditionalLightsToon_float(float3 WorldPos, float3 NormalWS, float BandCount, out float3 Diffuse)
{
    Diffuse = 0;
#ifndef SHADERGRAPH_PREVIEW
    uint lightCount = GetAdditionalLightsCount();
    for (uint i = 0u; i < lightCount; i++)
    {
        Light light = GetAdditionalLight(i, WorldPos, half4(1, 1, 1, 1));
        float NdotL = saturate(dot(NormalWS, light.direction));
        float litMask = NdotL * light.distanceAttenuation * light.shadowAttenuation;
        float banded = saturate(floor(litMask * BandCount) / max(BandCount - 1, 1));
        Diffuse += banded * light.color;
    }
#endif
}