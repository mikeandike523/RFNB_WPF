using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;

namespace RFNB_UWP
{
    internal class Notebook
    {

        private int _currentPage = 0;
        private int _currentSection = 0;
        string _name;

        private DictionaryWithExpiration<int, StrokeCollection> _pageData;

        string _root; //Absolute path to notebook root

        public int GetCurrentPageNumber() {
            return _currentPage;
        }

        public bool SwitchToPreviousPage() {
            _currentPage--;
            if (_currentPage < 0)
            {
                _currentPage = 0;
                return false;
            }
            return true;
        }

        public bool SwitchToNextPage()
        {
            _currentPage++;
            return true;
        }

        public void WriteStrokesToNotebook(int? pageNumber, StrokeCollection strokes,bool syncWithFilesystem = true) {
            if (pageNumber == null) {
                pageNumber = _currentPage;
            }
            _pageData[pageNumber ?? _currentPage] = strokes;
            if (syncWithFilesystem) { 
                SaveNotebookPageStrokes(pageNumber);
            }
        }

        public StrokeCollection ReadStrokesFromNotebook(int? pageNumber, bool syncFromFilesystem = true) {
            if (pageNumber == null){
                pageNumber = _currentPage;
            }
            return _pageData[pageNumber ?? _currentPage];
        }

        public StrokeCollection? LoadNotebookPageStrokes(int? pageNumber) {
            if (pageNumber == null)
            {
                pageNumber = _currentPage;
            }
            string filepath = Utilities.JoinPaths(_root, _currentSection.ToString(), pageNumber.ToString());
            if (File.Exists(filepath))
                return Utilities.LoadStrokeCollectionFromFile(filepath);
            return null;
        }

        private void SaveNotebookPageStrokes(int? pageNumber)
        {
            if (pageNumber == null)
            {
                pageNumber = _currentPage;
            }
            string filepath = Utilities.JoinPaths(_root, _currentSection.ToString(), pageNumber.ToString())+".isf";
            Utilities.SaveStrokeCollectionToFile(filepath,_pageData[pageNumber ?? _currentPage]);
        }

        public StrokeCollection StrokeFileLoader(int pageNumber) {
            string filepath = Utilities.GetFilePath(Utilities.JoinPaths(_name, _currentSection.ToString(), pageNumber.ToString()))+".isf";
            return Utilities.LoadStrokeCollectionFromFile(filepath);
        }

        public Notebook(string name, bool clearDataOnOpen = false) {
            _name = name;
            string notebookDirectory = Utilities.GetFilePath(Utilities.JoinPaths("Notebooks", name));
            if (Directory.Exists(notebookDirectory)) {
                if (clearDataOnOpen) {
                    Directory.Delete(notebookDirectory);
                    Directory.CreateDirectory(notebookDirectory);
                }
            }
            string sectionDirectory = Utilities.JoinPaths(notebookDirectory, _currentSection.ToString());
            if (!Directory.Exists(sectionDirectory)) { 
                Directory.CreateDirectory(sectionDirectory);
            }
            _pageData = new DictionaryWithExpiration<int, StrokeCollection>(16,StrokeFileLoader);
            _root = notebookDirectory;
        }

    }
}
