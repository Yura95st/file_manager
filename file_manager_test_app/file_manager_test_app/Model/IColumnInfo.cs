using System;
using System.Collections.Generic;

namespace file_manager_test_app.Model
{
    interface IColumnInfo
    {
        string CurrentPath
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

        List<MyFileSystemInfo> GetElementsList();

        List<int> GetSelectedElements();

        List<MyDriveInfo> GetDrivesList();
    }
}
