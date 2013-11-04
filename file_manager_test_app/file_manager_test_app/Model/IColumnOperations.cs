using System;
using System.Collections.Generic;

namespace file_manager_test_app.Model
{
    interface IColumnOperations
    {
        void SelectElements(List<int> elements);

        void DeleteSelectedElements();

        void CopyBufferedElementsTo(string destination);

        void CopySelectedElementsTo(string destination);

        void MoveBufferedElementsTo(string destination);

        void MoveSelectedElementsTo(string destination);

        void CreateNewDirectory(string name);

        void CreateNewTxtFile(string name);

        void Rename(string newName);

        void Read();

        void NavigationHistoryGoBack();

        void WriteToBuffer();
    }
}
