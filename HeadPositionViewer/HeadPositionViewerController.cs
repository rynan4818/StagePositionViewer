using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HeadPositionViewer
{
    /// <summary>
    /// GameObjectにMonobehaviours（スクリプト）を追加。
    /// Monobehaviourがゲームから受け取ることのできるメッセージの一覧は、https://docs.unity3d.com/ScriptReference/MonoBehaviour.html を参照してください。
    /// </summary>
    public class HeadPositionViewerController : MonoBehaviour
    {
        public static HeadPositionViewerController Instance { get; private set; }

        // これらのメソッドはUnityから自動的に呼び出されますので、使用しないものは削除してください。
        #region Monobehaviour Messages
        /// <summary>
        /// 一度だけ呼ばれ、主に変数の初期化に使用されます。
        /// </summary>
        private void Awake()
        {
            // このMonoBehaviourでは、常に1つのインスタンスしか存在させたくないので、staticプロパティにその参照を保存し、
            // 1つが既に存在している間に生成されたものは破棄します。
            if (Instance != null)
            {
                Plugin.Log?.Warn($"Instance of {GetType().Name} already exists, destroying.");
                GameObject.DestroyImmediate(this);
                return;
            }
            GameObject.DontDestroyOnLoad(this); // シーンチェンジ時にこのオブジェクトを破棄しない
            Instance = this;
            Plugin.Log?.Debug($"{name}: Awake()");
        }
        /// <summary>
        /// スクリプトがEnabledになった最初のフレームで一度だけ呼び出されます。Start は、他のスクリプトの Awake() の後、Update() の前に呼び出されます。
        /// </summary>
        private void Start()
        {

        }

        /// <summary>
        /// スクリプトが有効な場合、毎フレーム呼び出される。
        /// </summary>
        private void Update()
        {

        }

        /// <summary>
        /// 他の有効なスクリプトのUpdate()の後に毎フレーム呼び出される。
        /// </summary>
        private void LateUpdate()
        {

        }

        /// <summary>
        /// スクリプトが有効かつアクティブになったときに呼び出される
        /// </summary>
        private void OnEnable()
        {

        }

        /// <summary>
        /// スクリプトが無効になったとき、または破棄されるときに呼び出される。
        /// </summary>
        private void OnDisable()
        {

        }

        /// <summary>
        /// スクリプトが破棄されるときに呼び出される。
        /// </summary>
        private void OnDestroy()
        {
            Plugin.Log?.Debug($"{name}: OnDestroy()");
            if (Instance == this)
                Instance = null; // このMonoBehaviourは破棄されるため、staticインスタンスプロパティをnullに設定してください。

        }
        #endregion
    }
}
