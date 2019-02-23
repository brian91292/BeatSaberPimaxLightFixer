using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PimaxLightFixer
{
    [HarmonyPatch(typeof(Camera), new Type[] { typeof(Camera.StereoscopicEye) })]
    [HarmonyPatch("GetStereoProjectionMatrix", MethodType.Normal)]
    public class CameraPatches
    {
        public static void Postfix(Camera.StereoscopicEye eye, ref Matrix4x4 __result)
        {
            __result[8] = eye == Camera.StereoscopicEye.Left ? -0.24f : 0.24f;
        }
    }
}
