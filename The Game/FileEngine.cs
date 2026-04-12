using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Game;




public interface IIFileEngine
{
    static abstract string GetSavePath();
    void VerifyPaths();
    void PrintSaveFiles();
    void EnsureDataFilesExist();
    void CopyIfMissing(string relativePathFromProject, string targetPath);
}
