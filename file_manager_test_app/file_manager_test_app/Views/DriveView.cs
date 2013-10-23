using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace file_manager_test_app.Views
{
    public class DriveView
    {
        //_type: 0 - left, 1 - right
        private int _type;
        private Window _mainWindow;
        private Controllers.FileSystemController _controller;
        private List<Model.MyDriveInfo> _drives;
        private int _activeDrive = 0;

        private string[] _driveButtonsPanelNames = { "LeftDriveButtonsPanel", "RightDriveButtonsPanel" };
        private string[] _driveComboBoxPanelNames = { "LeftDriveComboBox", "RightDriveComboBox" };
        private string[] _driveInfoLabelsNames = { "LeftDriveInfoLabel", "RightDriveInfoLabel" };

        public DriveView(int type, Window mainWindow)
        {
            _type = type;
            _mainWindow = mainWindow;
            _controller = new Controllers.FileSystemController();
            _drives = _controller.GetDrivesList();
        }

        public void BuildDriveButtonsPanel()
        {
            StackPanel drivePanel = (StackPanel)_mainWindow.FindName(_driveButtonsPanelNames[_type]);

            foreach(Model.MyDriveInfo d in _drives)
            {
                Button button = new Button();
                button.Content = d.Name;
                button.Margin = new Thickness(5);
                button.MinWidth = 40;

                drivePanel.Children.Add(button);
            }
        }

        public void BuildDriveComboBoxPanel()
        {
            ComboBox driveComboBox = (ComboBox)_mainWindow.FindName(_driveComboBoxPanelNames[_type]);

            foreach (Model.MyDriveInfo d in _drives)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = d.Name;

                driveComboBox.Items.Add(item);
            }
        }

        public void SetActiveDrive()
        { }

        public void BuildDriveInfoLabel()
        {
            Label label = (Label)_mainWindow.FindName(_driveInfoLabelsNames[_type]);
            var d = _drives[_activeDrive];
            string labelText = "[" + d.VolumeLabel + "] " + d.AvailableFreeSpace + " b of " + d.TotalFreeSpace + " b free";

            label.Content = labelText;
        }
    }
}
