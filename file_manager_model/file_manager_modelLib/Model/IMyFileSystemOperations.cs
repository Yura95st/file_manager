using System;
using System.Collections.Generic;
using System.IO;

namespace file_manager_modelLib.Model
{
    interface IMyFileSystemOperations
    {
        void Delete();

        void Exists();

        void MoveTo(string destDirName);

        void Read();

        void Create();
    }
}
