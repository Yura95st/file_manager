namespace file_manager_test_app.Models
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using Microsoft.VisualBasic.FileIO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Text.RegularExpressions;
    using file_manager_test_app.Libs.ObserverPattern;
    using file_manager_test_app.MyWindows;

    public enum OverrideType { Unknown, NotOverride, CreateCopy, Override, OverrideAll };

    public class FileListModel : ISubject
    {
        private List<IObserver> _observers = new List<IObserver>();

        private List<FileSystemInfo> _elements = new List<FileSystemInfo>();
        private List<int> _selectedElements = new List<int>();
        private int _activeElement = 0;
        private string _currentPath;
        private MyBuffer _buffer = MyBuffer.GetInstance();
        private NavigationHistoryModel navigationHistory = new NavigationHistoryModel();
        private OverrideType _overrideType = OverrideType.Unknown;

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

        private void CopyElementsTo(List<FileSystemInfo> list, string destination, OverrideType overrideType = OverrideType.Unknown)
        {
            _overrideType = overrideType;

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
            string path = Path.Combine(destination, f.Name);
            string filePath = f.FullName;

            OverrideType tempOverrideType = _overrideType;

            if (_overrideType != OverrideType.CreateCopy && filePath == path)
            {
                throw new Exception("You can not copy a file to itself");
            }

            if (_overrideType == OverrideType.NotOverride)
            {
                return;
            }
            else if (_overrideType == OverrideType.CreateCopy)
            {
                int i = 1;
                string newName = Path.GetFileNameWithoutExtension(f.Name);

                while (File.Exists(path))
                {
                    if (i == 1)
                    {
                        newName += " - Copy";
                    }

                    path = Path.Combine(destination, newName + ((i == 1) ? "" : " (" + i.ToString() + ")") + f.Extension);
                    i++;
                }
            }
            else if (_overrideType == OverrideType.Unknown)
            {

                if (File.Exists(path))
                {
                    Task overrideAsk = new Task(this.ShowOverrideAskDialog);

                    overrideAsk.Start();
                    overrideAsk.Wait();
                    overrideAsk.Dispose();

                    if (_overrideType != OverrideType.OverrideAll)
                    {                        
                        if (_overrideType == OverrideType.NotOverride)
                        {
                            _overrideType = tempOverrideType;
                            return;
                        }

                        _overrideType = tempOverrideType;
                    }
                }
            }

            f.CopyTo(path, true);
        }

        private void CopyDirectoryElementTo(DirectoryInfo d, string destination)
        {
            string tempPath = Path.Combine(destination, d.Name);
            string dirPath = Path.Combine(_currentPath, d.Name);

            if (_overrideType != OverrideType.CreateCopy && dirPath == destination || tempPath == dirPath)
            {
                throw new Exception("You can not copy/move a directory to it's own subdirectory");
            }

            if (_overrideType == OverrideType.CreateCopy)
            {
                int i = 1;
                string newName = d.Name;

                while (Directory.Exists(tempPath))
                {
                    if (i == 1)
                    {
                        newName += " - Copy";
                    }

                    tempPath = Path.Combine(destination, newName + ((i == 1) ? "" : " (" + i.ToString() + ")"));
                    i++;
                }
            }

            Directory.CreateDirectory(tempPath);

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
                CopyElementsTo(bufferedElements, destination, OverrideType.CreateCopy);
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

            if (!IsPathDirectory(_currentPath))
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
                this.BuildDirectories(_elements);
                this.BuildFiles(_elements);
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

        public void ReBuildElements()
        {
            List<FileSystemInfo> rebuildedElements = new List<FileSystemInfo>();
            try
            {
                this.BuildDirectories(rebuildedElements);
                this.BuildFiles(rebuildedElements);
            }
            catch (Exception e)
            {
                throw e;
            }

            try
            {
                if (rebuildedElements.Count != _elements.Count)
                {
                    throw new Exception("Elements list is chaged.");
                }

                for (int i = 0, count = rebuildedElements.Count; i < count; i++)
                {
                    if (rebuildedElements[i].FullName != _elements[i].FullName)
                    {
                        throw new Exception("Elements list is chaged.");
                    }
                }
            }
            catch (Exception e)
            {
                _elements.Clear();
                _elements = rebuildedElements;
                NotifyObservers(0);
            }
        }

        private void BuildFiles(List<FileSystemInfo> elements)
        {
            DirectoryInfo di = new DirectoryInfo(@_currentPath);
            try
            {
                foreach (FileInfo f in di.GetFiles())
                {
                    elements.Add(f);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void BuildDirectories(List<FileSystemInfo> elements)
        {
            DirectoryInfo di = new DirectoryInfo(@_currentPath);
            try
            {
                elements.Add(di);

                foreach (DirectoryInfo d in di.GetDirectories())
                {
                    elements.Add(d);
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

        public MyFileSystemInfo GetElementById(int id)
        {
            if (id > _elements.Count)
            {
                throw new Exception("Invalid id");
            }

            var item = _elements[id];

            MyFileSystemInfo myItem = new MyFileSystemInfo(item.Name, item.Extension, item.FullName,
               item.Attributes, item.CreationTime, item.CreationTimeUtc, item.LastAccessTime,
               item.LastAccessTimeUtc, item.LastWriteTime, item.LastWriteTimeUtc);

            //File has its size, directory doesn't
            FileInfo f = item as FileInfo;
            if (f != null)
            {
                myItem.Size = f.Length;
            }

            return myItem;
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

        public bool IsPathDirectory(string path)
        {
            try
            {
                FileAttributes attr = File.GetAttributes(@path);

                //detect whether its a directory - (true) or file - (false)
                return (attr & FileAttributes.Directory) == FileAttributes.Directory;
            }
            catch (Exception e)
            {
                throw e;
            }            
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
            string message = "Do you want to override selected files (Cancel == OverrideAll) \n\n";
            //string message = "Override:\n\n\t" + firstPath + "\n\n With:\n\n\t" + secondPath + "\n\n(Cancel == OverrideAll)\n\n";
            MessageBoxButton buttons = MessageBoxButton.YesNoCancel;
            MessageBoxImage icon = MessageBoxImage.Question;
            MessageBoxResult result = MessageBox.Show(message, caption, buttons, icon);

            if (result == MessageBoxResult.Yes)
            {
                _overrideType = OverrideType.Override;
            }
            else if (result == MessageBoxResult.No)
            {
                _overrideType = OverrideType.NotOverride;
            }
            else if (result == MessageBoxResult.Cancel)
            {
                _overrideType = OverrideType.OverrideAll;
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

            //New name is the same as old
            if (newPath == item.FullName)
            {
                return;
            }

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

        public void Merge(bool isSpecial)
        {
            List<FileSystemInfo> list = new List<FileSystemInfo>();
            foreach (int i in _selectedElements)
            {
                list.Add(_elements[i]);
            }

            if (list.Count != 2)
            {
                throw new Exception("Too many/few elements selected. It is allowed to select only 2 elements.");
            }

            foreach (FileSystemInfo element in list)
            {
                DirectoryInfo d = element as DirectoryInfo;
                if (d != null)
                {
                    throw new Exception("Selected elements contain directory");
                }

                FileInfo f = element as FileInfo;
                if (f != null)
                {
                    continue;
                }
            }

            string dividingLine = "\n\n----------MERGED_TEXT----------\n\n";

            if (!isSpecial)
            {
                try
                {
                    var text = dividingLine + File.ReadAllText(@list[1].FullName);
                    File.AppendAllText(@list[0].FullName, text);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else
            {                
                try
                {
                    List<string> firstLinesList = new List<string>(File.ReadAllLines(@list[0].FullName));
                    List<string> secondLinesList = new List<string>(File.ReadAllLines(@list[1].FullName));

                    List<string> finalLinesList = new List<string>(firstLinesList);
                    finalLinesList.Add(dividingLine);

                    foreach (var line in secondLinesList) 
                    {
                        if (!firstLinesList.Contains(line))
                        {
                            finalLinesList.Add(line);
                        }
                    }

                    File.WriteAllLines(@list[0].FullName, finalLinesList);
                }
                catch (Exception e)
                {
                    throw e;
                }
 
            }

        }
    }
}
