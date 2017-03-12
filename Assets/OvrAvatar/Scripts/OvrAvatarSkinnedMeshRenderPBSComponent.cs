using UnityEngine;
using System.Collections;
using System;
using Oculus.Avatar;

public class OvrAvatarSkinnedMeshRenderPBSComponent : OvrAvatarRenderComponent {
    SkinnedMeshRenderer mesh;
    Transform[] bones;

    internal void Initialize(ovrAvatarRenderPart_SkinnedMeshRenderPBS skinnedMeshRenderPBS, Shader shader, int thirdPersonLayer, int firstPersonLayer, int sortOrder)
    {
        if (shader == null)
        {
            shader = Shader.Find("OvrAvatar/AvatarSurfaceShaderPBS");
        }
        mesh = CreateSkinnedMesh(skinnedMeshRenderPBS.meshAssetID, skinnedMeshRenderPBS.visibilityMask, thirdPersonLayer, firstPersonLayer, sortOrder);
        mesh.sharedMaterial = CreateAvatarMaterial(gameObject.name + "_material", shader);
        bones = mesh.bones;
    }

    internal void UpdateSkinnedMeshRenderPBS(OvrAvatar avatar, IntPtr renderPart)
    {
        ovrAvatarVisibilityFlags visibilityMask = CAPI.ovrAvatarSkinnedMeshRenderPBS_GetVisibilityMask(renderPart);
        ovrAvatarTransform localTransform = CAPI.ovrAvatarSkinnedMeshRenderPBS_GetTransform(renderPart);
        UpdateSkinnedMesh(avatar, mesh, bones, localTransform, visibilityMask, renderPart);

        Material mat = mesh.sharedMaterial;
        UInt64 albedoTextureID = CAPI.ovrAvatarSkinnedMeshRenderPBS_GetAlbedoTextureAssetID(renderPart);
        UInt64 surfaceTextureID = CAPI.ovrAvatarSkinnedMeshRenderPBS_GetSurfaceTextureAssetID(renderPart);
        mat.SetTexture("_Albedo", GetLoadedTexture(albedoTextureID));
        mat.SetTexture("_Surface", GetLoadedTexture(surfaceTextureID));
    }
}
