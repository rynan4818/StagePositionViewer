using HeadPositionViewer.Configuration;
using UnityEngine;
using UnityEngine.XR;
using Zenject;

namespace HeadPositionViewer.Models
{
    /// デンパ時計さんの、HeadDistanceTravelledを参考にしてます。
    /// 参考元ソース:https://github.com/denpadokei/HeadDistanceTravelled/blob/main/HeadDistanceTravelled/HeadDistanceTravelledController.cs
    /// ライセンス:https://github.com/denpadokei/HeadDistanceTravelled/blob/main/LICENSE

    /// <summary>
    /// GameObjectにMonobehaviours（スクリプト）を追加。
    /// Monobehaviourがゲームから受け取ることのできるメッセージの一覧は、https://docs.unity3d.com/ScriptReference/MonoBehaviour.html を参照してください。
    /// </summary>
    public class HeadPositionController : MonoBehaviour
    {
        public Vector3 _prevHMDPosition = Vector3.zero;
        public Vector3 _hmdPosition = Vector3.zero;

        private IVRPlatformHelper _platformHelper;

        [Inject]
        public void Constractor(IVRPlatformHelper helper)
        {
            this._platformHelper = helper;
        }
        public void Start()
        {
            this._platformHelper.GetNodePose(XRNode.Head, 0, out this._prevHMDPosition, out _);
        }
        public void Update()
        {
            this._platformHelper.GetNodePose(XRNode.Head, 0, out this._hmdPosition, out _);
            var distance = Vector3.Distance(this._hmdPosition, this._prevHMDPosition);
            if (PluginConfig.Instance.movementSensitivityThreshold < distance)
            {
                this._prevHMDPosition = this._hmdPosition;
            }
        }
    }
}
