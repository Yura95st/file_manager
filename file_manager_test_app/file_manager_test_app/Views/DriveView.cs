using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace file_manager_test_app.Views
{
    public class DriveView
    {
        //_type: 0 - left, 1 - right
        private int _type;
        private Window _mainWindow;
        private Controllers.ColumnController _controller;
        private List<Model.MyDriveInfo> _drives;
        private int _activeDrive = 0;

        private string[] _driveButtonsPanelNames = { "LeftDriveButtonsPanel", "RightDriveButtonsPanel" };
        private string[] _driveComboBoxPanelNames = { "LeftDriveComboBox", "RightDriveComboBox" };
        private string[] _driveInfoLabelsNames = { "LeftDriveInfoLabel", "RightDriveInfoLabel" };

        public DriveView(int type, Window mainWindow, Controllers.ColumnController controller)
        {
            _type = type;
            _mainWindow = mainWindow;
            _controller = controller;
            _drives = _controller.GetDrivesList();
        }

        public void BuildDriveButtonsPanel()
        {
            StackPanel drivePanel = (StackPanel)_mainWindow.FindName(_driveButtonsPanelNames[_type]);

            int i = 0;
            foreach (Model.MyDriveInfo d in _drives)
            {
                Button button = new Button();
                button.Uid = "" + i++;
                button.Content = d.Name;
                button.Margin = new Thickness(5);
                button.MinWidth = 40;
                button.Click += driveButton_Click;

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

            driveComboBox.SelectionChanged += driveComboBoxItem_Select;
        }

        public void SetActiveDrive(int id)
        {
            _activeDrive = id;
            SetActiveDriveAsButton();
            SetActiveDriveAsComboBoxItem();
            SetDriveInfoLabel();

            _controller.SetActiveDrive(_activeDrive);
        }

        public void SetDriveInfoLabel()
        {
            Label label = (Label)_mainWindow.FindName(_driveInfoLabelsNames[_type]);
            var d = _drives[_activeDrive];
            string labelText = "[" + d.VolumeLabel + "] " + d.AvailableFreeSpace + " b of " + d.TotalFreeSpace + " b free";

            label.Content = labelText;
        }

        private void SetActiveDriveAsButton()
        {
            StackPanel drivePanel = (StackPanel)_mainWindow.FindName(_driveButtonsPanelNames[_type]);

            foreach (var child in drivePanel.Children)
            {
                Button driveButton = child as Button;
                driveButton.IsEnabled = (driveButton.Uid != _activeDrive.ToString()) ? true : false;
            }
        }

        private void SetActiveDriveAsComboBoxItem()
        {
            ComboBox driveComboBox = (ComboBox)_mainWindow.FindName(_driveComboBoxPanelNames[_type]);
            driveComboBox.SelectedIndex = _activeDrive;
        }

        private void driveButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            int driveId = Convert.ToInt32(b.Uid);
            SetActiveDrive(driveId);
        }

        private void driveComboBoxItem_Select(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            SetActiveDrive(comboBox.SelectedIndex);
        }
    }
}
