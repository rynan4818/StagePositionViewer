using HeadPositionViewer.Configuration;
using HeadPositionViewer.Views;
using System;
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
        private HeadPositionUI _headPositionUI;

        [Inject]
        public void Constractor(IVRPlatformHelper helper, HeadPositionUI headPositionUI)
        {
            this._platformHelper = helper;
            this._headPositionUI= headPositionUI;
        }
        public void Start()
        {
            this._platformHelper.GetNodePose(XRNode.Head, 0, out this._prevHMDPosition, out _);
        }
        public void Update()
        {
            this._platformHelper.GetNodePose(XRNode.Head, 0, out this._hmdPosition, out _);
            var distance = Vector3.Distance(this._hmdPosition, this._prevHMDPosition);
            if (Math.Abs(this._hmdPosition.x) <= PluginConfig.Instance.CenterMovementSensitivityDistance || Math.Abs(this._hmdPosition.z) <= PluginConfig.Instance.CenterMovementSensitivityDistance)
            {
                if (PluginConfig.Instance.CenterMovementSensitivityThreshold >= distance)
                    return;
            }
            else
            {
                if (PluginConfig.Instance.MovementSensitivityThreshold >= distance)
                    return;
            }
            _headPositionUI.HeadMarkMove(this._hmdPosition);
            this._prevHMDPosition = this._hmdPosition;
        }
    }
}
