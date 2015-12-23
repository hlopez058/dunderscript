using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dunderscript
{

    public class FileManager
    {
        public readonly ICollection<scriptFile> _ScriptFiles = new ObservableCollection<scriptFile>();

        public ICollection<scriptFile> ScriptFiles
        {
            get
            {
                return _ScriptFiles;
            }
        }

        public void LoadFiles(string dir, List<string> ext)
        {
            //open directory
            //search files with extensions
            var myFiles = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories)
                .Where(s => ext.Any(e => s.EndsWith(e)));
            foreach (var file in myFiles)
            {
                // Open the file to read from.
                string data = File.ReadAllText(file);

                var name = file.Remove(0, file.LastIndexOf('\\') + 1);
                
                _ScriptFiles.Add(new scriptFile(file, name,data));
            }
        }
        
    }

    public class scriptFile
    {
        string _name;
        public string name { get { return _name; } set { _name = value; } }

        string _path;
        public string path { get { return _path; } set { _path = value; } }

        string _data;
        public string data { get { return _data; } set { _data = value; } }


        public scriptFile(string path, string name, string data)
        {
            this._name = name;
            this._path = path;
            this._data= data;
        }

    }
}
