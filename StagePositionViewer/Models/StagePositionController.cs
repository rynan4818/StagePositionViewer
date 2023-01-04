using StagePositionViewer.Configuration;
using StagePositionViewer.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;
using Zenject;

namespace StagePositionViewer.Models
{
    /// デンパ時計さんの、HeadDistanceTravelledを参考にしてます。
    /// 参考元ソース:https://github.com/denpadokei/HeadDistanceTravelled/blob/main/HeadDistanceTravelled/HeadDistanceTravelledController.cs
    /// ライセンス:https://github.com/denpadokei/HeadDistanceTravelled/blob/main/LICENSE

    /// <summary>
    /// GameObjectにMonobehaviours（スクリプト）を追加。
    /// Monobehaviourがゲームから受け取ることのできるメッセージの一覧は、https://docs.unity3d.com/ScriptReference/MonoBehaviour.html を参照してください。
    /// </summary>
    public class StagePositionController : MonoBehaviour
    {
        public Vector3 _prevHMDPosition = Vector3.zero;
        public Vector3 _hmdPosition = Vector3.zero;
        public Vector3 _target1Position = Vector3.zero;
        public Vector3 _target2Position = Vector3.zero;

        public List<InputDevice> _trackedDevices { get; private set; } = new List<InputDevice>();
        public InputDevice _targetDevice1;
        public InputDevice _targetDevice2;
        public bool _customDevice = false;
        public bool _customDeviceEnable = false;

        private IVRPlatformHelper _platformHelper;
        private StagePositionUI _stagePositionUI;

        [Inject]
        public void Constractor(IVRPlatformHelper helper, StagePositionUI stagePositionPositionUI)
        {
            this._platformHelper = helper;
            this._stagePositionUI= stagePositionPositionUI;
        }
        public void Start()
        {
            if (!PluginConfig.Instance.Enable)
                return;
            this._platformHelper.GetNodePose(XRNode.Head, 0, out this._prevHMDPosition, out _);
            var desiredCharacteristics = InputDeviceCharacteristics.TrackedDevice;
            InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, _trackedDevices);
            _targetDevice1 = _trackedDevices.FirstOrDefault(i => i.serialNumber == PluginConfig.Instance.TargetDevice1);
            _targetDevice2 = _trackedDevices.FirstOrDefault(i => i.serialNumber == PluginConfig.Instance.TargetDevice2);
            if (PluginConfig.Instance.CustomTargetDevice && (_targetDevice1 != null || _targetDevice2 != null))
                _customDevice = true;
        }
        public void Update()
        {
            if (!PluginConfig.Instance.Enable)
                return;
            if (_customDevice)
            {
                _customDeviceEnable = true;
                var target1 = _targetDevice1 != null && _targetDevice1.TryGetFeatureValue(CommonUsages.devicePosition, out _target1Position);
                var target2 = _targetDevice2 != null && _targetDevice2.TryGetFeatureValue(CommonUsages.devicePosition, out _target2Position);
                if (target1 && target2)
                    this._hmdPosition = (_target1Position + _target2Position) * 0.5f + new Vector3(PluginConfig.Instance.TargetDeviceOffsetX, 0, PluginConfig.Instance.TargetDeviceOffsetZ);
                else if (target1)
                    this._hmdPosition = _target1Position + new Vector3(PluginConfig.Instance.TargetDeviceOffsetX, 0, PluginConfig.Instance.TargetDeviceOffsetZ);
                else if (target2)
                    this._hmdPosition = _target2Position + new Vector3(PluginConfig.Instance.TargetDeviceOffsetX, 0, PluginConfig.Instance.TargetDeviceOffsetZ);
                else
                {
                    this._platformHelper.GetNodePose(XRNode.Head, 0, out this._hmdPosition, out _);
                    _customDeviceEnable = false;
                }
            }
            else
                this._platformHelper.GetNodePose(XRNode.Head, 0, out this._hmdPosition, out _);
            var distance = Vector3.Distance(this._hmdPosition, this._prevHMDPosition);
            if (PluginConfig.Instance.CenterSignal && (Math.Abs(this._hmdPosition.x) <= PluginConfig.Instance.CenterMovementSensitivityDistance || Math.Abs(this._hmdPosition.z) <= PluginConfig.Instance.CenterMovementSensitivityDistance))
            {
                if (PluginConfig.Instance.CenterMovementSensitivityThreshold >= distance)
                    return;
            }
            else
            {
                if (PluginConfig.Instance.MovementSensitivityThreshold >= distance)
                    return;
            }
            _stagePositionUI.PlayerMarkMove(this._hmdPosition, _customDeviceEnable);
            this._prevHMDPosition = this._hmdPosition;
        }
    }
}
