using StagePositionViewer.Configuration;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.GameplaySetup;
using BeatSaberMarkupLanguage.ViewControllers;
using System.Globalization;
using Zenject;
using TMPro;
using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;
using HMUI;
using System.Linq;
using System;
using UnityEngine.UI;

namespace StagePositionViewer.Views
{
    [HotReload]
    internal class SettingTabViewController : BSMLAutomaticViewController, IInitializable
    {
        public const string TabName = "Stage Position Viewer";
        public string ResourceName => string.Join(".", GetType().Namespace, GetType().Name);
        public List<InputDevice> _trackedDevices { get; private set; } = new List<InputDevice>();

        public List<string> _targetDevices;

        [UIComponent("position")]
        private readonly TextMeshProUGUI _position;

        [UIComponent("TrackedDevicePosition")]
        private readonly TextMeshProUGUI _trackedDevicePosition;

        [UIValue("Enable")]
        public bool Enable
        {
            get => PluginConfig.Instance.Enable;
            set
            {
                PluginConfig.Instance.Enable = value;
            }
        }
        [UIValue("LockPosition")]
        public bool LockPosition
        {
            get => PluginConfig.Instance.LockPosition;
            set
            {
                PluginConfig.Instance.LockPosition = value;
            }
        }
        [UIValue("ScreenPosX")]
        public float ScreenPosX
        {
            get => PluginConfig.Instance.ScreenPosX;
            set
            {
                if (PluginConfig.Instance.ScreenPosX.Equals(value))
                    return;
                PluginConfig.Instance.ScreenPosX = value;
                PositionUpdate();
                NotifyPropertyChanged();
            }
        }
        [UIValue("ScreenPosY")]
        public float ScreenPosY
        {
            get => PluginConfig.Instance.ScreenPosY;
            set
            {
                if (PluginConfig.Instance.ScreenPosY.Equals(value))
                    return;
                PluginConfig.Instance.ScreenPosY = value;
                PositionUpdate();
                NotifyPropertyChanged();
            }
        }
        [UIValue("ScreenPosZ")]
        public float ScreenPosZ
        {
            get => PluginConfig.Instance.ScreenPosZ;
            set
            {
                if (PluginConfig.Instance.ScreenPosZ.Equals(value))
                    return;
                PluginConfig.Instance.ScreenPosZ = value;
                PositionUpdate();
                NotifyPropertyChanged();
            }
        }
        [UIValue("ScreenRotX")]
        public float ScreenRotX
        {
            get => PluginConfig.Instance.ScreenRotX;
            set
            {
                if (PluginConfig.Instance.ScreenRotX.Equals(value))
                    return;
                PluginConfig.Instance.ScreenRotX = value;
                PositionUpdate();
                NotifyPropertyChanged();
            }
        }
        [UIValue("ScreenRotY")]
        public float ScreenRotY
        {
            get => PluginConfig.Instance.ScreenRotY;
            set
            {
                if (PluginConfig.Instance.ScreenRotY.Equals(value))
                    return;
                PluginConfig.Instance.ScreenRotY = value;
                PositionUpdate();
                NotifyPropertyChanged();
            }
        }
        [UIValue("ScreenRotZ")]
        public float ScreenRotZ
        {
            get => PluginConfig.Instance.ScreenRotZ;
            set
            {
                if (PluginConfig.Instance.ScreenRotZ.Equals(value))
                    return;
                PluginConfig.Instance.ScreenRotZ = value;
                PositionUpdate();
                NotifyPropertyChanged();
            }
        }
        [UIValue("HMDOnly")]
        public bool HMDOnly
        {
            get => PluginConfig.Instance.HMDOnly;
            set
            {
                PluginConfig.Instance.HMDOnly = value;
            }
        }
        [UIValue("PositionValueView")]
        public bool PositionValueView
        {
            get => PluginConfig.Instance.PositionValueView;
            set
            {
                PluginConfig.Instance.PositionValueView = value;
            }
        }
        [UIValue("CenterSignal")]
        public bool CenterSignal
        {
            get => PluginConfig.Instance.CenterSignal;
            set
            {
                PluginConfig.Instance.CenterSignal = value;
            }
        }
        [UIValue("ScreenSize")]
        public float ScreenSize
        {
            get => PluginConfig.Instance.ScreenSize;
            set
            {
                PluginConfig.Instance.ScreenSize = value;
            }
        }
        [UIValue("FrontLimitLine")]
        public float FrontLimitLine
        {
            get => PluginConfig.Instance.FrontLimitLine;
            set
            {
                PluginConfig.Instance.FrontLimitLine = value;
            }
        }
        [UIValue("BackLimitLine")]
        public float BackLimitLine
        {
            get => PluginConfig.Instance.BackLimitLine;
            set
            {
                PluginConfig.Instance.BackLimitLine = value;
            }
        }
        [UIValue("RightLimitLine")]
        public float RightLimitLine
        {
            get => PluginConfig.Instance.RightLimitLine;
            set
            {
                PluginConfig.Instance.RightLimitLine = value;
            }
        }
        [UIValue("LeftLimitLine")]
        public float LeftLimitLine
        {
            get => PluginConfig.Instance.LeftLimitLine;
            set
            {
                PluginConfig.Instance.LeftLimitLine = value;
            }
        }
        [UIValue("WarningPercentage1")]
        public float WarningPercentage1
        {
            get => PluginConfig.Instance.WarningPercentage1;
            set
            {
                PluginConfig.Instance.WarningPercentage1 = value;
            }
        }
        [UIValue("WarningPercentage2")]
        public float WarningPercentage2
        {
            get => PluginConfig.Instance.WarningPercentage2;
            set
            {
                PluginConfig.Instance.WarningPercentage2 = value;
            }
        }
        [UIValue("CenterLimitX")]
        public float CenterLimitX
        {
            get => PluginConfig.Instance.CenterLimitX;
            set
            {
                PluginConfig.Instance.CenterLimitX = value;
            }
        }
        [UIValue("LineWidth")]
        public float LineWidth
        {
            get => PluginConfig.Instance.LineWidth;
            set
            {
                PluginConfig.Instance.LineWidth = value;
            }
        }
        [UIValue("MarkSize")]
        public float MarkSize
        {
            get => PluginConfig.Instance.MarkSize;
            set
            {
                PluginConfig.Instance.MarkSize = value;
            }
        }
        [UIValue("CustomTargetDevice")]
        public bool CustomTargetDevice
        {
            get => PluginConfig.Instance.CustomTargetDevice;
            set
            {
                PluginConfig.Instance.CustomTargetDevice = value;
            }
        }
        [UIValue("TargetDeviceOffsetX")]
        public float TargetDeviceOffsetX
        {
            get => PluginConfig.Instance.TargetDeviceOffsetX;
            set
            {
                PluginConfig.Instance.TargetDeviceOffsetX = value;
            }
        }
        [UIValue("TargetDeviceOffsetZ")]
        public float TargetDeviceOffsetZ
        {
            get => PluginConfig.Instance.TargetDeviceOffsetZ;
            set
            {
                PluginConfig.Instance.TargetDeviceOffsetZ = value;
            }
        }

        /// デンパ時計さんの、UltimateFireworksを参考にしてます。
        /// 参考元ソース:https://github.com/denpadokei/UltimateFireworks/blob/master/UltimateFireworks/Views/Setting.cs
        /// ライセンス:https://github.com/denpadokei/UltimateFireworks/blob/master/LICENSE
        [UIValue("targetDevice1-set")]
        public string TargetDevice1set
        {
            get => PluginConfig.Instance.TargetDevice1;
            set => PluginConfig.Instance.TargetDevice1 = value;
        }
        [UIValue("targetDevice1choices")]
        public List<object> TargetDevice1choices { get; set; } = new List<object>() { "NONE" };

        [UIComponent("targetDevice1-dropdown")]
        private readonly object _dropDownObject1;
        private SimpleTextDropdown _simpleTextDropdown1;

        [UIValue("targetDevice2-set")]
        public string TargetDevice2set
        {
            get => PluginConfig.Instance.TargetDevice2;
            set => PluginConfig.Instance.TargetDevice2 = value;
        }
        [UIValue("targetDevice2choices")]
        public List<object> TargetDevice2choices { get; set; } = new List<object>() { "NONE" };

        [UIComponent("targetDevice2-dropdown")]
        private readonly object _dropDownObject2;
        private SimpleTextDropdown _simpleTextDropdown2;

        [UIAction("IntFormatter")]
        private string IntFormatter(float value)
        {
            return $"{value.ToString("F0", CultureInfo.InvariantCulture)}";
        }
        [UIAction("OneDecimalFormatter")]
        private string OneDecimalFormatter(float value)
        {
            return $"{value.ToString("F1", CultureInfo.InvariantCulture)}";
        }
        [UIAction("ThirdDecimalFormatter")]
        private string ThirdDecimalFormatter(float value)
        {
            return $"{value.ToString("F3", CultureInfo.InvariantCulture)}";
        }
        [UIAction("SecondDecimalFormatter")]
        private string SecondDecimalFormatter(float value)
        {
            return $"{value.ToString("F2", CultureInfo.InvariantCulture)}";
        }
        [UIAction("PercentageFormatter")]
        private string PercentageFormatter(float value)
        {
            return $"{(value * 100f).ToString("F0", CultureInfo.InvariantCulture)}%";
        }

        [UIAction("ResetCenterPosition")]
        private void ResetCenterPosition()
        {
            this.ScreenPosX = 0;
        }
        [UIAction("ResetRotation")]
        private void ResetRotation()
        {
            this.ScreenRotX = 0;
            this.ScreenRotY = 0;
            this.ScreenRotZ = 0;
        }
        [UIAction("DefaultPosition")]
        private void DefaultPosition()
        {
            this.ScreenPosX = PluginConfig.DefaultScreenPosX;
            this.ScreenPosY = PluginConfig.DefaultScreenPosY;
            this.ScreenPosZ = PluginConfig.DefaultScreenPosZ;
            this.ScreenRotX = 0;
            this.ScreenRotY = 0;
            this.ScreenRotZ = 0;
        }
        [UIAction("LoadPosition")]
        private void LoadPosition()
        {
            this.ScreenPosX = PluginConfig.Instance.ScreenPosX;
            this.ScreenPosY = PluginConfig.Instance.ScreenPosY;
            this.ScreenPosZ = PluginConfig.Instance.ScreenPosZ;
            this.ScreenRotX = PluginConfig.Instance.ScreenRotX;
            this.ScreenRotY = PluginConfig.Instance.ScreenRotY;
            this.ScreenRotZ = PluginConfig.Instance.ScreenRotZ;
        }

        [UIAction("#post-parse")]
        private void PostParse()
        {
            PositionUpdate();
            TrackedDevicePositionGet();
            this._simpleTextDropdown1.didSelectCellWithIdxEvent += this.SimpleTextDropdown1_didSelectCellWithIdxEvent;
            this._simpleTextDropdown2.didSelectCellWithIdxEvent += this.SimpleTextDropdown2_didSelectCellWithIdxEvent;
        }
        public void PositionUpdate()
        {
            this._position.text = $"Screen  Pos X={OneDecimalFormatter(PluginConfig.Instance.ScreenPosX)}  Y={OneDecimalFormatter(PluginConfig.Instance.ScreenPosY)}  Z={OneDecimalFormatter(PluginConfig.Instance.ScreenPosZ)}    Rot  X={IntFormatter(PluginConfig.Instance.ScreenRotX)}  Y={IntFormatter(PluginConfig.Instance.ScreenRotY)}  Z={IntFormatter(PluginConfig.Instance.ScreenRotZ)}";
        }

        public void CreateTargetDeviceList()
        {
            try
            {
                _targetDevices = new List<string>
                {
                    "NONE"
                };
                foreach (var device in this._trackedDevices)
                {
                    _targetDevices.Add(device.serialNumber);
                }
                if (this._dropDownObject1 is LayoutElement layout1 && this._dropDownObject2 is LayoutElement layout2)
                {
                    this._simpleTextDropdown1 = layout1.GetComponentsInChildren<SimpleTextDropdown>(true).FirstOrDefault();
                    this._simpleTextDropdown2 = layout2.GetComponentsInChildren<SimpleTextDropdown>(true).FirstOrDefault();
                    this._simpleTextDropdown1.SetTexts(_targetDevices);
                    this._simpleTextDropdown2.SetTexts(_targetDevices);
                    this._simpleTextDropdown1.ReloadData();
                    this._simpleTextDropdown2.ReloadData();
                    if (string.IsNullOrEmpty(PluginConfig.Instance.TargetDevice1))
                    {
                        PluginConfig.Instance.TargetDevice1 = _targetDevices.FirstOrDefault() ?? "NONE";
                    }
                    else if (_targetDevices.Any(x => x == PluginConfig.Instance.TargetDevice1))
                    {
                        this._simpleTextDropdown1.SelectCellWithIdx(_targetDevices.IndexOf(PluginConfig.Instance.TargetDevice1));
                    }
                    if (string.IsNullOrEmpty(PluginConfig.Instance.TargetDevice2))
                    {
                        PluginConfig.Instance.TargetDevice2 = _targetDevices.FirstOrDefault() ?? "NONE";
                    }
                    else if (_targetDevices.Any(x => x == PluginConfig.Instance.TargetDevice2))
                    {
                        this._simpleTextDropdown2.SelectCellWithIdx(_targetDevices.IndexOf(PluginConfig.Instance.TargetDevice2));
                    }
                }
            }
            catch (Exception e)
            {
                Plugin.Log.Error(e);
            }
        }
        private void SimpleTextDropdown1_didSelectCellWithIdxEvent(DropdownWithTableView arg1, int arg2)
        {
            PluginConfig.Instance.TargetDevice1 = _targetDevices[arg2];
        }
        private void SimpleTextDropdown2_didSelectCellWithIdxEvent(DropdownWithTableView arg1, int arg2)
        {
            PluginConfig.Instance.TargetDevice2 = _targetDevices[arg2];
        }
        protected override void OnDestroy()
        {
            this._simpleTextDropdown1.didSelectCellWithIdxEvent -= this.SimpleTextDropdown1_didSelectCellWithIdxEvent;
            this._simpleTextDropdown2.didSelectCellWithIdxEvent -= this.SimpleTextDropdown2_didSelectCellWithIdxEvent;
            GameplaySetup.instance.RemoveTab(TabName);
            base.OnDestroy();
        }
        public void Initialize()
        {
            GameplaySetup.instance.AddTab(TabName, this.ResourceName, this);
        }

        [UIAction("TrackedDevicePositionGet")]
        public void TrackedDevicePositionGet()
        {
            var desiredCharacteristics = InputDeviceCharacteristics.TrackedDevice;
            InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, _trackedDevices);
            CreateTargetDeviceList();
            var devPositionText = "";
            foreach (var device in _trackedDevices)
            {
                if (device.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 position) && device.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation))
                    devPositionText += $"{device.serialNumber}:   Pos X={SecondDecimalFormatter(position.x)}  Y={SecondDecimalFormatter(position.y)}  Z={SecondDecimalFormatter(position.z)}   Rot X={IntFormatter(rotation.eulerAngles.x)}  Y={IntFormatter(rotation.eulerAngles.y)}  Z={IntFormatter(rotation.eulerAngles.z)}\n";
            }
            if (_trackedDevices.Count < 23)
            {
                for (int i = _trackedDevices.Count; i <= 23; i++)
                    devPositionText += "\n";
            }
            this._trackedDevicePosition.text = devPositionText;
        }

        [UIAction("OutputLog")]
        public void OutputLog()
        {
            Plugin.Log.Info(this._trackedDevicePosition.text);
        }
    }
}
