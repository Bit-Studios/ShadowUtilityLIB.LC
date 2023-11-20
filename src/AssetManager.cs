using Logger = ShadowUtilityLIB.logging.Logger;
using UnityEngine;

namespace ShadowUtilityLIB
{
    public static class AssetManager
    {
        private static Logger logger = new Logger(ShadowLIB.ModId, ShadowLIB.ModVersion);
        private static Dictionary<string,Texture2D> texture2Ds = new Dictionary<string, Texture2D>();
        public static Texture2D GetAsset(string ID)
        {
            Texture2D AssetData = texture2Ds[ID];
            return AssetData;
        }
        public static void SetAsset(string ID, Texture2D AssetData)
        {
            texture2Ds[ID] = AssetData;
        }
        public static void LoadAsset(string ID, string Path)
        {
            try
            {


                if (Path.EndsWith(".png") || Path.EndsWith(".jpg"))
                {
                    System.Drawing.Image img = System.Drawing.Image.FromFile(Path);

                    try{
                        texture2Ds.Remove(ID);
                    } catch(Exception e){}
                        
                    
                    texture2Ds.Add(ID, new Texture2D(img.Size.Width, img.Size.Height));

                    texture2Ds[ID].LoadImage(File.ReadAllBytes(Path));

                    texture2Ds[ID].name = ID;
                }
            }
            catch (Exception e)
            {
                logger.Error($"{e}\n{e.Message}\n{e.InnerException}\n{e.Source}\n{e.Data}\n{e.HelpLink}\n{e.HResult}\n{e.StackTrace}\n{e.TargetSite}\n{e.GetBaseException()}");
            }

        }
        public static void LoadAllAssets()
        {
            try
            {
                List<string> DirectoriesToCheck = new List<string>();
                List<string> ModPathsToCheck = new List<string>();
                void LoadDirectory(string path)
                {
                    logger.Debug($"{path}");
                    foreach (var DirPath in Directory.GetDirectories(path))
                    {
                        DirectoriesToCheck.Add($"{DirPath}");
                        logger.Debug($"{DirPath}");
                    };
                }
                void LoadModPath(string path)
                {
                    logger.Debug($"{path}");
                    foreach (var ModPath in Directory.GetDirectories(path))
                    {
                        ModPathsToCheck.Add($"{ModPath}");
                        logger.Debug($"{ModPath}");
                    };
                }
                void LoadAssetsInFolder(string path)
                {
                    if (path.ToLower().EndsWith("assets"))
                    {
                        logger.Debug($"assets {path}");
                        if (Directory.GetDirectories(path).Contains($"{path}\\images"))//Image Assets convert to Texture2D
                        {
                            foreach (var file in Directory.GetFiles($"{path}\\images"))
                            {
                                logger.Debug($"{file}");
                                LoadAsset(Path.GetFileName(file), $"{file}");
                            }
                        }
                    }
                }
                if (Directory.Exists("./BepInEx/plugins"))
                {
                    LoadDirectory("./BepInEx/plugins");
                }
                foreach (string path in DirectoriesToCheck)
                {
                    LoadModPath(path);
                }
                foreach (string path in ModPathsToCheck)
                {
                    LoadAssetsInFolder(path);
                }
            }
            catch (Exception e)
            {
                logger.Error($"{e}\n{e.Message}\n{e.InnerException}\n{e.Source}\n{e.Data}\n{e.HelpLink}\n{e.HResult}\n{e.StackTrace}\n{e.TargetSite}\n{e.GetBaseException()}");
            }
        }
    }
}
