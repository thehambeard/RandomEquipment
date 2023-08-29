using Kingmaker;
using Kingmaker.PubSubSystem;
using Kingmaker.Utility;
using ModMaker;
using UnityEngine;

namespace WrathRandomEquipment.REModMenu
{
    internal class Fix : ISettingsUIHandler, IModEventHandler
    {
        public int Priority => 100;

        public void HandleModDisable()
        {
            EventBus.Unsubscribe(this);
        }

        public void HandleModEnable()
        {
            EventBus.Subscribe(this);
        }

        public void HandleOpenSettings(bool isMainMenu = false)
        {
            foreach (RectTransform transform in Game.Instance.RootUiContext.m_CommonView?.transform.Find("Canvas/SettingsView/ContentWrapper/MenuSelectorPCView"))
                transform.ResetScale();
        }
    }
}
