using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.FloatingScreen;
using BeatSaberMarkupLanguage.ViewControllers;
using HeadPositionViewer.Configuration;
using HMUI;
using IPA.Utilities;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VRUIControls;
using Zenject;

namespace HeadPositionViewer.Views
{
    /// デンパ時計さんの、BeatmapInformationを参考にしてます。
    /// 参考元ソース:https://github.com/denpadokei/BeatmapInformation/blob/master/BeatmapInformation/Views/BeatmapInformationViewController.cs
    /// ライセンス:https://github.com/denpadokei/BeatmapInformation/blob/master/LICENSE

    [HotReload(RelativePathToLayout = @"HeadPositionUI.bsml")]
    [ViewDefinition("HeadPositionViewer.Views.HeadPositionUI.bsml")]
    public class HeadPositionUI : BSMLAutomaticViewController
    {
        [UIObject("positionMap")]
        public readonly GameObject _positionMapObject;

        public const int UI_SORTING_ORDER = 31;
        public int _sortinglayerOrder;
        public ImageView _headMarkImg;
        public GameObject _headMarkObject;
        public RectTransform _headMarkRectTransform;
        private FloatingScreen _positionScreen;
        private PauseController _pauseController;
        private static readonly object _lockObject = new object();

        public void HeadMarkMove(Vector3 hmdPosition)
        {
            var x = hmdPosition.x * PluginConfig.Instance.ScreenSize;
            var z = hmdPosition.z * PluginConfig.Instance.ScreenSize;
            _headMarkRectTransform.anchoredPosition = new Vector2(x, z);
        }

        public void DrawLine(string objectName, Vector2 startPos, Vector2 endPos, Color color, float width)
        {
            var lineObject = new GameObject(objectName);
            var distance = Vector2.Distance(startPos, endPos);
            var direction = (endPos - startPos).normalized;
            var image = lineObject.AddComponent<ImageView>();
            image.sprite = BeatSaberMarkupLanguage.Utilities.ImageResources.WhitePixel;
            image.material = BeatSaberMarkupLanguage.Utilities.ImageResources.NoGlowMat;
            image.color = color;
            lineObject.transform.SetParent(_positionMapObject.transform, false);
            var rt = lineObject.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = new Vector2(distance, width);
            rt.anchoredPosition = startPos + direction * distance * .5f;
            rt.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        }

        private void OnHandleReleased(object sender, FloatingScreenHandleEventArgs e)
        {
            Plugin.Log.Debug($"Handle Released");
            lock (_lockObject)
            {
                PluginConfig.Instance.ScreenPosX = e.Position.x;
                PluginConfig.Instance.ScreenPosY = e.Position.y;
                PluginConfig.Instance.ScreenPosZ = e.Position.z;

                //プレイヤーの方を向く？
                //var direction = new Vector3(0, 1.7f, 0) - (e.Position - new Vector3(0.5f,0.5f));
                //var lookRotation = Quaternion.LookRotation(direction);
                //this._positionScreen.transform.rotation = lookRotation;
                //var rot = lookRotation.eulerAngles;

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
            var halfWidth = 3f * PluginConfig.Instance.ScreenSize / 2f;
            var halfHight = 2f * PluginConfig.Instance.ScreenSize / 2f;
            var lineWidth = PluginConfig.Instance.ScreenSize / 20f;
            DrawLine("1", new Vector2(-halfWidth, -halfHight), new Vector2(halfWidth, -halfHight), Color.white, lineWidth);
            DrawLine("2", new Vector2(halfWidth, -halfHight), new Vector2(halfWidth, halfHight), Color.white, lineWidth);
            DrawLine("3", new Vector2(halfWidth, halfHight), new Vector2(-halfWidth, halfHight), Color.white, lineWidth);
            DrawLine("4", new Vector2(-halfWidth, halfHight), new Vector2(-halfWidth, -halfHight), Color.white, lineWidth);
            DrawLine("5", new Vector2(-halfWidth, 0), new Vector2(halfWidth, 0), Color.white, lineWidth / 2f);
            DrawLine("6", new Vector2(0, -halfHight), new Vector2(0, halfHight), Color.white, lineWidth / 2f);

            _headMarkObject = new GameObject("headMark");
            _headMarkImg = _headMarkObject.AddComponent<ImageView>();
            _headMarkImg.sprite = BeatSaberMarkupLanguage.Utilities.ImageResources.WhitePixel;
            _headMarkImg.material = BeatSaberMarkupLanguage.Utilities.ImageResources.NoGlowMat;
            _headMarkImg.color = Color.cyan;
            _headMarkObject.transform.SetParent(_positionMapObject.transform, false);
            _headMarkRectTransform = _headMarkObject.GetComponent<RectTransform>();
            _headMarkRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            _headMarkRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            var markSize = PluginConfig.Instance.ScreenSize / 4f;
            _headMarkRectTransform.sizeDelta = new Vector2(markSize, markSize);
            _headMarkRectTransform.anchoredPosition = new Vector2(0, 0);
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
            var width = 3f * PluginConfig.Instance.ScreenSize + 10f;
            var hight = 2f * PluginConfig.Instance.ScreenSize + 10f;
            this._positionScreen = FloatingScreen.CreateFloatingScreen(new Vector2(width, hight), true, new Vector3(PluginConfig.Instance.ScreenPosX, PluginConfig.Instance.ScreenPosY, PluginConfig.Instance.ScreenPosZ), Quaternion.Euler(0f, 0f, 0f));
            this._positionScreen.SetRootViewController(this, HMUI.ViewController.AnimationType.None);
            var canvas = this._positionScreen.GetComponentsInChildren<Canvas>(true).FirstOrDefault();
            canvas.renderMode = RenderMode.WorldSpace;
            this._positionScreen.transform.rotation = Quaternion.Euler(PluginConfig.Instance.ScreenRotX, PluginConfig.Instance.ScreenRotY, PluginConfig.Instance.ScreenRotZ);
            this._positionScreen.HandleGrabbed += this.OnHandleGrabbed;
            this._positionScreen.HandleReleased += this.OnHandleReleased;
            this._positionScreen.HandleSide = FloatingScreen.Side.Bottom;
        }
    }
}
