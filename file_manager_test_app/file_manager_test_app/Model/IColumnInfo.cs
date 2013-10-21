using System;
using System.Collections.Generic;
using System.IO;

namespace file_manager_test_app
{
    interface IColumnInfo
    {
        string Path
        {
            get;
            set;
        }

        int ActiveElement
        {
            get;
            set;
        }

        int ActiveDrive
        {
            get;
            set;
        }

        List<DriveInfo> Drives
        {
            get;
        }
    }
}
