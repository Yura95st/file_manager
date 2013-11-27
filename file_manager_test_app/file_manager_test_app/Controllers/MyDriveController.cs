using System;
using System.Collections.Generic;
using file_manager_test_app.Models;
using file_manager_test_app.Views;

namespace file_manager_test_app.Controllers
{
    public class MyDriveController
    {
        private DriveModel _model;
        private DriveView _view;

        public MyDriveController(DriveModel model, DriveView view)
        {
            _model = model;
            _view = view;
            _model.AddObserver(_view);
            _view.AddModel(_model);
        }

        public void Init()
        {
            try
            {
                _model.BuildDrives();
                _model.SetFirstReadyActiveDrive();
            }
            catch (Exception e)
            { }
        }

        public void Refresh()
        {
            _model.ReBuildDrives();
        }

        public void SetActiveDrive(int id)
        {
            _model.ActiveDrive = id;
        }

        public int GetActiveDrive()
        {
            return _model.ActiveDrive;
        }

        public List<MyDriveInfo> GetDrivesList()
        {
            return _model.GetDrivesList();
        }

        public void SetFirstReadyActiveDrive()
        {
            try
            {
                _model.SetFirstReadyActiveDrive();
            }
            catch (Exception e)
            { }
        }
    }
}
