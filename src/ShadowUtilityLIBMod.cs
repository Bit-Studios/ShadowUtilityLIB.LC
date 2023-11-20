using Logger = ShadowUtilityLIB.logging.Logger;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using ShadowUtilityLIB.UI;
using HarmonyLib;
using UnityEngine.UIElements;
using BepInEx;
using ShadowUtilityLIB.logging;
using System.Reflection;

namespace ShadowUtilityLIB;
public class CoroutineExecuter : MonoBehaviour { 
    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}

[BepInPlugin("com.shadowdev.utilitylib", "shadowutilitylib", ShadowLIB.ModVersion)]
public sealed class ShadowUtilityLIBMod : BaseUnityPlugin
{
    public const string ModId = ShadowLIB.ModId;
    public const string ModVersion = ShadowLIB.ModVersion;
    public static bool IsDev = ShadowLIB.IsDev;
    public static bool Initilised = ShadowLIB.Initilised;
    void Awake()
    {
        ShadowLIB.SoftDependancys["BepInEx"] = true;
        ShadowLIB.Awake();
        
    }

    //compatibility with older versions
    public static void EnableDebugMode()
    {
        ShadowLIB.EnableDebugMode();
    }
    public static void RunCr(IEnumerator cr)
    {
        ShadowLIB.RunCr(cr);
    }
}


//universal
public static class ShadowLIB
{
    public const string ModId = "com.shadowdev.utilitylib";
    public const string ModVersion = "0.0.9";
    public static bool IsDev = false;
    private static Logger logger = new Logger(ModId, ModVersion);
    private static CoroutineExecuter instance;
    public static bool Initilised = false;
    public static Dictionary<string, bool> SoftDependancys = new Dictionary<string, bool>() {{ "BepInEx", false } };
    public static bool CRE = false;
    public static void Awake()
    {
        EnableDebugMode();
        try
        {
            if (!CRE)
            {
                CRE = true;
                instance = new GameObject("CoroutineExecuter").AddComponent<CoroutineExecuter>();
            }
            
        }
        catch (Exception e)
        {
            logger.Error($"{e}\n{e.Message}\n{e.InnerException}\n{e.Source}\n{e.Data}\n{e.HelpLink}\n{e.HResult}\n{e.StackTrace}\n{e.TargetSite}\n{e.GetBaseException()}");
        }
        LogManager.StartLogManager();
        AssetManager.LoadAllAssets();
        try
        {
            Harmony.CreateAndPatchAll(typeof(DropdownUtils));
        }
        catch (Exception e)
        {
            logger.Error($"{e}\n{e.Message}\n{e.InnerException}\n{e.Source}\n{e.Data}\n{e.HelpLink}\n{e.HResult}\n{e.StackTrace}\n{e.TargetSite}\n{e.GetBaseException()}");
        }
        
        

        logger.Log($"Initialized");
        Initilised = true;
    }
    public static void EnableDebugMode()
    {
        IsDev = true;
        logger.Debug("Debug Mode Enabled");
    }
    public static void RunCr(IEnumerator cr)
    {
        try
        {
            instance.StartCoroutine(cr);
        }
        catch (Exception e)
        {
            logger.Error($"{e}\n{e.Message}\n{e.InnerException}\n{e.Source}\n{e.Data}\n{e.HelpLink}\n{e.HResult}\n{e.StackTrace}\n{e.TargetSite}\n{e.GetBaseException()}");
            if (!CRE)
            {
                CRE = true;
                instance = new GameObject("CoroutineExecuter").AddComponent<CoroutineExecuter>();
                RunCr(cr);
            }
        }

    }
    public static bool NamespaceExists(string desiredNamespace)//https://forum.unity.com/threads/run-bit-of-code-if-namespace-exists-c.437745/
    {
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.Namespace == desiredNamespace)
                    return true;
            }
        }
        return false;
    }
}