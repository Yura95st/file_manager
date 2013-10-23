using System;
using System.IO;

namespace file_manager_test_app.Model
{
    public class MyDriveInfo
    {
        private long _availableFreeSpace;
        private string _driveFormat;
        private DriveType _driveType;
        private string _name;
        private long _totalFreeSpace;
        private long _totalSize;
        private string _volumeLabel;

        public long AvailableFreeSpace
        {
            get
            {
                return _availableFreeSpace;
            }
        }

        public string DriveFormat
        {
            get
            {
                return _driveFormat;
            }
        }

        public DriveType DriveType
        {
            get
            {
                return _driveType;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public long TotalFreeSpace
        {
            get
            {
                return _totalFreeSpace;
            }
        }

        public long TotalSize
        {
            get
            {
                return _totalSize;
            }
        }

        public string VolumeLabel
        {
            get
            {
                return _volumeLabel;
            }
        }

        public MyDriveInfo(long availableFreeSpace, string driveFormat, DriveType driveType,
            string name, long totalFreeSpace, long totalSize, string volumeLabel)
        {
            _availableFreeSpace = availableFreeSpace;
            _driveFormat = driveFormat;
            _driveType = driveType;
            _name = name.ToLower().Substring(0, name.Length - 2);
            _totalFreeSpace = totalFreeSpace;
            _totalSize = totalSize;
            _volumeLabel = volumeLabel.ToLower();
        }
    }
}
