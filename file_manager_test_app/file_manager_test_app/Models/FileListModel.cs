using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.VisualBasic.FileIO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Text.RegularExpressions;
using file_manager_test_app.Libs.ObserverPattern;

namespace file_manager_test_app.Models
{
    public class FileListModel : ISubject
    {
        private List<IObserver> _observers = new List<IObserver>();

        private List<FileSystemInfo> _elements = new List<FileSystemInfo>();
        private List<int> _selectedElements = new List<int>();
        private int _activeElement = -1;
        private string _currentPath;
        private MyBuffer _buffer = MyBuffer.GetInstance();
        private NavigationHistoryModel navigationHistory = new NavigationHistoryModel();
        private bool _overrideFile = false;

        public FileListModel()
        { }

        public string CurrentPath
        {
            get
            {
                return _currentPath;
            }
        }

        public int ActiveElement
        {
            get
            {
                return _activeElement;
            }
            set
            {
                _activeElement = value;
            }
        }

        public void SelectElements(List<int> elements)
        {
            _selectedElements.Clear();
            _selectedElements = elements;
        }

        public void DeleteSelectedElements(bool delPermanent)
        {
            foreach (int i in _selectedElements)
            {
                try
                {
                    DeleteElement(_elements[i], delPermanent);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        private void DeleteElement(FileSystemInfo element, bool delPermanent)
        {
            DirectoryInfo d = element as DirectoryInfo;
            if (d != null)
            {
                if (delPermanent)
                {
                    d.Delete(true);
                }
                else
                {
                    //Move to Recycle Bin
                    FileSystem.DeleteDirectory(d.FullName, UIOption.AllDialogs, RecycleOption.SendToRecycleBin,
                        UICancelOption.DoNothing);
                }
                return;
            }

            FileInfo f = element as FileInfo;
            if (f != null)
            {
                if (delPermanent)
                {
                    f.Delete();
                }
                else
                {
                    //Move to Recycle Bin
                    FileSystem.DeleteFile(f.FullName, UIOption.AllDialogs, RecycleOption.SendToRecycleBin,
                        UICancelOption.DoNothing);
                }
                return;
            }
        }

        public void CopySelectedElementsTo(string destination)
        {
            List<FileSystemInfo> list = new List<FileSystemInfo>();
            foreach (int i in _selectedElements)
            {
                list.Add(_elements[i]);
            }

            try
            {
                CopyElementsTo(list, destination);
            }
            catch (Exception e)
            {
                throw e;
            }            
        }

        private void CopyElementsTo(List<FileSystemInfo> list, string destination)
        {
            foreach (FileSystemInfo element in list)
            {
                DirectoryInfo d = element as DirectoryInfo;
                if (d != null)
                {
                    CopyDirectoryElementTo(d, destination);
                    continue;
                }

                FileInfo f = element as FileInfo;
                if (f != null)
                {
                    CopyFileElementTo(f, destination);
                    continue;
                }
            }
        }

        private void CopyFileElementTo(FileInfo f, string destination)
        {
            _overrideFile = false;
            string path = System.IO.Path.Combine(destination, f.Name);

            //int i = 0;
            //string newName = System.IO.Path.GetFileNameWithoutExtension(f.Name);

            //while (File.Exists(path))
            //{
            //    if (i == 0)
            //    {
            //        newName += "-Copy"; 
            //    }

            //    path = System.IO.Path.Combine(destination, newName + ((i==0) ? "" : i.ToString()) + f.Extension);
            //    i++;
            //}

            if (File.Exists(path))
            {
                Task overrideAsk = new Task(this.ShowOverrideAskDialog);

                overrideAsk.Start();
                overrideAsk.Wait();
                overrideAsk.Dispose();

                if (!_overrideFile)
                {
                    return;
                }
            }

            f.CopyTo(path, true);
        }

        private void CopyDirectoryElementTo(DirectoryInfo d, string destination)
        {
            string tempPath = Path.Combine(destination, d.Name);

            // If the destination directory doesn't exist, create it. 
            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = d.GetFiles();
            foreach (FileInfo f in files)
            {
                CopyFileElementTo(f, tempPath);
            }

            // Get the subdirectories in the directory and copy them to the new location.
            DirectoryInfo[] subDirectories = d.GetDirectories();
            foreach (DirectoryInfo subdir in subDirectories)
            {
                CopyDirectoryElementTo(subdir, tempPath);
            }
        }

        public void MoveSelectedElementsTo(string destination)
        {
            if (destination != _currentPath)
            {
                try
                {
                    this.CopySelectedElementsTo(destination);
                    this.DeleteSelectedElements(true);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public void PasteBufferedElementsTo(string destination)
        {
            try
            {
                if (_buffer.IsCutPasteType())
                {
                    this.MoveBufferedElementsTo(destination);
                }
                else
                {
                    this.CopyBufferedElementsTo(destination);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void CopyBufferedElementsTo(string destination)
        {
            List<FileSystemInfo> bufferedElements = _buffer.GetElements();
            try
            {
                CopyElementsTo(bufferedElements, destination);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void MoveBufferedElementsTo(string destination)
        {
            try
            {
                this.CopyBufferedElementsTo(destination);
                this.DeleteBufferedElements();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void DeleteBufferedElements()
        {
            List<FileSystemInfo> bufferedElements = _buffer.GetElements();

            foreach (FileSystemInfo element in bufferedElements)
            {
                try
                {
                    DeleteElement(element, true);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public void WriteToBuffer(int type)
        {
            _buffer.Clear();

            foreach (int i in _selectedElements)
            {
                _buffer.AddElement(_elements[i]);
            }

            if (type == 1)
            {
                _buffer.SetCutPasteType();
            }
            else
            {
                _buffer.SetCopyPasteType();
            }
        }

        public void GoToPath(string path, bool addToNavHistory = true)
        {
            _currentPath = path;

            if (!IsCurrentPathDirectory())
            {                
                System.Diagnostics.Process.Start(@_currentPath);
                _currentPath = navigationHistory.GetLastItem();
            }
            else
            {
                if (addToNavHistory)
                {
                    navigationHistory.AddItem(_currentPath);
                }

                try
                {
                    BuildElements();
                }
                catch (Exception e)
                {
                    //if (addToNavHistory)
                    //{
                    //    NavigationHistoryGoBack();
                    //}
                    //throw e;
                }
            }
        }

        public void BuildElements()
        {
            _elements.Clear();
            try
            {
                this.BuildDirectories();
                this.BuildFiles();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                NotifyObservers(0);
            }
        }

        private void BuildFiles()
        {
            DirectoryInfo di = new DirectoryInfo(@_currentPath);
            try
            {
                foreach (FileInfo f in di.GetFiles())
                {
                    _elements.Add(f);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void BuildDirectories()
        {
            DirectoryInfo di = new DirectoryInfo(@_currentPath);
            try
            {
                foreach (DirectoryInfo d in di.GetDirectories())
                {
                    _elements.Add(d);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<MyFileSystemInfo> GetElementsList()
        {
            List<MyFileSystemInfo> myItemsList = new List<MyFileSystemInfo>();

            foreach (FileSystemInfo item in _elements)
            {
                MyFileSystemInfo myItem = new MyFileSystemInfo(item.Name, item.Extension, item.FullName,
                    item.Attributes, item.CreationTime, item.CreationTimeUtc, item.LastAccessTime,
                    item.LastAccessTimeUtc, item.LastWriteTime, item.LastWriteTimeUtc);

                //File has its size, directory doesn't
                FileInfo f = item as FileInfo;
                if (f != null)
                {
                    myItem.Size = f.Length;
                }

                myItemsList.Add(myItem);
            }

            return myItemsList;
        }

        public List<int> GetSelectedElements()
        {
            return _selectedElements;
        }

        public void Read()
        {
            var item = _elements[_activeElement];

            try
            {
                GoToPath(item.FullName);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private bool IsCurrentPathDirectory()
        {
            FileAttributes attr = File.GetAttributes(@_currentPath);

            //detect whether its a directory - (true) or file - (false)
            return (attr & FileAttributes.Directory) == FileAttributes.Directory;
        }

        public void NavigationHistoryGoBack()
        {
            navigationHistory.GoBack();
            string path = navigationHistory.GetLastItem();
            
            if (path != "")
            {
                GoToPath(path, false);
            }
        }

        private void ShowOverrideAskDialog()
        {
            string caption = "File Commander";
            string message = "Do you want to override this file \n\n";
            MessageBoxButton buttons = MessageBoxButton.YesNoCancel;
            MessageBoxImage icon = MessageBoxImage.Question;
            if (MessageBox.Show(message, caption, buttons, icon) == MessageBoxResult.Yes)
            {
                _overrideFile = true;
            }
            else
            {
                return;
            }
        }

        public void CreateNewDirectory(string name)
        {
            string newDirFullName = _currentPath + name;

            if (!IsValidFilename(newDirFullName))
            {
                throw new Exception("Invalid directory name");
            }

            if (Directory.Exists(newDirFullName))
            {
                throw new Exception("Directory already exists");
            }
            
            try
            {
                Directory.CreateDirectory(newDirFullName);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void CreateNewTxtFile(string name)
        {
            string newFileFullName = _currentPath + name;

            if (!IsValidFilename(newFileFullName))
            {
                throw new Exception("Invalid file name");
            }

            if (File.Exists(newFileFullName))
            {
                throw new Exception("File already exists");
            }

            try
            {
                File.Create(newFileFullName);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private bool IsValidFilename(string testName)
        {
            try
            {
                Path.GetFullPath(testName);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public void AddObserver(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void NotifyObservers(int notificationCode)
        {
            foreach (var observer in _observers)
            {
                observer.Update(notificationCode);
            }
        }

        public void Rename(string newName)
        {
            string newPath = Path.Combine(_currentPath, newName);

            if (!IsValidFilename(newPath))
            {
                throw new Exception("Invalid file/folder name");
            }

            FileSystemInfo item = _elements[_activeElement];

            FileInfo fileInfo = item as FileInfo;
            if (fileInfo != null)
            {
                try
                {
                    fileInfo.MoveTo(newPath);
                    return;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            DirectoryInfo directoryInfo = item as DirectoryInfo;
            if (directoryInfo != null)
            {
                try
                {
                    directoryInfo.MoveTo(newPath);
                    return;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
    }
}
