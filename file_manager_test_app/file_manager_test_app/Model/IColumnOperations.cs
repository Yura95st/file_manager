using System;
using System.Collections.Generic;

namespace file_manager_test_app.Model
{
    interface IColumnOperations
    {
        void SelectElements(List<int> elements);

        void DeleteSelectedElements(bool delPermanent);

        void CopySelectedElementsTo(string destination);

        void PasteBufferedElementsTo(string destination);

        void MoveSelectedElementsTo(string destination);

        void CreateNewDirectory(string name);

        void CreateNewTxtFile(string name);

        void Rename(string newName);

        void Read();

        void NavigationHistoryGoBack();

        void WriteToBuffer(int type = 0);
    }
}
