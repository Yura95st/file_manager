using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace file_manager_test_app.Model
{
    public class MyFileSystemInfo
    {
        private string _name;
        private long _size;
        private string _extension;
        private string _fullName;
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

        public MyFileSystemInfo(string name, string extension, string fullName, 
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
        }
    }
}
