using System;
using System.Collections.Generic;
using System.Windows;

namespace file_manager_test_app.Views
{
    public class MainView
    {
        private Window _mainWindow;
        private DriveView _leftDriveView;
        private DriveView _rightDriveView;
        private FileSystemElementView _leftFileSystemElementView;
        private FileSystemElementView _rightFileSystemElementView;

        public MainView(Window mainWindow)
        {
            _mainWindow = mainWindow;
            _leftDriveView = new DriveView(0, _mainWindow);
            _rightDriveView = new DriveView(1, _mainWindow);

            _leftFileSystemElementView = new FileSystemElementView(0, _mainWindow);
            _rightFileSystemElementView = new FileSystemElementView(1, _mainWindow);
        }

        public void Init()
        {
            _leftDriveView.BuildDriveButtonsPanel();
            _rightDriveView.BuildDriveButtonsPanel();

            _leftDriveView.BuildDriveComboBoxPanel();
            _rightDriveView.BuildDriveComboBoxPanel();

            _leftDriveView.BuildDriveInfoLabel();
            _rightDriveView.BuildDriveInfoLabel();

            _leftFileSystemElementView.BuildList();
            _rightFileSystemElementView.BuildList();
        }
    }
}
