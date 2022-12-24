using IPA;
using IPA.Config;
using IPA.Config.Stores;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using IPALogger = IPA.Logging.Logger;

namespace HeadPositionViewer
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        // TODO: Harmony を使用している場合、コメントを解除して YourGitHub を GitHub アカウントの名前に変更するか、"com.company.project.product" という形式を使用します。
        //       また、Libs フォルダに Harmony アセンブリへの参照を追加する必要があります。
        // public const string HarmonyId = "com.github.YourGitHub.HeadPositionViewer";
        // internal static readonly HarmonyLib.Harmony harmony = new HarmonyLib.Harmony(HarmonyId);

        internal static Plugin Instance { get; private set; }
        internal static IPALogger Log { get; private set; }
        internal static HeadPositionViewerController PluginController { get { return HeadPositionViewerController.Instance; } }

        [Init]
        /// <summary>
        /// IPAによってプラグインが最初にロードされたときに呼び出される（ゲームが開始されたとき、またはプラグインが無効な状態で開始された場合は有効化されたときのいずれか）。
        /// [Init]コンストラクタを使用するメソッドや、InitWithConfigなどの通常のメソッドの前に呼び出されるメソッド。
        /// [Init]は1つのコンストラクタにのみ使用してください。
        /// </summary>
        public Plugin(IPALogger logger)
        {
            Instance = this;
            Plugin.Log = logger;
            Plugin.Log?.Debug("Logger initialized.");
        }

        #region BSIPA Config
        //BSIPA の設定を使用する場合はコメントを外してください。
        /*
        [Init]
        public void InitWithConfig(Config conf)
        {
            Configuration.PluginConfig.Instance = conf.Generated<Configuration.PluginConfig>();
            Plugin.Log?.Debug("Config loaded");
        }
        */
        #endregion


        #region Disableable

        /// <summary>
        /// プラグインが有効な場合（プラグインが有効な場合はゲーム開始時も含む）に呼び出されます。
        /// </summary>
        [OnEnable]
        public void OnEnable()
        {
            new GameObject("HeadPositionViewerController").AddComponent<HeadPositionViewerController>();
            //ApplyHarmonyPatches();
        }

        /// <summary>
        /// プラグインを無効にしたときと、Beat Saberの終了時に呼び出される。ここで、Harmonyパッチ、GameObject、Monobehavioursをクリーンアップすることが重要です。
        /// プラグインを起動しない状態にしておく。
        /// [OnDisable]の付いたメソッドは、voidまたはTaskを返す必要があります。
        /// </summary>
        [OnDisable]
        public void OnDisable()
        {
            if (PluginController != null)
                GameObject.Destroy(PluginController);
            //RemoveHarmonyPatches();
        }

        /*
        /// <summary>
        /// プラグインが無効のとき、およびBeat Saberの終了時に呼び出されます。
        /// プラグインが無効化するために長時間実行する非同期作業が必要な場合に、タスクを返します。
        /// Task を返す[OnDisable]メソッドは、void を返すすべての[OnDisable]メソッドの後に呼び出されます。
        /// </summary>
        [OnDisable]
        public async Task OnDisableAsync()
        {
            await LongRunningUnloadTask().ConfigureAwait(false);
        }
        */
        #endregion

        // Harmonyを使用する場合は、このセクションのメソッドのコメントを外します。
        #region Harmony
        /*
        /// <summary>
        /// このアセンブリに含まれるすべてのHarmonyパッチを適用しようとします。
        /// </summary>
        internal static void ApplyHarmonyPatches()
        {
            try
            {
                Plugin.Log?.Debug("Applying Harmony patches.");
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                Plugin.Log?.Error("Error applying Harmony patches: " + ex.Message);
                Plugin.Log?.Debug(ex);
            }
        }

        /// <summary>
        /// HarmonyIdを使用したすべてのHarmonyパッチの削除を試みます。
        /// </summary>
        internal static void RemoveHarmonyPatches()
        {
            try
            {
                // この HarmonyId を持つすべてのパッチを削除する。
                harmony.UnpatchAll(HarmonyId);
            }
            catch (Exception ex)
            {
                Plugin.Log?.Error("Error removing Harmony patches: " + ex.Message);
                Plugin.Log?.Debug(ex);
            }
        }
        */
        #endregion
    }
}
