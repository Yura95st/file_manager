using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace file_manager_test_app.Views
{
    public class MainView
    {
        private Window _mainWindow;
        private static List<DriveView> _driveViewList;
        private static List<FileSystemElementView> _fileSystemElementViewList;
        //private PathView _pathView;
        private static List<Controllers.ColumnController> _columnControllersList;

        private static int activeColumnId = 0;

        public MainView(Window mainWindow)
        {
            _mainWindow = mainWindow;
            _driveViewList = new List<DriveView>();
            _fileSystemElementViewList = new List<FileSystemElementView>();
            _columnControllersList = new List<Controllers.ColumnController>();            

            for (int i = 0; i < 2; i++)
            {
                Controllers.ColumnController controller = new Controllers.ColumnController();
                FileSystemElementView fsElementView = new FileSystemElementView(i, _mainWindow, controller);
                DriveView driveView = new DriveView(i, _mainWindow, controller);

                controller.DriveView = driveView;
                controller.FsElementView = fsElementView;

                _columnControllersList.Add(controller);
                _fileSystemElementViewList.Add(fsElementView);
                _driveViewList.Add(driveView);
            }

            //both columnControllers can interract with each other
            _columnControllersList[0].SecondColumnController = _columnControllersList[1];
            _columnControllersList[1].SecondColumnController = _columnControllersList[0];
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

                //FileSystemElementView fv = _fileSystemElementViewList[i];
                //fv.SetElementsList();
            }
        }

        public static int ActiveColumnId
        {
            get
            {
                return activeColumnId;
            }
            set
            {
                activeColumnId = value;
            }
        }
    }
}
