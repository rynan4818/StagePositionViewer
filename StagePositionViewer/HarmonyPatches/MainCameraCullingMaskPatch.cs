using HarmonyLib;
using UnityEngine;
using StagePositionViewer.Configuration;

namespace StagePositionViewer.HarmonyPatches
{
    [HarmonyPatch(typeof(MainCameraCullingMask))]
    [HarmonyPatch("Start", MethodType.Normal)]
    public class MainCameraCullingMaskPatch
    {
        static void Postfix(Camera ____camera)
        {
            if (PluginConfig.Instance.FirstPersonOnly)
                ____camera.cullingMask |= 1 << PluginConfig.Instance.FirstPersonLayer;
            else
                ____camera.cullingMask &= ~(1 << PluginConfig.Instance.FirstPersonLayer);
        }
    }
}
