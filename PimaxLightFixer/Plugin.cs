using Harmony;
using IllusionPlugin;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PimaxLightFixer
{
    public class Plugin : IPlugin
    {
        public string Name => "PimaxLightFixer";
        public string Version => "1.0.1";
        public void OnApplicationStart()
        {
            SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;

            var harmonyInstance = HarmonyInstance.Create("com.brian91292.beatsaber.pimaxlightfixer");
            harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
        }

        private void SceneManagerOnActiveSceneChanged(Scene arg0, Scene arg1)
        {
            if(Config.DisableLighting)
                SharedCoroutineStarter.instance.StartCoroutine(DisablePrePassLights());
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

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (arg0.name == "Menu")
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
