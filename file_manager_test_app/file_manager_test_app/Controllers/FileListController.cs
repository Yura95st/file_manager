using System;
using System.Collections.Generic;
using file_manager_test_app.Models;
using file_manager_test_app.Views;
using file_manager_test_app.MyWindows;

namespace file_manager_test_app.Controllers
{
    public class FileListController
    {
        private FileListModel _model;
        private FileListView _view;

        public FileListController(FileListModel model, FileListView view)
        {
            _model = model;
            _view = view;
            _model.AddObserver(_view);
            _view.AddModel(_model);
        }

        public void Init(string path)
        {
            try
            {
                _model.GoToPath(path);
                _model.ActiveElement = 0;
            }
            catch (Exception e)
            { }
        }

        public List<int> GetSelectedElements()
        {
            return _model.GetSelectedElements();
        }

        public void SelectElements(List<int> elements)
        {
            try
            {
                _model.SelectElements(elements);
            }
            catch (Exception e)
            {
                //notify view about error
                _view.SetAlertMessage(e.Message);
            }
        }

        public void SetActiveElement(int id)
        {
            _model.ActiveElement = id;
        }

        public int GetActiveElement()
        {
            return _model.ActiveElement;
        }

        public void Read()
        {
            try
            {
                _model.Read();
            }
            catch (Exception e)
            {
                //notify view about error
                _view.SetAlertMessage(e.Message);
            }
        }

        public List<MyFileSystemInfo> GetElementsList()
        {
            return _model.GetElementsList();
        }

        public string GetCurrentPath()
        {
            return _model.CurrentPath;
        }

        public void Refresh()
        {            
            try
            {
                _model.ReBuildElements();
            }
            catch (Exception e)
            {
                //notify view about error
                _view.SetAlertMessage(e.Message);
            }
        }

        public void CopySelectedElementsTo(string destination)
        {
            try
            {
                _model.CopySelectedElementsTo(destination);
                Refresh();
            }
            catch (Exception e)
            {
                //notify view about error
                _view.SetAlertMessage(e.Message);
            }
        }

        public void MoveSelectedElementsTo(string destination)
        {
            try
            {
                _model.MoveSelectedElementsTo(destination);
                Refresh();
            }
            catch (Exception e)
            {
                //notify view about error
                _view.SetAlertMessage(e.Message);
            }
        }

        public void DeleteSelectedElements(bool delPermanent)
        {
            try
            {
                _model.DeleteSelectedElements(delPermanent);
                Refresh();
            }
            catch (Exception e)
            {
                //notify view about error
                _view.SetAlertMessage(e.Message);
            }
        }

        public void WriteToBuffer(int type)
        {
            try
            {
                _model.WriteToBuffer(type);
            }
            catch (Exception e)
            {
                //notify view about error
                _view.SetAlertMessage(e.Message);
            }
        }

        public void PasteBufferedElementsTo(string destination)
        {
            try
            {
                _model.PasteBufferedElementsTo(destination);
                Refresh();
            }
            catch (Exception e)
            {
                //notify view about error
                _view.SetAlertMessage(e.Message);
            }
        }

        public void NavigationHistoryGoBack()
        {
            try
            {
                _model.NavigationHistoryGoBack();
            }
            catch (Exception e)
            {
                //notify view about error
                _view.SetAlertMessage(e.Message);
            }
        }

        public void GoToPath(string path)
        {            
            try
            {
                _model.GoToPath(path);
            }
            catch (Exception e)
            {
                //notify view about error
                _view.SetAlertMessage(e.Message);
            }
        }

        public void CreateNewTxtFile(string name)
        {
            try
            {
                _model.CreateNewTxtFile(name);
                Refresh();
            }
            catch (Exception e)
            {
                //notify view about error
                _view.SetAlertMessage(e.Message);
            }    
        }

        public void CreateNewDirectory(string name)
        {
            try
            {
                _model.CreateNewDirectory(name);
                Refresh();
            }
            catch (Exception e)
            {
                //notify view about error
                _view.SetAlertMessage(e.Message);
            }
        }

        public void Rename(string newName)
        {
            try
            {
                _model.Rename(newName);
                Refresh();
            }
            catch (Exception e)
            {
                //notify view about error
                _view.SetAlertMessage(e.Message);
            }
        }

        public void Merge()
        {
            try
            {
                _model.Merge(false);
                _view.SetInfoMessage("Files are merged");
            }
            catch (Exception e)
            {
                //notify view about error
                _view.SetAlertMessage(e.Message);
            }
        }

        public void SpecialMerge()
        {
            try
            {
                _model.Merge(true);
                _view.SetInfoMessage("Files are merged with special algorithm");
            }
            catch (Exception e)
            {
                //notify view about error
                _view.SetAlertMessage(e.Message);
            }
        }

        public void MyRead()
        {            
            try
            {
                var element = _model.GetElementById(_model.ActiveElement);
                string path = element.FullName;

                if (_model.IsPathDirectory(path))
                {
                    _model.Read();
                }
                else 
                {
                    TextEditorWindow textEditor = new TextEditorWindow(path);
                    textEditor.Show();
                }
            }
            catch (Exception e)
            {
                //notify view about error
                _view.SetAlertMessage(e.Message);
            }
        }
    }
}
