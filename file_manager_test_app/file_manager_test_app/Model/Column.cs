using System;
using System.IO;
using System.Collections.Generic;

namespace file_manager_test_app.Model
{
    public class Column : IColumnInfo, IColumnBuild, IColumnOperations
    {
        private List<FileSystemInfo> _elements = new List<FileSystemInfo>();
        private int _activeElement = -1;
        private List<int> _selectedElements = new List<int>();
        private string _currentPath;
        private List<DriveInfo> _drives = new List<DriveInfo>();
        private int _activeDrive = -1;
        private List<string> _navigationHistory = new List<string>();
        private int _navigationHistoryPointer = -1;
        private Model.MyBuffer _buffer = Model.MyBuffer.GetInstance();

        public Column()
        {
            BuildDrives();
            _currentPath = _drives[0].RootDirectory.Name;
            NavigationHistoryAddItem();

            try
            {
                BuildElements();
            }
            catch (Exception e)
            { }
            finally
            { }     
        }

        public string CurrentPath
        {
            get
            {
                return _currentPath;
            }
            set
            {
                try
                {
                    string prevPath = _currentPath;

                    CheckPath(value);
                    _currentPath = value;

                    if (IsCurrentPathDirectory())
                    {
                        CheckCurrentPathAsSpecialDirectory();
                        NavigationHistoryAddItem();
                        BuildElements();
                    }
                    else
                    {
                        System.Diagnostics.Process.Start(@_currentPath);
                        _currentPath = prevPath;
                    }
                }
                catch (Exception e)
                {
                    NavigationHistoryGoBack();
                    throw e;
                }
                finally
                { }
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
                if (value < _elements.Count)
                {
                    _activeElement = value;
                }
            }
        }

        public int ActiveDrive
        {
            get
            {
                return _activeDrive;
            }
            set
            {
                //TODO: Check valid value
                try
                {                    
                    CurrentPath = _drives[value].Name;
                    _activeDrive = value;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public void SelectElements(List<int> elements)
        {
            _selectedElements.Clear();
            _selectedElements = elements;
        }

        public void DeleteSelectedElements()
        {
            foreach (int i in _selectedElements)
            {
                try
                {
                    _elements[i].Delete();
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                { }
            }
        }

        public void DeleteBufferedElements()
        {
            List<FileSystemInfo> bufferedElements = _buffer.GetElements();

            foreach (FileSystemInfo element in bufferedElements)
            {
                try
                {
                    element.Delete();
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                { }
            }
        }

        public void CopySelectedElementsTo(string destination)
        {
            try
            {
                CheckPath(destination);
                foreach (int i in _selectedElements)
                {
                    DirectoryInfo d = _elements[i] as DirectoryInfo;
                    if (d != null)
                    {
                        CopyDirectoryElementTo(d, destination);
                        continue;
                    }

                    FileInfo f = _elements[i] as FileInfo;
                    if (f != null)
                    {
                        CopyFileElementTo(f, destination);
                        continue;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            { }
        }

        public void CopyBufferedElementsTo(string destination)
        {
            try
            {
                CheckPath(destination);

                List<FileSystemInfo> bufferedElements = _buffer.GetElements();

                foreach (FileSystemInfo element in bufferedElements)
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
            catch (Exception e)
            {
                throw e;
            }
            finally
            { }
        }

        private void CopyFileElementTo(FileInfo f, string destination)
        {
            try
            {
                string path = System.IO.Path.Combine(destination, f.Name);

                int i = 0;
                string newName = System.IO.Path.GetFileNameWithoutExtension(f.Name);

                while (File.Exists(path))
                {
                    if (i == 0)
                    {
                        newName += "-Copy"; 
                    }

                    path = System.IO.Path.Combine(destination, newName + ((i==0) ? "" : i.ToString()) + f.Extension);
                    i++;
                }

                f.CopyTo(path);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            { }
        }

        private void CopyDirectoryElementTo(DirectoryInfo d, string destination)
        {
            try
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
            catch (Exception e)
            {
                throw e;
            }
            finally
            { }
        }

        public void MoveSelectedElementsTo(string destination)
        {
            if (destination != _currentPath)
            {
                try
                {
                    this.CopySelectedElementsTo(destination);
                    this.DeleteSelectedElements();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public void MoveBufferedElementsTo(string destination)
        {
            if (destination != _currentPath)
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
        }

        public void BuildDrives()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            try
            {
                foreach (DriveInfo d in allDrives)
                {
                    try
                    {
                        _drives.Add(d);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                    finally
                    { }
                }
            }
            catch (Exception e)
            {
                throw e;
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
        }

        public void CreateNewDirectory(string name)
        {
            try
            {
                string newDirFullName = _currentPath + name;

                if (Directory.Exists(newDirFullName))
                {
                    //TODO: throw an exception - directory with this name is already exists
                }
                Directory.CreateDirectory(newDirFullName);
            }
            catch (Exception e)
            {
                //TODO: throw an exception
                throw e;
            }
            finally
            { }
        }

        public void CreateNewTxtFile(string name)
        {
            try
            {
                string newFileFullName = _currentPath + name;

                if (File.Exists(newFileFullName))
                {
                    //TODO: throw an exception - file with this name is already exists
                }
                File.Create(newFileFullName);
            }
            catch (Exception e)
            {
                //TODO: throw an exception
                throw e;
            }
            finally
            { }
        }

        public void Rename(string newName)
        {
            try
            {
                CheckName(newName);

                string newPath = Path.Combine(_currentPath, newName);
                
                FileSystemInfo item = _elements[_activeElement];

                FileInfo fileInfo = item as FileInfo;
                if (fileInfo != null)
                {
                    fileInfo.MoveTo(newPath);
                    return;
                }

                DirectoryInfo directoryInfo = item as DirectoryInfo;
                if (directoryInfo != null)
                {
                    directoryInfo.MoveTo(newPath);
                    return;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            { }
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
                if (f != null) {
                    myItem.Size = f.Length;
                }

                myItemsList.Add(myItem);
            }

            return myItemsList;
        }

        public List<MyDriveInfo> GetDrivesList()
        {
            List<MyDriveInfo> myDrivesList = new List<MyDriveInfo>();

            foreach (DriveInfo d in _drives)
            {
                MyDriveInfo myDrive = new MyDriveInfo(d.AvailableFreeSpace, d.DriveFormat, d.DriveType, 
                    d.Name, d.TotalFreeSpace, d.TotalSize, d.VolumeLabel, d.RootDirectory.Name);

                myDrivesList.Add(myDrive);
            }

            return myDrivesList;
        }

        public void Read()
        {
            var item = _elements[_activeElement];

            try
            {
                CurrentPath = item.FullName;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void NavigationHistoryGoBack()
        {
            if (_navigationHistoryPointer >= 1)
            {
                _currentPath = _navigationHistory[_navigationHistoryPointer - 1];

                try
                {
                    BuildElements();
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    _navigationHistoryPointer--;
                }
            }
        }

        public void WriteToBuffer()
        {
            _buffer.Clear();

            foreach (int i in _selectedElements)
            {
                _buffer.AddElement(_elements[i]);
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

        private bool IsCurrentPathDirectory()
        {
            FileAttributes attr = File.GetAttributes(@_currentPath);

            //detect whether its a directory - (true) or file - (false)
            return (attr & FileAttributes.Directory) == FileAttributes.Directory;
        }

        private void CheckPath(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            else if (path.Length == 0)
            {
                throw new ArgumentException("The path is empty.", "path");
            }
            else
            {
                DirectoryInfo d = new DirectoryInfo(path);
                if (d == null)
                {
                    throw new ArgumentException("Invalid path.", "path");
                }
            }
        }

        private void CheckName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            else if (name.Length == 0)
            {
                throw new ArgumentException("The name is empty.", "name");
            }
            else if (name.IndexOf(Path.AltDirectorySeparatorChar) >= 0
                || name.IndexOf(Path.AltDirectorySeparatorChar) >= 0)
            {
                throw new ArgumentException("The name contains path separators.", "newName");
            }
        }

        private void NavigationHistoryAddItem()
        {
            _navigationHistoryPointer++;
            _navigationHistory.Insert(_navigationHistoryPointer, _currentPath);
        }

        private void CheckCurrentPathAsSpecialDirectory()
        {
            DirectoryInfo di = new DirectoryInfo(@_currentPath);

            if ((di.Attributes & FileAttributes.ReparsePoint) == FileAttributes.ReparsePoint)
            {
                foreach (Environment.SpecialFolder folder in Enum.GetValues(typeof(Environment.SpecialFolder)))
                {
                    if (di.Name.Replace(" ", "").Equals(Enum.GetName(typeof(Environment.SpecialFolder), folder)))
                    {
                        _currentPath = Environment.GetFolderPath(folder);
                        return;
                    }
                }
            }
        }
    }
}
