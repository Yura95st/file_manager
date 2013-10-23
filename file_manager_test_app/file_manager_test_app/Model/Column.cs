using System;
using System.IO;
using System.Collections.Generic;

namespace file_manager_test_app.Model
{
    public class Column : IColumnInfo, IColumnBuild, IColumnOperations
    {
        private List<FileSystemInfo> _elements;
        private int _activeElement;
        private List<int> _selectedElements;
        private string _currentPath;
        private List<DriveInfo> _drives;
        private int _activeDrive;

        public Column()
        {
            _elements = new List<FileSystemInfo>();
            _drives = new List<DriveInfo>();
            _selectedElements = new List<int>();
            _activeDrive = -1;
            _activeElement = -1;

            BuildDrives();
            _currentPath = _drives[0].Name.ToLower();

            BuildElements();        
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
                    CheckPath(value);
                    _currentPath = value;
                }
                catch (Exception e)
                { }
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
                    _activeElement = value;
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
                _activeDrive = value;
            }
        }

        public void Select(int[] elements)
        { }

        public void DeleteSelectedElements()
        {
            foreach (int i in _selectedElements)
            {
                try
                {
                    _elements[i].Delete();
                }
                catch (Exception e)
                { }
                finally
                { }
            }
        }

        public void CopySelectedElementsTo(string destination)
        {
            try
            {
                CheckPath(destination);

                if (destination != _currentPath)
                {
                    foreach (int i in _selectedElements)
                    {
                        try
                        {
                            DirectoryInfo d = _elements[i] as DirectoryInfo;
                            if (d != null)
                            {
                                d.MoveTo(destination);
                                return;
                            }

                            FileInfo f = _elements[i] as FileInfo;
                            if (f != null)
                            {
                                f.MoveTo(destination);
                                return;
                            }
                        }
                        catch (Exception e)
                        { }
                        finally
                        { }
                    }
                }
            }
            catch (Exception e)
            { }
            finally
            { }
        }

        public void MoveSelectedElementsTo(string destination)
        {
            this.CopySelectedElementsTo(destination);
            this.DeleteSelectedElements();
        }

        public void BuildDrives()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo d in allDrives)
            {
                try
                {
                    _drives.Add(d);
                }
                catch (Exception e)
                {

                }
                finally
                { }
            }
        }

        public void BuildElements()
        {
            this.BuildDirectories();
            this.BuildFiles();
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
            }
            finally
            { }
        }

        public void ClearSelection()
        {
            _selectedElements.RemoveRange(0, _selectedElements.Count);
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
            { }
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
                    d.Name, d.TotalFreeSpace, d.TotalSize, d.VolumeLabel);

                myDrivesList.Add(myDrive);
            }

            return myDrivesList;
        }

        private void BuildFiles()
        {
            DirectoryInfo di = new DirectoryInfo(_currentPath);
            try
            {
                foreach (FileInfo f in di.GetFiles())
                {
                    try
                    {
                        _elements.Add(f);
                    }
                    catch (Exception e)
                    {

                    }
                    finally
                    { }
                }
            }
            catch (Exception e)
            {

            }
            finally { }
        }

        private void BuildDirectories()
        {
            DirectoryInfo di = new DirectoryInfo(_currentPath);
            try
            {
                foreach (DirectoryInfo d in di.GetDirectories())
                {
                    try
                    {
                        _elements.Add(d);
                    }
                    catch (Exception e)
                    {

                    }
                    finally
                    { }
                }
            }
            catch (Exception e)
            {

            }
            finally { }
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
    }
}
