using System;
using System.Collections.Generic;

namespace file_manager_test_app.Controllers
{
    //Based on "mediator" pattern
    public class ColumnController
    {
        private Model.Column _column;

        private Controllers.ColumnController _secondColumnController;

        private Views.DriveView _driveView;
        private Views.FileSystemElementView _fsElementView;

        public ColumnController()
        {
            _column = new Model.Column();
        }

        public Controllers.ColumnController SecondColumnController
        {
            set
            {
                _secondColumnController = value;
            }
        }

        public Views.DriveView DriveView
        {
            set
            {
                _driveView = value;
            }
        }

        public Views.FileSystemElementView FsElementView
        {
            get
            {
                return _fsElementView;
            }
            set
            {
                _fsElementView = value;
            }
        }

        public void SelectElements(List<int> elements)
        {
            try
            {
                _column.SelectElements(elements);
            }
            catch (Exception e)
            {
                _fsElementView.SetAlertMessage(e.Message);
            }
        }

        public void WriteToBuffer()
        {
            try
            {
                _column.WriteToBuffer();
            }
            catch (Exception e)
            {
                _fsElementView.SetAlertMessage(e.Message);
            }
        }

        public void CopyTo(int type=0)
        {
            string destination = "";
            try
            {
                //type = 1 - buffered elements copying
                if (type == 1)
                {
                    if (Views.MainView.ActiveColumnId == 0)
                    {
                        destination = this.GetCurrentPath();
                    }
                    else if (Views.MainView.ActiveColumnId == 1)
                    {
                        destination = _secondColumnController.GetCurrentPath();
                    }
                    else 
                    {
                        //TODO: throw Exception;
                    }

                    _column.CopyBufferedElementsTo(destination);
                }
                else
                {
                    destination = _secondColumnController.GetCurrentPath();
                    _column.CopySelectedElementsTo(destination);
                }

                _column.BuildElements();
                //_column.BuildDrives();
                this.FsElementView.Refresh();

                _secondColumnController._column.BuildElements();
                _secondColumnController.FsElementView.Refresh();
            }
            catch (Exception e)
            {
                _fsElementView.SetAlertMessage(e.Message);
            }
        }

        public void SetActiveElement(int id)
        {
            _column.ActiveElement = id;
        }

        public void Delete()
        {
            try
            {
                _column.DeleteSelectedElements();

                _column.BuildElements();
                //_column.BuildDrives();
                this.FsElementView.Refresh();

                _secondColumnController._column.BuildElements();
                _secondColumnController.FsElementView.Refresh();
            }
            catch (Exception e)
            {
                _fsElementView.SetAlertMessage(e.Message);
            }
        }

        public void MoveTo(int type=0)
        {
            string destination = _secondColumnController.GetCurrentPath();

            try
            {
                //type = 1 - buffered elements copying
                if (type == 1)
                {
                    _column.MoveBufferedElementsTo(destination);
                }
                else
                {
                    _column.MoveSelectedElementsTo(destination);
                }

                _column.BuildElements();
                //_column.BuildDrives();
                this.FsElementView.Refresh();

                _secondColumnController._column.BuildElements();
                _secondColumnController.FsElementView.Refresh();
            }
            catch (Exception e)
            {
                _fsElementView.SetAlertMessage(e.Message);
            }
        }

        public void Read()
        {
            try
            {
                _column.Read();
                _fsElementView.Refresh();
            }
            catch (Exception e)
            {
                _fsElementView.SetAlertMessage(e.Message);
            }
        }

        public void CreateNewTxtFile(string name)
        {
            try
            {
                _column.CreateNewTxtFile(name);
            }
            catch (Exception e)
            {
                _fsElementView.SetAlertMessage(e.Message);
            }
        }

        public void CreateNewDirectory(string name)
        {
            try
            {
                _column.CreateNewDirectory(name);
            }
            catch (Exception e)
            {
                _fsElementView.SetAlertMessage(e.Message);
            }
        }

        public void Merge()
        {

        }

        public void Rename(string newName)
        {
            try
            {
                _column.Rename(newName);

            }
            catch (Exception e)
            {
                _fsElementView.SetAlertMessage(e.Message);
            }
        }

        public void Search(string query)
        {

        }

        public void SpecialMegre()
        {

        }

        public void Edit()
        {

        }

        public List<Model.MyFileSystemInfo> GetElementsList()
        {
            return _column.GetElementsList();
        }

        public List<Model.MyDriveInfo> GetDrivesList()
        {
            return _column.GetDrivesList();
        }

        public void SetActiveDrive(int id)
        {
            try
            {
                _column.ActiveDrive = id;
                _fsElementView.Refresh();
            }
            catch (Exception e)
            {
                _fsElementView.SetAlertMessage(e.Message);
                throw e;
            }
        }

        public void SetCurrentPath(string path)
        {
            try
            {
                _column.CurrentPath = path;
                _fsElementView.Refresh();
            }
            catch (Exception e)
            {
                _fsElementView.SetAlertMessage(e.Message);
            }
        }

        public bool isCurrentPathRoot()
        {
            string root = _column.GetDrivesList()[_column.ActiveDrive].RootDirectory;
            string path = _column.CurrentPath;
            return root == path;
        }

        public string GetCurrentPath()
        {
            return _column.CurrentPath;
        }

        public void NavigationHistoryGoBack()
        {
            try
            {
                _column.NavigationHistoryGoBack();
                _fsElementView.Refresh();
            }
            catch (Exception e)
            {
                _fsElementView.SetAlertMessage(e.Message);
            }
        }

        public void SortByDate()
        {

        }

        public void SortByExtention()
        {

        }

        public void SortByName()
        {

        }

        public void SortBySize()
        {

        }

    }
}

