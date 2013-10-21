using System;

namespace file_manager_test_app.Model
{
    interface IColumnOperations
    {
        void Select(int[] elements);

        void DeleteSelectedElements();

        void CopySelectedElementsTo(string destination);

        void MoveSelectedElementsTo(string destination);

        void CreateNewDirectory(string name);

        void CreateNewTxtFile(string name);

        void ClearSelection();

        void Rename(string newName);
    }
}
