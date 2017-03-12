using UnityEngine;
using System.Collections;
using System;
using Oculus.Avatar;

public class OvrAvatarSkinnedMeshRenderComponent : OvrAvatarRenderComponent
{
    Shader surface;
    Shader surfaceSelfOccluding;
    SkinnedMeshRenderer mesh;
    Transform[] bones;

    internal void Initialize(ovrAvatarRenderPart_SkinnedMeshRender skinnedMeshRender, Shader surface, Shader surfaceSelfOccluding, int thirdPersonLayer, int firstPersonLayer, int sortOrder)
    {
        this.surfaceSelfOccluding = surfaceSelfOccluding != null ? surfaceSelfOccluding :  Shader.Find("OvrAvatar/AvatarSurfaceShaderSelfOccluding");
        this.surface = surface != null ? surface : Shader.Find("OvrAvatar/AvatarSurfaceShader");
        this.mesh = CreateSkinnedMesh(skinnedMeshRender.meshAssetID, skinnedMeshRender.visibilityMask, thirdPersonLayer, firstPersonLayer, sortOrder);
        this.bones = mesh.bones;
        UpdateMeshMaterial(skinnedMeshRender.visibilityMask);
    }

    internal void UpdateSkinnedMeshRender(OvrAvatar avatar, IntPtr renderPart)
    {
        ovrAvatarMaterialState materialState = CAPI.ovrAvatarSkinnedMeshRender_GetMaterialState(renderPart);
        ovrAvatarVisibilityFlags visibilityMask = CAPI.ovrAvatarSkinnedMeshRender_GetVisibilityMask(renderPart);
        ovrAvatarTransform localTransform = CAPI.ovrAvatarSkinnedMeshRender_GetTransform(renderPart);
        UpdateSkinnedMesh(avatar, mesh, bones, localTransform, visibilityMask, renderPart);
        UpdateMeshMaterial(visibilityMask);
        UpdateAvatarMaterial(mesh.sharedMaterial, materialState);
    }

    private void UpdateMeshMaterial(ovrAvatarVisibilityFlags visibilityMask)
    {
        Shader shader = (visibilityMask & ovrAvatarVisibilityFlags.SelfOccluding) != 0 ? surfaceSelfOccluding : surface;
        if (mesh.sharedMaterial == null || mesh.sharedMaterial.shader != shader)
        {
            mesh.sharedMaterial = CreateAvatarMaterial(gameObject.name + "_material", shader);
        }
    }
}
