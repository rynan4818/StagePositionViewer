using StagePositionViewer.Configuration;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.GameplaySetup;
using BeatSaberMarkupLanguage.ViewControllers;
using System.Globalization;
using Zenject;

// todo 横と縦の警告閾値は変えたほうが良いか？
namespace StagePositionViewer.Views
{
    [HotReload(RelativePathToLayout = @"SettingTabViewController.bsml")]
    [ViewDefinition("StagePositionViewer.Views.SettingTabViewController.bsml")]
    internal class SettingTabViewController : BSMLAutomaticViewController, IInitializable
    {
        public const string TabName = "Stage Position Viewer";
        public string ResourceName => string.Join(".", GetType().Namespace, GetType().Name);
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
        [UIValue("ScreenPosZ")]
        public float ScreenPosZ
        {
            get => PluginConfig.Instance.ScreenPosZ;
            set
            {
                if (PluginConfig.Instance.ScreenPosZ.Equals(value))
                    return;
                PluginConfig.Instance.ScreenPosZ = value;
                NotifyPropertyChanged();
            }
        }
        [UIValue("HmdOnly")]
        public bool HmdOnly
        {
            get => PluginConfig.Instance.HmdOnly;
            set
            {
                PluginConfig.Instance.HmdOnly = value;
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
        [UIAction("PercentageFormatter")]
        private string PercentageFormatter(float value)
        {
            return $"{(value * 100f).ToString("F0", CultureInfo.InvariantCulture)}%";
        }

        [UIAction("ResetCenterPosition")]
        private void ResetCenterPosition()
        {
            PluginConfig.Instance.ScreenPosX = 0;
        }
        [UIAction("ResetRotation")]
        private void ResetRotation()
        {
            PluginConfig.Instance.ScreenRotX = 0;
            PluginConfig.Instance.ScreenRotY = 0;
            PluginConfig.Instance.ScreenRotZ = 0;
        }
        [UIAction("DefaultPosition")]
        private void DefaultPosition()
        {
            PluginConfig.Instance.ScreenPosX = 0;
            PluginConfig.Instance.ScreenPosY = 3.5f;
            this.ScreenPosZ = 7.0f;
            PluginConfig.Instance.ScreenRotX = 0;
            PluginConfig.Instance.ScreenRotY = 0;
            PluginConfig.Instance.ScreenRotZ = 0;
        }

        public void Initialize()
        {
            GameplaySetup.instance.AddTab(TabName, this.ResourceName, this);
        }
    }
}
