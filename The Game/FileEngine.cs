using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



public interface IIFileEngine
{
    static abstract string GetSavePath();
    void VerifyPaths();
    void PrintSaveFiles();
    void EnsureDataFilesExist();
    void CopyIfMissing(string relativePathFromProject, string targetPath);
}
