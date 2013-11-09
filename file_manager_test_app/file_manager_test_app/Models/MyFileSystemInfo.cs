using System;
using System.IO;

namespace file_manager_test_app.Models
{
    public class MyFileSystemInfo
    {
        private string _name;
        private long _size;
        private string _extension;
        private string _fullName;
        private bool _isHidden;
        private bool _isSystem;
        private bool _isReadOnly;
        private DateTime _creationTime;
        private DateTime _creationTimeUtc;
        private DateTime _lastAccessTime;
        private DateTime _lastAccessTimeUtc;
        private DateTime _lastWriteTime;
        private DateTime _lastWriteTimeUtc;

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public long Size
        {
            get
            {
                return _size;
            }
            set 
            {
                _size = value;
            }
        }

        public string Extension
        {
            get
            {
                return _extension;
            }
        }

        public string FullName
        {
            get
            {
                return _fullName;
            }
        }

        public bool IsHidden
        {
            get
            {
                return _isHidden;
            }
        }

        public bool IsSystem
        {
            get
            {
                return _isSystem;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return _isReadOnly;
            }
        }

        public DateTime CreationTime
        {
            get
            {
                return _creationTime;
            }
        }

        public DateTime CreationTimeUtc
        {
            get
            {
                return _creationTimeUtc;
            }
        }

        public DateTime LastAccessTime
        {
            get
            {
                return _lastAccessTime;
            }
        }

        public DateTime LastAccessTimeUtc
        {
            get
            {
                return _lastAccessTimeUtc;
            }
        }

        public DateTime LastWriteTime
        {
            get
            {
                return _lastWriteTime;
            }
        }

        public DateTime LastWriteTimeUtc
        {
            get
            {
                return _lastWriteTimeUtc;
            }
        }

        public MyFileSystemInfo()
        { }

        public MyFileSystemInfo(string name, string extension, string fullName, FileAttributes attributes,
            DateTime creationTime, DateTime creationTimeUtc, DateTime lastAccessTime,
            DateTime lastAccessTimeUtc, DateTime lastWriteTime, DateTime lastWriteTimeUtc)
        {
            _name = name;
            _size = 0;
            _extension = extension;
            _fullName = fullName;
            _creationTime = creationTime;
            _creationTimeUtc = creationTimeUtc;
            _lastAccessTime = lastAccessTime;
            _lastAccessTimeUtc = lastAccessTimeUtc;
            _lastWriteTime = lastWriteTime;
            _lastWriteTimeUtc = lastWriteTimeUtc;

            _isHidden = ((attributes & FileAttributes.Hidden) == FileAttributes.Hidden);
            _isSystem = ((attributes & FileAttributes.System) == FileAttributes.System);
            _isReadOnly = ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly);
        }
    }
}
