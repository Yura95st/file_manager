using System;
using System.Collections.Generic;
using System.IO;

namespace file_manager_modelLib.Model
{
    interface IMyFileSystemInfo
    {
        string GetName();

        string GetPath();

        string GetExtension();

        DateTime GetLastAccessTime();

        DateTime GetLastWriteTime();

        DateTime GetCreationTime();

        MyDirectory GetParent();
    }
}
