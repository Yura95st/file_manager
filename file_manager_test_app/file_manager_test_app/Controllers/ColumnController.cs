using System;
using System.Collections.Generic;

namespace file_manager_test_app.Controllers
{
    //Based on "mediator" pattern
    public class ColumnController
    {
        private Model.Column _column;
        private Views.DriveView _driveView;
        private Views.FileSystemElementView _fsElementView;

        public ColumnController()
        {
            _column = new Model.Column();
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
            set
            {
                _fsElementView = value;
            }
        }

        public void Select(int[] elements)
        {
            if (elements.Length > 0)
            {
                _column.Select(elements);
            }
        }

        public void CopyTo(string destination)
        {
            _column.CopySelectedElementsTo(destination);
        }

        public void SetActiveElement(int id)
        {
            _column.ActiveElement = id;
        }

        public void Delete()
        {
            _column.DeleteSelectedElements();
        }

        public void MoveTo(string destination)
        {
            _column.MoveSelectedElementsTo(destination);
        }

        public void Read()
        {
            _column.Read();
            _fsElementView.Refresh();
        }

        public void CreateNewTxtFile(string name)
        {
            _column.CreateNewTxtFile(name);
        }

        public void CreateNewDirectory(string name)
        {
            _column.CreateNewDirectory(name);
        }

        public void Merge()
        {
            
        }

        public void Rename(string newName)
        {
            _column.Rename(newName);
        }

        public void Search(string query)
        {
            
        }

        public void SpecialMegre()
        {
            
        }

        public void ClearSelection()
        {
            _column.ClearSelection();
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
            _column.ActiveDrive = id;
            _fsElementView.Refresh();
        }

        public void SetCurrentPath(string path)
        {
            _column.CurrentPath = path;
            _fsElementView.Refresh();
        }

        public bool isCurrentPathRoot()
        {
            string root = _column.GetDrivesList()[_column.ActiveDrive].RootDirectory;
            string path = _column.CurrentPath;
            return root == path;
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

