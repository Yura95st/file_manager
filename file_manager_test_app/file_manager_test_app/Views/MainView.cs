using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace file_manager_test_app.Views
{
    public class MainView
    {
        private Window _mainWindow;
        private List<DriveView> _driveViewList;
        private List<FileSystemElementView> _fileSystemElementViewList;
        private List<Controllers.ColumnController> _controllerList;

        public MainView(Window mainWindow)
        {
            _mainWindow = mainWindow;
            _driveViewList = new List<DriveView>();
            _fileSystemElementViewList = new List<FileSystemElementView>();
            _controllerList = new List<Controllers.ColumnController>();

            for (int i = 0; i < 2; i++)
            {
                Controllers.ColumnController controller = new Controllers.ColumnController();
                FileSystemElementView fsElementView = new FileSystemElementView(i, _mainWindow, controller);
                DriveView driveView = new DriveView(i, _mainWindow, controller);

                controller.DriveView = driveView;
                controller.FsElementView = fsElementView;

                _controllerList.Add(controller);
                _fileSystemElementViewList.Add(fsElementView);
                _driveViewList.Add(driveView);
            }
        }

        public void Init()
        {
            for (int i = 0; i < 2; i++)
            {
                DriveView dv = _driveViewList[i];
                dv.BuildDriveButtonsPanel();
                dv.BuildDriveComboBoxPanel();
                dv.SetDriveInfoLabel();
                dv.SetActiveDrive(0);

                FileSystemElementView fv = _fileSystemElementViewList[i];
                fv.SetElementsList();
            }
        }
    }
}
