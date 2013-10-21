using System;
using System.Collections.Generic;

namespace file_manager_test_app.Controllers
{
    //Based on "mediator" pattern
    public class FileSystemController
    {
        private Model.Column _leftColumn;
        private Model.Column _rightColumn;
        private Model.Column _activeColumn;

        public FileSystemController()
        {
            _leftColumn = new Model.Column();
            _rightColumn = new Model.Column();
            _activeColumn = _leftColumn;
        }

        public void SetActiveColumn(int id)
        {
            if (id == 0) 
            {
                _activeColumn = _leftColumn;
            }
            else if (id == 1) 
            {
                _activeColumn = _rightColumn;
            }
        }

        public int GetActiveColumnId()
        {
            return (_activeColumn.Equals(_leftColumn) ? 0 : 1);
        }

        public void Select(int[] elements)
        {
            if (elements.Length > 0)
            {
                _activeColumn.Select(elements);
            }
        }

        public void CopyTo(string destination)
        {
            _activeColumn.CopySelectedElementsTo(destination);
        }

        public void SetActive(int id)
        {
            _activeColumn.ActiveElement = id;
        }

        public void Delete()
        {
            _activeColumn.DeleteSelectedElements();
        }

        public void MoveTo(string destination)
        {
            _activeColumn.MoveSelectedElementsTo(destination);
        }

        public void Read()
        {
            
        }

        public void CreateNewTxtFile(string name)
        {
            _activeColumn.CreateNewTxtFile(name);
        }

        public void CreateNewDirectory(string name)
        {
            _activeColumn.CreateNewDirectory(name);
        }

        public void Merge()
        {
            
        }

        public void Rename(string newName)
        {
            _activeColumn.Rename(newName);
        }

        public void Search(string query)
        {
            
        }

        public void SpecialMegre()
        {
            
        }

        public void ClearSelection()
        {
            _activeColumn.ClearSelection();
        }

        public void Edit()
        {
           
        }

        private void SetActiveElement(int id)
        {
            _activeColumn.ActiveDrive = id;
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

