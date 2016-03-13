using Orion.Core.Module;
using System.IO;
using System.IO.Compression;

namespace Orion.Platform.Win32
{
    public class ModuleLoader : IPlatformModuleLoader
    {
        public string StagingDirectory { get; internal set; }
        public string FileName { get; internal set; }
        public ModuleAccessMode AccessMode { get; internal set; }

        public ModuleLoader(ModuleAccessMode accessMode, string stagingDir)
        {
            StagingDirectory = stagingDir;
            AccessMode = accessMode;
        }

        public void Prepare(string modulePath)
        {
            Prepare();

            FileName = modulePath;

            if (AccessMode == ModuleAccessMode.Disk)
                ZipFile.ExtractToDirectory(FileName, StagingDirectory);
        }

        public void Prepare()
        {
            if (!Directory.Exists(StagingDirectory))
                Directory.CreateDirectory(StagingDirectory);
        }

        public byte[] GetEntryAsBytes(string entryPath)
        {
            byte[] entryData = null;

            if (AccessMode == ModuleAccessMode.Disk)
            {
                try
                {
                    using (var stream = new FileStream(Path.Combine(StagingDirectory, entryPath), FileMode.Open))
                    {
                        using (BinaryReader binReader = new BinaryReader(stream))
                        {
                            entryData = Orion.Core.Utilities.ReadAllBytes(binReader);
                        }
                    }
                }
                catch (IOException)
                {
                    //MessageBox.Show("Could not locate module entry [" + entryPath + "].", "Error",
                    //    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                try
                {
                    using (ZipArchive archive = new ZipArchive(new FileStream(FileName, FileMode.Open), ZipArchiveMode.Read))
                    {
                        ZipArchiveEntry zipArchiveEntry = archive.GetEntry(entryPath.Replace("\\", "/"));
                        using (var stream = zipArchiveEntry.Open())
                        {
                            using (BinaryReader binReader = new BinaryReader(stream))
                            {
                                entryData = Orion.Core.Utilities.ReadAllBytes(binReader);
                            }
                        }
                    }
                }
                catch (IOException)
                {
                    //MessageBox.Show("Could not locate module entry [" + entryPath + "].", "Error",
                    //    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return entryData;
        }

        public string GetEntryAsString(string entryPath)
        {
            string data = string.Empty;

            if (AccessMode == ModuleAccessMode.Disk)
            {
                try
                {
                    using (var stream = new FileStream(Path.Combine(StagingDirectory, entryPath), FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            data = reader.ReadToEnd();
                        }
                    }
                }
                catch (IOException)
                {
                    //MessageBox.Show("Could not locate module entry [" + entryPath + "].", "Error",
                    //    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                try
                {
                    using (ZipArchive archive = new ZipArchive(new FileStream(FileName, FileMode.Open), ZipArchiveMode.Read))
                    {
                        ZipArchiveEntry zipArchiveEntry = archive.GetEntry(entryPath.Replace("\\", "/"));
                        using (var stream = zipArchiveEntry.Open())
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                data = reader.ReadToEnd();
                            }
                        }
                    }
                }
                catch (IOException)
                {
                    //MessageBox.Show("Could not locate module entry [" + entryPath + "].", "Error",
                    //    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return data;
        }

        public void CloseStream(Stream stream)
        {
            stream.Close();
        }

        public string CopyDatabaseToStagingDir(string filename, byte[] database)
        {
            string databasePath = string.Empty;

            try
            {
                string absFileName = Path.Combine(StagingDirectory, filename);

                if (!Directory.Exists(Path.GetDirectoryName(absFileName)))
                    Directory.CreateDirectory(Path.GetDirectoryName(absFileName));

                databasePath = StagingDirectory + filename;

                if (File.Exists(databasePath))
                    File.Delete(databasePath);

                // save database to temp location
                FileStream fs = new FileStream(databasePath, FileMode.Create);
                BinaryWriter writer = new BinaryWriter(fs);
                writer.Write(database);
                writer.Close();
                fs.Close();

            }
            catch(IOException)
            {
                //MessageBox.Show("Could not load database out of the module.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }

            return databasePath;
        }
    }
}
