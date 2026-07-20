using System.Collections;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using CullFixedProps.Plants;
using HarmonyLib;
using Nautilus.Handlers;

namespace CullFixedProps;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }

    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();

    private void Awake()
    {
        // plugin startup logic
        Logger = base.Logger;

        // register prefab modifications in early wait screen
        WaitScreenHandler.RegisterEarlyLoadTask("CullFixedProps", ModifyPrefabs);

        // register harmony patches, if there are any
        Harmony.CreateAndPatchAll(Assembly, $"{PluginInfo.PLUGIN_GUID}");
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }

    private void ModifyPrefabs(WaitScreenHandler.WaitScreenTask task)
    {
        StartCoroutine(MaterialCullFix.RegisterAsync());
    }
}
