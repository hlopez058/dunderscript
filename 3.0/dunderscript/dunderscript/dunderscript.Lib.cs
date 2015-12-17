using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dunderscript
{
   public class Lib { }

   public class ObjectLibrary 
    {
        public readonly ICollection<scriptFile> _ScriptFiles = new ObservableCollection<scriptFile>();

        public ICollection<scriptFile> ScriptFiles 
        {
            get
            {
                return _ScriptFiles;
            }
        }

        public void LoadFiles()
        {
            //open directory
            //search files with extensions
            var dir = System.AppDomain.CurrentDomain.BaseDirectory;
            var ext = new List<string> { ".dnd", ".js" };
            var myFiles = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories)
                .Where(s => ext.Any(e => s.EndsWith(e)));
            foreach (var file in myFiles)
            {
                var name = file.Remove(0, file.LastIndexOf('\\') + 1);
                _ScriptFiles.Add(new scriptFile(file, name));
            }
        }

        public ObjectLibrary()
        {  
            

            
        }

         
    }
   public class scriptFile
    {
        string _name;
        public string name { get { return _name; } set { _name = value; } }

        string _path;
        public string path { get { return _path; } set { _path = value; } }

        public scriptFile(string path,string name)
        {
            this._name = name;
            this._path = path;
        }
    }
}
