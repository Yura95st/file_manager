using System;
using System.IO;
using System.Collections.Generic;

namespace file_manager_modelLib.Model
{
    class ListColumn
    {
        private List<FileSystemElement> _elementList;
        private int _activeElement;
        private int[] _selectedElements;
        private string _path;
        private List<MyDrive> _drives;
        private int active_drive_;

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
                //TODO: Check valid value
                _activeElement = value;
            }
        }

        public int ActiveDrive_
        {
            get
            {
                return active_drive_;
            }
            set
            {
                //TODO: Check valid value
                active_drive_ = value;
            }
        }

        public List<MyDrive> Drives
        {
            get
            {
                return _drives;
            }
        }

        public void Select();

        public void BuildDrives()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo d in allDrives)
            {
                MyDrive my_drive = new MyDrive(d.Name);

                try
                {
                    my_drive.Label = d.VolumeLabel;
                    my_drive.Type = d.DriveType;
                    my_drive.Format = d.DriveFormat;
                    my_drive.FreeSpace = d.AvailableFreeSpace;
                    my_drive.Size = d.TotalSize;

                    _drives.Add(my_drive);
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

        private void BuildFiles()
        {
            DirectoryInfo di = new DirectoryInfo(_path);
            try
            {
                foreach (FileInfo f in di.GetFiles())
                {
                   

                    try
                    {
                        MyFile my_file = new MyFile(f.Name, f.Length, f.Extension, f.FullName, 
                            f.CreationTimeUtc, f.LastAccessTimeUtc, f.LastWriteTimeUtc);

                        _elementList.Add(my_file);
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
                        MyDirectory my_directory = new MyDirectory(d.Name, d.Extension, d.FullName,
                            d.CreationTimeUtc, d.LastAccessTimeUtc, d.LastWriteTimeUtc);

                        _elementList.Add(my_directory);

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
    }
}
