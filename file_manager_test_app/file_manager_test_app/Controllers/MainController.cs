using System;
using System.Collections.Generic;
using System.Timers;
using file_manager_test_app.Controllers;
using file_manager_test_app.Models;
using file_manager_test_app.Views;

namespace file_manager_test_app.Controllers
{
    class MainController
    {
        private List<ColumnController> _controllersList = new List<ColumnController>();

        public MainController(MainWindow window)
        {
            for (int i = 0; i < 2; i++)
            {
                ColumnController controller = new ColumnController();

                DriveView driveView = new DriveView(i, window);
                driveView.AddListener(controller);
                DriveModel driveModel = new DriveModel();                
                controller.SetDriveController(new MyDriveController(driveModel, driveView));

                FileListView fileListView = new FileListView(i, window);
                fileListView.AddListener(controller);
                FileListModel fileListModel = new FileListModel();
                controller.SetFileListController(new FileListController(fileListModel, fileListView));

                _controllersList.Add(controller);
            }

            //Connect two columnControllers with each others
            _controllersList[0].SetColumnController(_controllersList[1]);
            _controllersList[1].SetColumnController(_controllersList[0]);
        }

        public void Init()
        {
            foreach (var controller in _controllersList)
            {
                controller.Init();
            }
        }

        public void Refresh()
        {
            foreach (var controller in _controllersList)
            {
                controller.Refresh();
            }
        }
    }
}
