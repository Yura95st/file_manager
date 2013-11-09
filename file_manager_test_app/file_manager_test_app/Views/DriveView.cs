using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using file_manager_test_app.Libs.ObserverPattern;
using file_manager_test_app.Models;
using file_manager_test_app.Controllers;

namespace file_manager_test_app.Views
{
    public class DriveView : IObserver
    {
        //_type: 0 - left, 1 - right
        private int _type;
        private Window _mainWindow;

        private ColumnController _controller;
        private DriveModel _model;

        private string[] _driveButtonsPanelNames = { "LeftDriveButtonsPanel", "RightDriveButtonsPanel" };
        private string[] _driveComboBoxPanelNames = { "LeftDriveComboBox", "RightDriveComboBox" };
        private string[] _driveInfoLabelsNames = { "LeftDriveInfoLabel", "RightDriveInfoLabel" };

        public DriveView(int type, Window mainWindow)
        {
            _type = type;
            _mainWindow = mainWindow;
            BindDriveComboBoxSelectionChanged();
        }

        public void AddListener(ColumnController controller)
        {
            _controller = controller;
        }

        public void AddModel(DriveModel model)
        {
            _model = model;
        }

        public void Update(int notificationCode)
        {
            switch (notificationCode)
            {
                case 0:
                    BuildDrivePanels();
                    SetActiveDriveInPanels();
                    break;

                case 1:
                    BuildDrivePanels();                    
                    break;

                case 2:
                    SetActiveDriveInPanels();
                    break;
            }
        }

        public void BuildDrivePanels()
        {
            List<MyDriveInfo> drivesList = _model.GetDrivesList();

            BuildDriveButtonsPanel(drivesList);
            BuildDriveComboBoxPanel(drivesList);
        }

        private void BuildDriveButtonsPanel(List<MyDriveInfo> drivesList)
        {
            StackPanel drivePanel = (StackPanel)_mainWindow.FindName(_driveButtonsPanelNames[_type]);
            drivePanel.Children.Clear();

            int i = 0;
            foreach (MyDriveInfo d in drivesList)
            {
                Button button = new Button();
                button.Uid = i.ToString();
                button.Content = d.Name;
                button.Margin = new Thickness(5);
                button.MinWidth = 40;
                button.Click += driveButton_OnClick;

                drivePanel.Children.Add(button);
                i++;
            }
        }

        private void BuildDriveComboBoxPanel(List<MyDriveInfo> drivesList)
        {
            ComboBox driveComboBox = (ComboBox)_mainWindow.FindName(_driveComboBoxPanelNames[_type]);
            driveComboBox.Items.Clear();

            foreach (MyDriveInfo d in drivesList)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = d.Name;

                driveComboBox.Items.Add(item);
            }        
        }

        private void BindDriveComboBoxSelectionChanged()
        {
            ComboBox driveComboBox = (ComboBox)_mainWindow.FindName(_driveComboBoxPanelNames[_type]);
            driveComboBox.SelectionChanged += driveComboBoxItem_OnSelect;
        }

        private void UnbindDriveComboBoxSelectionChanged()
        {
            ComboBox driveComboBox = (ComboBox)_mainWindow.FindName(_driveComboBoxPanelNames[_type]);
            driveComboBox.SelectionChanged -= driveComboBoxItem_OnSelect;
        }

        private void SetActiveDriveInPanels()
        {
            List<MyDriveInfo> drivesList = _model.GetDrivesList();
            int id = _model.ActiveDrive;
            MyDriveInfo d = drivesList[_model.ActiveDrive];

            UnbindDriveComboBoxSelectionChanged();            

            SetActiveDriveAsButton(id);
            SetActiveDriveAsComboBoxItem(id);
            SetDriveInfoLabel(d);

            BindDriveComboBoxSelectionChanged();
        }

        private void SetDriveInfoLabel(MyDriveInfo d)
        {
            Label label = (Label)_mainWindow.FindName(_driveInfoLabelsNames[_type]);
            string labelText = "[" + d.VolumeLabel + "] " + d.AvailableFreeSpace + " b of " + d.TotalFreeSpace + " b free";

            label.Content = labelText;
        }

        private void SetActiveDriveAsButton(int id)
        {
            StackPanel drivePanel = (StackPanel)_mainWindow.FindName(_driveButtonsPanelNames[_type]);

            foreach (var child in drivePanel.Children)
            {
                Button driveButton = child as Button;
                driveButton.IsEnabled = (driveButton.Uid != id.ToString()) ? true : false;
            }
        }

        private void SetActiveDriveAsComboBoxItem(int id)
        {
            ComboBox driveComboBox = (ComboBox)_mainWindow.FindName(_driveComboBoxPanelNames[_type]);
            driveComboBox.SelectedIndex = id;
        }

        private void driveButton_OnClick(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            int driveId = Convert.ToInt32(b.Uid);
            SetActiveDrive(driveId);
        }

        private void driveComboBoxItem_OnSelect(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            SetActiveDrive(comboBox.SelectedIndex);
        }

        public void SetActiveDrive(int id)
        {
            _controller.SetActiveDrive(id);
        }
    }
}
