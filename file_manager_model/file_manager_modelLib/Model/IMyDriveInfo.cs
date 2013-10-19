using System;
using System.Collections.Generic;
using System.IO;

namespace file_manager_modelLib.Model
{
    interface IMyDriveInfo
    {
        string Label
        {
            get;
            set;
        }

        DriveType Type
        {
            get;
            set;
        }

        string Format
        {
            get;
            set;
        }

        long FreeSpace
        {
            get;
            set;
        }

        long Size
        {
            get;
            set;
        }
    }
}
