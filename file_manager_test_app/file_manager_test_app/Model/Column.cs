

        //private void CheckCurrentPathAsSpecialDirectory()
        //{
        //    DirectoryInfo di = new DirectoryInfo(@_currentPath);

        //    if ((di.Attributes & FileAttributes.ReparsePoint) == FileAttributes.ReparsePoint)
        //    {
        //        foreach (Environment.SpecialFolder folder in Enum.GetValues(typeof(Environment.SpecialFolder)))
        //        {
        //            if (di.Name.Replace(" ", "").Equals(Enum.GetName(typeof(Environment.SpecialFolder), folder)))
        //            {
        //                _currentPath = Environment.GetFolderPath(folder);
        //                return;
        //            }
        //        }
        //    }
        //}
