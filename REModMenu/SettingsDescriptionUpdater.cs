using Kingmaker;
using Kingmaker.UI.MVVM._PCView.Settings;
using Kingmaker.UI.MVVM._PCView.Settings.Entities;
using Kingmaker.UI.MVVM._VM.Settings.Entities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WrathRandomEquipment.REModMenu
{
    internal class SettingsDescriptionUpdater<T>
        where T : SettingsEntityWithValueVM
    {
        private readonly string _pathMainUi;
        private readonly string _pathDescriptionUi;

        private Transform _mainUI;
        private Transform _settingsUI;
        private Transform _descriptionUI;

        private List<SettingsEntityWithValueView<T>> _settingViews;
        private SettingsDescriptionPCView _descriptionView;

        public SettingsDescriptionUpdater(string pathUI, string pathDesriptionUI)
        {
            _pathMainUi = pathUI;
            _pathDescriptionUi = pathDesriptionUI;
        }

        private bool Ensure()
        {
            if ((_mainUI = Game.Instance.RootUiContext.m_CommonView.transform) == null)
                return false;

            _settingsUI = _mainUI.Find(_pathMainUi);
            _descriptionUI = _mainUI.Find(_pathDescriptionUi);
            if (_settingsUI == null || _descriptionUI == null)
                return false;

            _settingViews = _settingsUI.gameObject.GetComponentsInChildren<SettingsEntityWithValueView<T>>().ToList();
            _descriptionView = _descriptionUI.GetComponent<SettingsDescriptionPCView>();

            if (_settingViews == null || _descriptionView == null || _settingViews.Count == 0)
                return false;

            return true;
        }

        public bool TryUpdate(string title, string desription)
        {
            if (!Ensure()) return false;

            T svm = null;

            foreach (var settingView in _settingViews)
            {
                var test = (T)settingView.GetViewModel();
                if (test.Title.Equals(title))
                {
                    svm = test;
                    break;
                }
            }

            if (svm == null)
                return false;

            svm.GetType().GetField("Description").SetValue(svm, desription);

            _descriptionView.m_DescriptionText.text = desription;

            return true;
        }
    }
}
