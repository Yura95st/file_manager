using System;
using System.IO;
using System.Collections.Generic;

namespace file_manager_test_app
{
    class Column : IColumnInfo, IColumnBuild
    {
        private List<FileSystemInfo> _elements;
        private int _activeElement;
        private List<int> _selectedElements;
        private string _path;
        private List<DriveInfo> _drives;
        private int _activeDrive;

        public Column()
        {
            _elements = new List<FileSystemInfo>();
            _drives = new List<DriveInfo>();
            _selectedElements = new List<int>();
            _activeDrive = -1;
            _activeElement = -1;
            _path = "";
        }

        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                //TODO: Check valid value
                _path = value;
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

        public List<DriveInfo> Drives
        {
            get
            {
                return _drives;
            }
        }

        public void Select(int[] elements)
        { }

        public FileSystemInfo GetActiveElement()
        {
            return _elements[_activeElement];
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
                { }
                finally
                { }
            }
        }

        public void CopySelectedElementsTo(string destination)
        {
            if (CheckPath(destination) && destination != _path)
            {
                foreach (int i in _selectedElements)
                {
                    try
                    {
                        var element = _elements[i];
                        if (element.GetType().Name == "DriveInfo")
                        {
                            DirectoryInfo d = (DirectoryInfo)element;
                            d.MoveTo(destination);
                        }
                        else if (element.GetType().Name == "FileInfo")
                        {
                            FileInfo f = (FileInfo)element;
                            f.MoveTo(destination);
                        }
                    }
                    catch (Exception e)
                    { }
                    finally
                    { }
                }
            }
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
                string newDirFullName = _path + name;

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
                string newFileFullName = _path + name;

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

        private void BuildFiles()
        {
            DirectoryInfo di = new DirectoryInfo(_path);
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
            DirectoryInfo di = new DirectoryInfo(_path);
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

        private bool CheckPath(string path)
        {
            DirectoryInfo d = new DirectoryInfo(path);
            return (d != null ? true : false);
        }
    }
}
