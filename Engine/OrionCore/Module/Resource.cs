// --------------------------------------------------------------
// This source code file is part of the Orion Engine.
// Developed by Michael Kyle - 2016
// --------------------------------------------------------------

namespace Orion.Core.Module
{
    public class Resource
    {
        public string Reference { get; set; }
        public string Path { get; set; }

        public Resource()
        {
            this.Reference = string.Empty;
            this.Path = string.Empty;
        }
        
        public Resource(string reference, string path)
        {
            this.Reference = reference;
            this.Path = path;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
