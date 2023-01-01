using HeadPositionViewer.Configuration;
using HeadPositionViewer.Installers;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using SiraUtil.Zenject;
using IPALogger = IPA.Logging.Logger;

namespace HeadPositionViewer
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        internal static Plugin Instance { get; private set; }
        internal static IPALogger Log { get; private set; }

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
            PluginConfig.Instance = conf.Generated<PluginConfig>();
            Log?.Debug("Config loaded");
            zenjector.Install<HeadPositionPlayerInstaller>(Location.Player);
            zenjector.Install<HeadPositionMenuInstaller>(Location.Menu);
        }
        [OnStart]
        public void OnApplicationStart()
        {
            Log?.Info("OnApplicationStart");
        }

        [OnExit]
        public void OnApplicationQuit()
        {
            Log?.Debug("OnApplicationQuit");
        }
    }
}
