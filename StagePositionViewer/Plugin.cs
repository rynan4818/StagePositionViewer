using StagePositionViewer.Configuration;
using StagePositionViewer.Installers;
using HarmonyLib;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using SiraUtil.Zenject;
using IPALogger = IPA.Logging.Logger;
using System.Reflection;

namespace StagePositionViewer
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        internal static Plugin Instance { get; private set; }
        internal static IPALogger Log { get; private set; }

        private Harmony _harmony;
        public const string HARMONY_ID = "com.github.rynan4818.StagePositionViewer";

        /// <summary>
        /// IPAによってプラグインが最初にロードされたときに呼び出される（ゲームが開始されたとき、またはプラグインが無効な状態で開始された場合は有効化されたときのいずれか）。
        /// [Init]コンストラクタを使用するメソッドや、InitWithConfigなどの通常のメソッドの前に呼び出されるメソッド。
        /// [Init]は1つのコンストラクタにのみ使用してください。
        /// </summary>
        [Init]
        public void Init(IPALogger logger, Config conf, Zenjector zenjector)
        {
            Instance = this;
            Log = logger;
            Log?.Debug("Logger initialized.");
            this._harmony = new Harmony(HARMONY_ID);
            PluginConfig.Instance = conf.Generated<PluginConfig>();
            Log?.Debug("Config loaded");
            zenjector.Install<StagePositionPlayerInstaller>(Location.Player);
            zenjector.Install<StagePositionMenuInstaller>(Location.Menu);
        }
        [OnStart]
        public void OnApplicationStart()
        {
            Log?.Info("OnApplicationStart");
            this._harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        [OnExit]
        public void OnApplicationQuit()
        {
            this._harmony.UnpatchSelf();
            Log?.Debug("OnApplicationQuit");
        }
    }
}
