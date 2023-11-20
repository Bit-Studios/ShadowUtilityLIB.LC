using System.Collections.Concurrent;
using System.Linq;
using ZstdNet;

namespace ShadowUtilityLIB.logging
{
    public static class LogManager
    {       
        public static string FileLocation = "./log.sl";//.sl Shadow log, .slc shadow log compressed

        public static ConcurrentBag<string> Log = new ConcurrentBag<string>();

        private static Logger logger = new Logger("Shadow Lib Log Manager", "1.0.0");
        public static void StartLogManager()
        {
            try
            {
                if (Directory.Exists("./logs")) { }
                else
                {
                    Directory.CreateDirectory("./logs");
                }
                foreach (string logFileLocation in Directory.GetFiles("./", "*.sl"))
                {
                    File.Move($"./{logFileLocation}", $"./logs/{logFileLocation}{DateTime.Now.ToFileTimeUtc().ToString()}.sl");
                    logger.Debug(logFileLocation);
                }
            }
            catch (Exception e)
            {
                logger.Error($"{e}\n{e.Message}\n{e.InnerException}\n{e.Source}\n{e.Data}\n{e.HelpLink}\n{e.HResult}\n{e.StackTrace}\n{e.TargetSite}\n{e.GetBaseException()}");
            }
            new Thread(new ThreadStart(() => {
                while (true)
                {
                    Thread.Sleep(1000);
                    if(Log.Count > 0)
                    {
                        File.AppendAllText(FileLocation, string.Join("", Log.ToList<string>()));
                        Log = new ConcurrentBag<string>();
                    }
                }
            })).Start();
            try
            {
                string[] files = Directory.GetFiles("./logs", "*.sl");
                string[] cfiles = Directory.GetFiles("./logs", "*.slc");
                using var options = new CompressionOptions(compressionLevel: 22);
                using var compressor = new Compressor(options);

                foreach (var uncompressedLog in files)
                {
                    logger.Log(uncompressedLog);
                    var compressedData = compressor.Wrap(File.ReadAllBytes(uncompressedLog));
                    File.WriteAllBytes(uncompressedLog + "c", compressedData);
                    File.Delete(uncompressedLog);
                }
                if (cfiles.Length > 10)
                {
                    if (File.Exists("./logs.slcf"))
                    {
                        File.Delete("./logs.slcf");
                    }
                    foreach (var file in cfiles)
                    {
                        logger.Log(file);
                        var compressedData = File.ReadAllText(file);
                        
                        //File.AppendAllText("./logs.slcf", compressedData + "/@.@/"); Just delete the old logs
                        File.Delete(file);
                    }

                }
            }
            catch (Exception e)
            {
                logger.Error($"{e}\n{e.Message}\n{e.InnerException}\n{e.Source}\n{e.Data}\n{e.HelpLink}\n{e.HResult}\n{e.StackTrace}\n{e.TargetSite}\n{e.GetBaseException()}");
            }
            
        }
        
    }
    public class Logger
    {
        public string ModName { get; set; }
        public string ModVersion { get; set; }
        public Logger(string ModName,string ModVersion) {
            this.ModName = ModName;
            this.ModVersion = ModVersion;
        }
        public void Log(string logmessage)
        {
            Console.WriteLine($"[Info] [{DateTime.UtcNow}] [{ModName}] [{ModVersion}] {logmessage}\n");
            LogManager.Log.Add($"[Info] [{DateTime.UtcNow}] [{ModName}] [{ModVersion}] {logmessage}\n");
        }
        public void Error(string logmessage)
        {
            Console.WriteLine($"[Error] [{DateTime.UtcNow}] [{ModName}] [{ModVersion}] {logmessage}\n");
            LogManager.Log.Add($"[Error] [{DateTime.UtcNow}] [{ModName}] [{ModVersion}] {logmessage}\n");
        }
        public void Debug(string logmessage)
        {
            if (ShadowLIB.IsDev)
            {
                Console.WriteLine($"[Debug] [{DateTime.UtcNow}] [{ModName}] [{ModVersion}] {logmessage}\n");
                LogManager.Log.Add($"[Debug] [{DateTime.UtcNow}] [{ModName}] [{ModVersion}] {logmessage}\n");
            }
        }
    }
}
