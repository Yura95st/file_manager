using System;
using System.Collections.Generic;
using file_manager_test_app.Models;

namespace file_manager_test_app.Controllers
{
    public class ColumnController
    {
        private ColumnController _columnController;
        private MyDriveController _driveController;
        private FileListController _fileListController;

        public ColumnController()
        { }

        public void Init()
        {
            _driveController.Init();
        }

        public void Refresh()
        {
            _driveController.Refresh();
            _fileListController.Refresh();
        }

        public void SetColumnController(ColumnController columnController)
        {
            _columnController = columnController;
        }

        public ColumnController GetColumnController()
        {
            return _columnController;
        }

        public void SetDriveController(MyDriveController driveController)
        {
            _driveController = driveController;
        }

        public void SetFileListController(FileListController fileListController)
        {
            _fileListController = fileListController;
        }

        public void SelectElements(List<int> elements)
        {
            _fileListController.SelectElements(elements);
        }

        public List<int> GetSelectedElements()
        {
            return _fileListController.GetSelectedElements();
        }

        public int GetActiveElement()
        {
            return _fileListController.GetActiveElement();
        }

        public void SetActiveElement(int id)
        {
            _fileListController.SetActiveElement(id);
        }

        public void SetActiveDrive(int id)
        {
            int oldId = _driveController.GetActiveDrive();
            _driveController.SetActiveDrive(id);

            string path = _driveController.GetDrivesList()[_driveController.GetActiveDrive()].RootDirectory;
            _fileListController.GoToPath(path);
        }

        public void Read()
        {
            _fileListController.Read();
        }

        public List<MyFileSystemInfo> GetElementsList()
        {
            return _fileListController.GetElementsList();
        }

        public List<MyDriveInfo> GetDrivesList()
        {
            return _driveController.GetDrivesList();
        }

        public void WriteToBuffer(int type = 0)
        {
            _fileListController.WriteToBuffer(type);
        }

        public void PasteFromBuffer()
        {
            string destination = this.GetCurrentPath();

            _fileListController.PasteBufferedElementsTo(destination);
            RefreshSecondColumnController();
        }

        public string GetCurrentPath()
        {
            return _fileListController.GetCurrentPath();
        }

        public bool isCurrentPathRoot()
        {
            string root = _driveController.GetDrivesList()[_driveController.GetActiveDrive()].RootDirectory;
            string path = _fileListController.GetCurrentPath();
            return root == path;
        }

        public void RefreshFileListElements()
        {
            _fileListController.Refresh();
        }

        public void CopyTo(string destination = "")
        {
            if (destination == "")
            {
                destination = _columnController.GetCurrentPath();
            }
            _fileListController.CopySelectedElementsTo(destination);
            RefreshSecondColumnController();
        }

        public void MoveTo(string destination = "")
        {
            if (destination == "")
            {
                destination = _columnController.GetCurrentPath();
            }
            _fileListController.MoveSelectedElementsTo(destination);
            RefreshSecondColumnController();
        }

        public void Delete(bool delPermanent = false)
        {
            _fileListController.DeleteSelectedElements(delPermanent);
            RefreshSecondColumnController(true);
        }

        public void NavigationHistoryGoBack()
        {
            _fileListController.NavigationHistoryGoBack();
        }

        public void CreateNewTxtFile(string name)
        {
            _fileListController.CreateNewTxtFile(name);
            RefreshSecondColumnController(true);
        }

        public void CreateNewDirectory(string name)
        {
            _fileListController.CreateNewDirectory(name);
            RefreshSecondColumnController(true);
        }

        public void Rename(string newName)
        {
            _fileListController.Rename(newName);
            RefreshSecondColumnController(true);
        }

        public void Merge()
        {
            _fileListController.Merge();
        }

        public void SpecialMerge()
        {
            _fileListController.SpecialMerge();
        }

        public void MyRead()
        {
            _fileListController.MyRead();
        }

        private void RefreshSecondColumnController(bool withPathCheck = false)
        {
            if (!withPathCheck)
            {
                _columnController.RefreshFileListElements();
            }
            else
            {
                if (this.GetCurrentPath() == _columnController.GetCurrentPath())
                {
                    _columnController.RefreshFileListElements();
                }
            }
        }
        
        //public void Merge()
        //{

        //}



        //public void Search(string query)
        //{

        //}

        //public void SpecialMegre()
        //{

        //}

        //public void NavigationHistoryGoBack()
        //{
        //    try
        //    {
        //        _column.NavigationHistoryGoBack();
        //        _fsElementView.Refresh();
        //    }
        //    catch (Exception e)
        //    {
        //        _fsElementView.SetAlertMessage(e.Message);
        //    }
        //}

        //public void SortByDate()
        //{

        //}

        //public void SortByExtention()
        //{

        //}

        //public void SortByName()
        //{

        //}

        //public void SortBySize()
        //{

        //}
    }
}
