using System.IO;

namespace Orion.Core.Module
{
    public interface IPlatformModuleLoader
    {
        string StagingDirectory { get; }
        string FileName { get; }
        ModuleAccessMode AccessMode { get; }

        void Prepare();
        void Prepare(string modulePath);
        byte[] GetEntryAsBytes(string entryPath);
        string GetEntryAsString(string entryPath);
        void CloseStream(Stream stream);
        string CopyDatabaseToStagingDir(string filename, byte[] database);
    }
}
