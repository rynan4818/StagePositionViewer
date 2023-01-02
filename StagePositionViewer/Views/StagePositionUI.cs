using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.FloatingScreen;
using BeatSaberMarkupLanguage.ViewControllers;
using StagePositionViewer.Configuration;
using HMUI;
using IPA.Utilities;
using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRUIControls;
using Zenject;

namespace StagePositionViewer.Views
{
    /// デンパ時計さんの、BeatmapInformationを流用・参考にしてます。
    /// 参考元ソース:https://github.com/denpadokei/BeatmapInformation/blob/master/BeatmapInformation/Views/BeatmapInformationViewController.cs
    /// ライセンス:https://github.com/denpadokei/BeatmapInformation/blob/master/LICENSE

    [HotReload(RelativePathToLayout = @"StagePositionUI.bsml")]
    [ViewDefinition("StagePositionViewer.Views.StagePositionUI.bsml")]
    public class StagePositionUI : BSMLAutomaticViewController
    {
        [UIObject("positionMap")]
        public readonly GameObject _positionMapObject;
        [UIComponent("positionValue")]
        private readonly TextMeshProUGUI _positionValue;

        public const int UI_SORTING_ORDER = 31;
        public int _sortinglayerOrder;
        public ImageView _playerMarkImg;
        public ImageView _playerMarkImg1;
        public ImageView _playerMarkImg2;
        public GameObject _playerMarkObject;
        public float flontWarning1;
        public float backWarning1;
        public float rightWarning1;
        public float leftWarning1;
        public float flontWarning2;
        public float backWarning2;
        public float rightWarning2;
        public float leftWarning2;
        private FloatingScreen _positionScreen;
        private PauseController _pauseController;
        private static readonly object _lockObject = new object();

        public void PlayerMarkMove(Vector3 playerPosition)
        {
            var x = playerPosition.x * PluginConfig.Instance.ScreenSize;
            var z = playerPosition.z * PluginConfig.Instance.ScreenSize;
            this._playerMarkObject.transform.localPosition = new Vector3(x, z);
            if (playerPosition.x > rightWarning2 || playerPosition.x < -leftWarning2 || playerPosition.z > flontWarning2 || playerPosition.z < -backWarning2)
            {
                this._playerMarkImg.color = Color.red;
                this._playerMarkImg1.color = Color.red;
                this._playerMarkImg2.color = Color.red;
            }
            else if (playerPosition.x > rightWarning1 || playerPosition.x < -leftWarning1 || playerPosition.z > flontWarning1 || playerPosition.z < -backWarning1)
            {
                this._playerMarkImg.color = Color.yellow;
                this._playerMarkImg1.color = Color.yellow;
                this._playerMarkImg2.color = Color.yellow;
            }
            else if (playerPosition.x >= -PluginConfig.Instance.CenterLimitX && playerPosition.x <= PluginConfig.Instance.CenterLimitX)
            {
                this._playerMarkImg.color = Color.white;
                this._playerMarkImg1.color = Color.white;
                this._playerMarkImg2.color = Color.white;
            }
            else
            {
                this._playerMarkImg.color = Color.blue;
                this._playerMarkImg1.color = Color.blue;
                this._playerMarkImg2.color = Color.blue;
            }
            if (this._positionScreen.screenMover.enabled)
                this._positionValue.text = $"X:{playerPosition.x.ToString("F3")} Z:{playerPosition.z.ToString("F3")}";
        }

        public (GameObject, ImageView) DrawMark(String markName, Transform parent, Color color, Vector2 sizeDelta, Vector2 anchoredPosition, float angle)
        {
            var markObject = new GameObject(markName);
            markObject.transform.SetParent(parent, false);
            var image = markObject.AddComponent<ImageView>();
            image.sprite = BeatSaberMarkupLanguage.Utilities.ImageResources.WhitePixel;
            image.material = BeatSaberMarkupLanguage.Utilities.ImageResources.NoGlowMat;
            image.color = color;
            var rt = markObject.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = sizeDelta;
            rt.anchoredPosition = anchoredPosition;
            rt.localEulerAngles = new Vector3(0, 0, angle);
            return (markObject , image);
        }

        public void DrawLine(string objectName, Vector2 startPos, Vector2 endPos, Color color, float width)
        {
            var distance = Vector2.Distance(startPos, endPos);
            var direction = (endPos - startPos).normalized;
            DrawMark(objectName, _positionMapObject.transform, color, new Vector2(distance, width), startPos + direction * distance * 0.5f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        }

        private void OnHandleReleased(object sender, FloatingScreenHandleEventArgs e)
        {
            Plugin.Log.Debug($"Handle Released");
            lock (_lockObject)
            {
                PluginConfig.Instance.ScreenPosX = e.Position.x;
                PluginConfig.Instance.ScreenPosY = e.Position.y;
                PluginConfig.Instance.ScreenPosZ = e.Position.z;
                var rot = e.Rotation.eulerAngles;
                PluginConfig.Instance.ScreenRotX = rot.x;
                PluginConfig.Instance.ScreenRotY = rot.y;
                PluginConfig.Instance.ScreenRotZ = rot.z;
            }
        }
        private void OnHandleGrabbed(object sender, FloatingScreenHandleEventArgs e)
        {
            Plugin.Log.Debug($"Handle Grabbed");
        }
        private void OnDidResumeEvent()
        {
            if (this._positionScreen == null)
            {
                return;
            }
            this._positionScreen.ShowHandle = false;
            this._positionScreen.screenMover.enabled = false;
            this._positionValue.enabled = PluginConfig.Instance.PositionValueView;
            foreach (var canvas in this._positionScreen.GetComponentsInChildren<Canvas>())
            {
                canvas.sortingOrder = this._sortinglayerOrder;
            }
        }

        private void OnDidPauseEvent()
        {
            if (this._positionScreen == null)
            {
                return;
            }
            foreach (var canvas in this._positionScreen.GetComponentsInChildren<Canvas>())
            {
                canvas.sortingOrder = UI_SORTING_ORDER;
            }
            _positionValue.enabled = true;
            if (PluginConfig.Instance.LockPosition)
            {
                return;
            }
            this._positionScreen.ShowHandle = true;
            this._positionScreen.screenMover.enabled = true;
        }

        private IEnumerator Start()
        {
            if (!PluginConfig.Instance.Enable)
                yield break;
            yield return new WaitWhile(() => this._positionScreen == null || !this._positionScreen);
            this._positionScreen.ShowHandle = false;
            var halfWidth = PluginConfig.Instance.StageWidth * PluginConfig.Instance.ScreenSize / 2f;
            var halfHight = PluginConfig.Instance.StageHight * PluginConfig.Instance.ScreenSize / 2f;
            var lineWidth = PluginConfig.Instance.ScreenSize / 20f;
            DrawLine("1", new Vector2(-halfWidth, -halfHight), new Vector2(halfWidth, -halfHight), Color.white, lineWidth);
            DrawLine("2", new Vector2(halfWidth, -halfHight), new Vector2(halfWidth, halfHight), Color.white, lineWidth);
            DrawLine("3", new Vector2(halfWidth, halfHight), new Vector2(-halfWidth, halfHight), Color.white, lineWidth);
            DrawLine("4", new Vector2(-halfWidth, halfHight), new Vector2(-halfWidth, -halfHight), Color.white, lineWidth);
            DrawLine("5", new Vector2(-halfWidth, 0), new Vector2(halfWidth, 0), Color.white, lineWidth / 2f);
            DrawLine("6", new Vector2(0, -halfHight), new Vector2(0, halfHight), Color.white, lineWidth / 2f);

            var flontLimitLine = PluginConfig.Instance.FrontLimitLine * PluginConfig.Instance.ScreenSize;
            var backLimitLine = PluginConfig.Instance.BackLimitLine * PluginConfig.Instance.ScreenSize;
            var rightLimitLine = PluginConfig.Instance.RightLimitLine * PluginConfig.Instance.ScreenSize;
            var leftLimitLine = PluginConfig.Instance.LeftLimitLine * PluginConfig.Instance.ScreenSize;

            DrawLine("flontLimitLine", new Vector2(-leftLimitLine, flontLimitLine), new Vector2(rightLimitLine, flontLimitLine), Color.white, lineWidth / 4f);
            DrawLine("backLimitLine", new Vector2(-leftLimitLine, -backLimitLine), new Vector2(rightLimitLine, -backLimitLine), Color.white, lineWidth / 4f);
            DrawLine("rightLimitLine", new Vector2(rightLimitLine, flontLimitLine), new Vector2(rightLimitLine, -backLimitLine), Color.white, lineWidth / 4f);
            DrawLine("leftLimitLine", new Vector2(-leftLimitLine, flontLimitLine), new Vector2(-leftLimitLine, -backLimitLine), Color.white, lineWidth / 4f);

            var markSize = PluginConfig.Instance.ScreenSize / 4f;
            (_playerMarkObject, _playerMarkImg) = DrawMark("playerMark", _positionMapObject.transform, Color.white, new Vector2(markSize, markSize), Vector2.zero, 45f);
            _playerMarkImg1 = DrawMark("playerMark1", _playerMarkObject.transform, Color.white, new Vector2(markSize * 3f, markSize / 5f), Vector2.zero, -45f).Item2;
            _playerMarkImg2 = DrawMark("playerMark2", _playerMarkObject.transform, Color.white, new Vector2(markSize / 5f, markSize * 3f), Vector2.zero, -45f).Item2;

            flontWarning1 = PluginConfig.Instance.FrontLimitLine * PluginConfig.Instance.WarningPercentage1;
            backWarning1 = PluginConfig.Instance.BackLimitLine * PluginConfig.Instance.WarningPercentage1;
            rightWarning1 = PluginConfig.Instance.RightLimitLine * PluginConfig.Instance.WarningPercentage1;
            leftWarning1 = PluginConfig.Instance.LeftLimitLine * PluginConfig.Instance.WarningPercentage1;
            flontWarning2 = PluginConfig.Instance.FrontLimitLine * PluginConfig.Instance.WarningPercentage2;
            backWarning2 = PluginConfig.Instance.BackLimitLine * PluginConfig.Instance.WarningPercentage2;
            rightWarning2 = PluginConfig.Instance.RightLimitLine * PluginConfig.Instance.WarningPercentage2;
            leftWarning2 = PluginConfig.Instance.LeftLimitLine * PluginConfig.Instance.WarningPercentage2;

            _positionValue.color = Color.white;
            _positionValue.overflowMode = TextOverflowModes.Overflow;
            _positionValue.text = $"X:{0.ToString("F3")} Z:{0.ToString("F3")}";
            _positionValue.enabled = PluginConfig.Instance.PositionValueView;
        }

        protected override void OnDestroy()
        {
            Plugin.Log.Debug("OnDestroy call");
            if (this._pauseController != null)
            {
                this._pauseController.didPauseEvent -= this.OnDidPauseEvent;
                this._pauseController.didResumeEvent -= this.OnDidResumeEvent;
            }
            if (this._positionScreen != null)
            {
                this._positionScreen.HandleGrabbed -= this.OnHandleGrabbed;
                this._positionScreen.HandleReleased -= this.OnHandleReleased;
                Destroy(this._positionScreen);
            }
            base.OnDestroy();
        }

        [UIAction("#post-parse")]
        private void PostParse()
        {
            HMMainThreadDispatcher.instance.Enqueue(this.CanvasConfigUpdate());
        }

        private IEnumerator CanvasConfigUpdate()
        {
            yield return new WaitWhile(() => this._positionScreen == null || !this._positionScreen);
            Plugin.Log.Debug("CanvasConfigUpdate call");
            try
            {
                var coreGameHUDController = Resources.FindObjectsOfTypeAll<CoreGameHUDController>().FirstOrDefault();
                if (coreGameHUDController != null)
                {
                    var energyGo = coreGameHUDController.GetField<GameObject, CoreGameHUDController>("_energyPanelGO");
                    var energyCanvas = energyGo.GetComponent<Canvas>();
                    foreach (var canvas in this._positionScreen.GetComponentsInChildren<Canvas>())
                    {
                        canvas.worldCamera = Camera.main;
                        canvas.overrideSorting = energyCanvas.overrideSorting;
                        canvas.sortingLayerID = energyCanvas.sortingLayerID;
                        canvas.sortingLayerName = energyCanvas.sortingLayerName;
                        this._sortinglayerOrder = energyCanvas.sortingOrder;
                        canvas.sortingOrder = this._sortinglayerOrder;
                        if (PluginConfig.Instance.FirstPersonOnly)
                            canvas.gameObject.layer = PluginConfig.Instance.FirstPersonLayer;
                        else
                            canvas.gameObject.layer = PluginConfig.Instance.ScreenLayer;
                    }
                    foreach (var graphic in this._positionScreen.GetComponentsInChildren<Graphic>())
                    {
                        graphic.raycastTarget = false;
                    }
                    try
                    {
                        Destroy(this._positionScreen.GetComponent<VRGraphicRaycaster>());
                    }
                    catch (Exception e)
                    {
                        Plugin.Log.Error(e);
                    }
                }
            }
            catch (Exception e)
            {
                Plugin.Log.Error(e);
            }
        }
        [Inject]
        private void Constractor(DiContainer container)
        {
            Plugin.Log.Debug("Constractor call");
            this._pauseController = container.TryResolve<PauseController>();
            if (this._pauseController != null)
            {
                this._pauseController.didPauseEvent += this.OnDidPauseEvent;
                this._pauseController.didResumeEvent += this.OnDidResumeEvent;
            }
            var width = PluginConfig.Instance.StageWidth * PluginConfig.Instance.ScreenSize * PluginConfig.Instance.CanvasMargin;
            var hight = PluginConfig.Instance.StageHight * PluginConfig.Instance.ScreenSize * PluginConfig.Instance.CanvasMargin;
            this._positionScreen = FloatingScreen.CreateFloatingScreen(new Vector2(width, hight), true, new Vector3(PluginConfig.Instance.ScreenPosX, PluginConfig.Instance.ScreenPosY, PluginConfig.Instance.ScreenPosZ), Quaternion.Euler(0f, 0f, 0f));
            this._positionScreen.SetRootViewController(this, HMUI.ViewController.AnimationType.None);
            var canvas = this._positionScreen.GetComponentsInChildren<Canvas>(true).FirstOrDefault();
            canvas.renderMode = RenderMode.WorldSpace;
            this._positionScreen.transform.rotation = Quaternion.Euler(PluginConfig.Instance.ScreenRotX, PluginConfig.Instance.ScreenRotY, PluginConfig.Instance.ScreenRotZ);
            this._positionScreen.HandleGrabbed += this.OnHandleGrabbed;
            this._positionScreen.HandleReleased += this.OnHandleReleased;
            this._positionScreen.HandleSide = FloatingScreen.Side.Top;

            //レイヤー調査
            //var a = "";
            //for(int i=0; i < 32; i++)
            //    a += $"{i}:{LayerMask.LayerToName(i)},";
            //Plugin.Log.Debug(a);
            // BeatSaber1.26.0
            // 0:Default
            // 1:TransparentFX
            // 2:Ignore Raycast
            // 3:・・・・・・・・[CameraPlus,Camera2:OnlyInThirdPerson]
            // 4:Water
            // 5:UI
            // 6:・・・・・・・・[CameraPlus,Camera2:OnlyInFirstPerson]
            // 7:
            // 8:Note
            // 9:NoteDebris
            // 10:Avatar
            // 11:Obstacle
            // 12:Saber
            // 13:NeonLight
            // 14:Environment
            // 15:GrabPassTexture1
            // 16:CutEffectParticles
            // 17:
            // 18:
            // 19:NonReflectedParticles
            // 20:EnvironmentPhysics
            // 21:
            // 22:Event
            // 23:
            // 24:・・・・・・・・[CameraPlus,Camera2:CustomNoteLayer]
            // 25:FixMRAlpha
            // 26:
            // 27:DontShowInExternalMRCamera
            // 28:PlayersPlace
            // 29:Skybox
            // 30:MRForegroundClipPlane
            // 31:Reserved
        }
    }
}
