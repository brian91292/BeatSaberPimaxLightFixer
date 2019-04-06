﻿using Harmony;
using IllusionPlugin;
using System.Collections;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PimaxLightFixer
{
    public class Plugin : IPlugin
    {
        public string Name => "PimaxUnfucker";
        public string Version => "1.0.2";
        public void OnApplicationStart()
        {
            SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;

            var harmonyInstance = HarmonyInstance.Create("com.brian91292.beatsaber.pimaxunfucker");
            harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
        }

        private void SceneManagerOnActiveSceneChanged(Scene arg0, Scene arg1)
        {
            if(Config.DisableLighting)
                SharedCoroutineStarter.instance.StartCoroutine(DisablePrePassLights());

            SharedCoroutineStarter.instance.StartCoroutine(SwapSpriteTextShader());
        }

        private IEnumerator DisablePrePassLights()
        {
            yield return new WaitForSeconds(0.1f);

            Resources.FindObjectsOfTypeAll<BloomPrePassLight>()?.ToList().ForEach(t =>
            {
                var tube = t.GetComponentInChildren<TubeBloomPrePassLight>();
                if (tube)
                    tube.enabled = false;
            });
        }

        private IEnumerator SwapSpriteTextShader()
        {
            yield return new WaitForSeconds(0.1f);

            Material material = Resources.FindObjectsOfTypeAll<Material>().Where(m => m.name == "UINoGlow").FirstOrDefault();

            Resources.FindObjectsOfTypeAll<UnityEngine.UI.Image>()?.ToList().ForEach(t =>
            {
                var mat = Material.Instantiate(material);
                mat.color = t.material.color.ColorWithAlpha(t.material.color.a == 0 ? 0.7f : t.material.color.a);
                t.material = mat;
            });

            Resources.FindObjectsOfTypeAll<TMP_Text>()?.ToList().ForEach(t =>
            {
                var mat = Material.Instantiate(material);
                mat.color = t.material.color.ColorWithAlpha(t.material.color.a == 0 ? 0.7f : t.material.color.a);
                t.material = mat;
            });
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (arg0.name == "MenuCore")
                Settings.OnLoad();
        }

        public void OnApplicationQuit()
        {
            SceneManager.activeSceneChanged -= SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        }

        public void OnLevelWasLoaded(int level)
        {
        }

        public void OnLevelWasInitialized(int level)
        {
        }

        public void OnUpdate()
        {
        }

        public void OnFixedUpdate()
        {
        }
    }
}
