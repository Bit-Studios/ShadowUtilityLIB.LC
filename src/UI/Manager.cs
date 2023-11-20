using UnityEngine.UIElements;
using Logger = ShadowUtilityLIB.logging.Logger;
using HarmonyLib;
using static UnityEngine.UIElements.GenericDropdownMenu;

namespace ShadowUtilityLIB.UI
{
    public class Manager
    {
        private static Logger logger = new Logger(ShadowUtilityLIBMod.ModId, ShadowLIB.ModVersion);
        public Dictionary<string, UIDocument> Windows { get; set; }
        public int WidthScaleLimit = 1920;
        public int HeightScaleLimit = 1080;
        public static PanelSettings PanelSettings { get; internal set; }
        public Manager()
        {
            try
            {
                Windows = new Dictionary<string, UIDocument>();
            }
            catch (Exception e)
            {
                logger.Error($"{e.Message}\n{e.InnerException}\n{e.Source}\n{e.Data}\n{e.HelpLink}\n{e.HResult}\n{e.StackTrace}\n{e.TargetSite}");
            }
        }
        public void Add(string name, UIDocument document)
        {
            Add(name, document, true);
        }
        public void Add(string name, UIDocument document,bool noScale = true)
        {
            try
            {
                if (noScale)
                {
                    document.panelSettings = PanelSettings;
                }
                Windows.Add(name, document);
            }
            catch (Exception e)
            {
                logger.Error($"{e.Message}\n{e.InnerException}\n{e.Source}\n{e.Data}\n{e.HelpLink}\n{e.HResult}\n{e.StackTrace}\n{e.TargetSite}");
            }
        }
        public void Remove(string name)
        {
            try
            {
                Windows.Remove(name);
            }
            catch (Exception e)
            {
                logger.Error($"{e.Message}\n{e.InnerException}\n{e.Source}\n{e.Data}\n{e.HelpLink}\n{e.HResult}\n{e.StackTrace}\n{e.TargetSite}");
            }
        }
        public UIDocument Get(string name)
        {
            return Windows[name];
        }
        public void Toggle(string name)
        {
            try
            {
                Windows[name].rootVisualElement.visible = !Windows[name].rootVisualElement.visible;
            }
            catch (Exception e)
            {
                logger.Error($"{e.Message}\n{e.InnerException}\n{e.Source}\n{e.Data}\n{e.HelpLink}\n{e.HResult}\n{e.StackTrace}\n{e.TargetSite}");
            }
        }
        public void Set(string name,bool state)
        {
            try
            {
                Windows[name].rootVisualElement.visible = state;
            }
            catch (Exception e)
            {
                logger.Error($"{e.Message}\n{e.InnerException}\n{e.Source}\n{e.Data}\n{e.HelpLink}\n{e.HResult}\n{e.StackTrace}\n{e.TargetSite}");
            }
        }
    }
    public static class DropdownUtils
    {
        public static Dictionary<string,float> LabelsToChange = new Dictionary<string,float>();
        private static Logger logger = new Logger(ShadowUtilityLIBMod.ModId, "");

        [HarmonyPatch(typeof(GenericDropdownMenu))]
        [HarmonyPatch("AddItem", new Type[] { typeof(string), typeof(bool), typeof(bool), typeof(object) })]
        [HarmonyPostfix]
        public static void GenericDropdownMenu_AddItem(GenericDropdownMenu __instance, ref MenuItem __result, ref string itemName, ref bool isChecked,ref bool isEnabled,ref object data)
        {
            try{

                if (LabelsToChange.ContainsKey(itemName))
                {
                    Label labelEl = __result.element.Q<Label>();
                    labelEl.style.fontSize = LabelsToChange[itemName];
                }
            }
            catch (Exception e)
            {
                logger.Error($"{e.Message}\n{e.InnerException}\n{e.Source}\n{e.Data}\n{e.HelpLink}\n{e.HResult}\n{e.StackTrace}\n{e.TargetSite}");
            }

        }
        public static void SetLabel(string labelName, float fontsize)
        {
            if (LabelsToChange.ContainsKey(labelName))
            {
                LabelsToChange[labelName] = fontsize;
            }
            else
            {
                LabelsToChange.Add(labelName, fontsize);
            }
        }
        public static void RemoveLabel(string labelName)
        {
            if (LabelsToChange.ContainsKey(labelName))
            {
                LabelsToChange.Remove(labelName);
            }
        }
    }
}
