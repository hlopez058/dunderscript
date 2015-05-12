using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dunderscript
{

    static class Commands
    {
        static public char delim = '.'; //path delimiter

        private class ActiveObject
        {
            public string parentKey { get; set; }
            public string key { get; set; }
        }

        private static ActiveObject ParseArgAsObject(string path, string arg, List<Node> nlist)
        {
            //var isinlist = nlist.Exists(x=>
            //                x.key == nlist.Find(o => 
            //                    o.parentKey == path).key);

            //world> enter home
            //world.home>

            //combine the current path and hte one that 
            //is in the argument
            var opath = path + delim + arg;

            //split the paths
            var paths = opath.Split(delim).ToList();

            //save the key as the last parameter in the path
            var key = paths.Last();

            //create the parent key by removing the last parameter 
            paths.RemoveAt(paths.Count - 1);
            var parentkey = "";

            //recreate the path string without hte "key"
            foreach (var p in paths)
            {
                parentkey += p + delim;
            }

            parentkey = parentkey.Remove(parentkey.Length - 1);
            if (parentkey == delim.ToString()) { parentkey = ""; }

            //the parent key and the key are ready

            //search for an object in the linked list that
            //has the parentkey and the key 
            var isinlist = nlist.Exists(x => x.parentKey == parentkey && x.key == key);

            //if it exists return the new path location
            if (isinlist)
            {
                var activeobject = new ActiveObject();
                activeobject.parentKey = parentkey;
                activeobject.key = key;
                return activeobject;

            }
            else
            {
                throw (new Exception());
            }
        }

        /// <summary>
        /// Enters an object and changes current location
        /// </summary>
        /// <param name="path"></param>
        /// <param name="arg"></param>
        /// <param name="nlist"></param>
        /// <returns></returns>
        public static string Enter(string path, string arg, List<Node> nlist,ref string output)
        {

            //navigate to the paths object
            try
            {

                var activeobj = ParseArgAsObject(path, arg, nlist);
                if (activeobj.parentKey != "")
                {
                    return activeobj.parentKey + delim + activeobj.key;
                }
                else
                {
                    return activeobj.key;
                }

            }
            catch
            {

                output = String.Format("Could not find '{0}'", arg);
                return path;
            }


        }

        /// <summary>
        /// Exits the current object and changes location
        /// </summary>
        /// <param name="path"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static string Exit(string path, string arg,ref string output)
        {
            try
            {

                //split the path 
                var paths = path.Split(delim).ToList();
                //find the current path that will be exited
                var indx = paths.IndexOf(arg);

                //remove every object in the path after the identified one
                var max = paths.Count;
                var opath = "";
                if (indx == 0)
                {
                    //exit to upper level
                    opath = "";
                }
                else
                {
                    for (int i = max - 1; i >= indx; i--)
                    {

                        paths.RemoveAt(i);
                    }

                    opath = "";
                    foreach (var p in paths)
                    {
                        if (p != "")
                        {
                            opath += p + delim;
                        }

                    }

                    //remove  trailing delimiter
                    opath = opath.Remove(opath.Length - 1);
                }

                output = String.Format("Exited {0}", arg);
                return opath;
            }
            catch
            {
                output = String.Format("Not inside of {0}", arg);
                return path;
            }

        }

        /// <summary>
        /// Creates an object under the current object
        /// </summary>
        /// <param name="path"></param>
        /// <param name="arg"></param>
        /// <param name="nlist"></param>
        /// <returns></returns>
        public static string Create(string path, string arg, ref List<Node> nlist,ref string output)
        {

            try
            {

                var delequal = ':';
                var node = new Node();

                if (arg.Contains(delequal))
                {
                    // create a node and assign it a property
                    var args = arg.Split(delequal);
                    node.key = args[0].Trim();
                    node.value = args[1].Trim();
                    node.parentKey = path.Trim();
                    nlist.Add(node);

                }
                else
                {
                    //the argument has no value so leave it empty
                    node.key = arg.Trim();
                    node.value = null;
                    node.parentKey = path.Trim();
                    nlist.Add(node);

                }


                output = String.Format("Created {0}", arg);
                return path;
            }
            catch (Exception ex)
            {
                output = String.Format("Could not create '{0}' - {1}", arg, ex.Message);
                return path;
            }

        }

        /// <summary>
        /// Prints a list of all items within a given object
        /// </summary>
        /// <param name="path"></param>
        /// <param name="arg"></param>
        /// <param name="nlist"></param>
        /// <returns></returns>
        public static string Search(string path, string arg, List<Node> nlist, ref string output)
        {
            //navigate to the paths object
            try
            {
                ActiveObject activeobj;
                string aobj_child_parentkey;
                if (arg != "")
                {
                    activeobj = ParseArgAsObject(path, arg, nlist);
                    aobj_child_parentkey = activeobj.parentKey + delim + activeobj.key;

                }
                else
                {
                    aobj_child_parentkey = path;

                }
                //read the objects under the active object


                var children = nlist.FindAll(node => node.parentKey == aobj_child_parentkey);

                //print hte children
                foreach (var c in children)
                {
                    //check if the child has children
                    var objchar = "";
                    if (nlist.Exists(node => node.parentKey == (c.parentKey + delim + c.key)))
                    {
                        objchar = ".";
                    }
                    if (c.value != null)
                    {
                        //check if the child has children


                        if (c.value.GetType() == typeof(string))
                        {
                            output = String.Format(" {0}:{1}{2}", c.key, c.value, objchar);
                        }
                        else if (c.value.GetType() == typeof(int))
                        {
                            output = String.Format(" {0}:{1}{2}", c.key, c.value, objchar);
                        }
                        else
                        {
                            output = String.Format(" {0}.", c.key, c.value);
                        }
                    }
                    else
                    {
                        output = String.Format(" {0}{1}", c.key, objchar);
                    }
                }

                return path;
            }
            catch
            {

                output = String.Format("Could not find '{0}'", arg);
                return path;
            }

        }
        
        /// <summary>
        /// Delete object by keyword or delete object with given 
        /// key and value 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="arg"></param>
        /// <param name="nlist"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public static string Delete(string path, string arg, List<Node> nlist, ref string output)
        {
            try
            {

                var delequal = ':';
                var node = new Node();

                if (arg.Contains(delequal))
                {
                    // create a node and assign it a property
                    var args = arg.Split(delequal);
                    var key = args[0].Trim();
                    var value = args[1].Trim();
                    
                    //search for a node with parentkey equal to path
                    var children = nlist.FindAll(x => x.parentKey == path.Trim());

                    //remove all children
                    foreach (var child in children)
                    {
                        if (child.key == key && child.value == value)
                        {
                            nlist.Remove(child);
                        }
                    }
                }
                else
                {
                    //search for a node with parentkey equal to path plus current object
                    var children = nlist.FindAll(x => x.parentKey == path.Trim());
                    
                    //remove all children
                    foreach (var child in children)
                    {
                        if(child.key == arg.Trim())
                        {
                            nlist.Remove(child);
                        }
                    }

                }


                output = String.Format("Deleted {0}", arg);
                return path;
            }
            catch (Exception ex)
            {
                output = String.Format("Could not create '{0}' - {1}", arg, ex.Message);
                return path;
            }
        }

        /// <summary>
        /// Reads the local help file for displaying
        /// </summary>
        /// <param name="path"></param>
        /// <param name="p"></param>
        /// <param name="nlist"></param>
        public static void Help(string path, string p, List<Node> nlist)
        {
            string line;
            
            string help = System.Configuration.ConfigurationManager.AppSettings["Help"];
            // Read the file and display it line by line.
            System.IO.StreamReader file =
               new System.IO.StreamReader(System.Windows.Forms.Application.StartupPath + "\\" + help);
            while ((line = file.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }

            file.Close();
        }

        //---

        public class CustomCommand
        {
            public string Name { get; set; }
            public List<string> CommandLine = new List<string>();
            public string Description { get; set; }
        }

        public static List<CustomCommand> CustomCommands { get; set; }

        public static void LoadCustomCommands()
        {
            string cuscmdfile = System.Configuration.ConfigurationManager.AppSettings["CustomCommands"];

            var myCustomCommand = new CustomCommand();
            var commands = new List<CustomCommand>();
            commands.Add(myCustomCommand);
            commands.Add(myCustomCommand);

            // Open the file to read from. 
            string readJson = System.IO.File.ReadAllText(
                System.Windows.Forms.Application.StartupPath + "\\" + cuscmdfile);

            List<CustomCommand> usercmds = JsonConvert.DeserializeObject<List<CustomCommand>>(readJson);

            CustomCommands = usercmds;
            
            Console.WriteLine("Loaded '{0}'\n", cuscmdfile);
            usercmds.ForEach(delegate(CustomCommand cmd){ Console.WriteLine("Added '{0}'\n",cmd.Name);});
        }


        public class CustomCommandInterpreter
        {
            //read the commands that have been created in the commands.json file
            //allow the commands to have internal functions
       
            internal static void Run(CustomCommand cmd)
            {
                //read through the commandline
                foreach (var command in cmd.CommandLine)
                {
                    var parms = command.Split(' ');
                    
                }
            }
        }


        //static string for copy key
        static string copybufferID = "system.copy::";

        /// <summary>
        /// Create a new node "system.copybuffer" and add a copy of the current
        /// object to the node. If there is a node existing it is removed.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="arg"></param>
        /// <param name="nlist"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        internal static string Copy(string path, string arg, ref List<Node> nlist, ref string output)
        {
            //determine if any system.copybuffer nodes exist in the nodelist. 
            if (nlist.Exists(x => x.key.Contains(copybufferID)))
            {
                //remove the node
                var index = nlist.FindIndex(x=>x.key.Contains(copybufferID));

                nlist.RemoveAt(index);
            }

            //copy the current node 
            
            //find the current node in the nodelist
            var currentNode = nlist.Find(x => x.parentKey == path);
            
            //find all the children

            var copiedNode = new Node();

            copiedNode =  currentNode;

            copiedNode.key = "system.copy::" + currentNode.key;

            nlist.Add(copiedNode);

            output = String.Format("{0} has been copied\n", copiedNode.key);

            return output;
        }

        internal static string Paste(string path, string p, ref List<Node> nlist, ref string output)
        {
            
           
            //determine if any system.copybuffer nodes exist in the nodelist. 
            if (nlist.Exists(x => x.key.Contains(copybufferID)))
            {
                //remove the node
               // var index = nlist.FindIndex(x => x.key.Contains(copybufferID));

                //find the copied node
                //var copiedNode = new Node();

                //copiedNode = nlist[index];
              
                //find the curent node location

            }

            return "na";
        }

        internal static string Load(string path, string p, ref List<Node> nlist, ref string output)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Save the entire nodelist to a JSON file. 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="p"></param>
        /// <param name="nlist"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        internal static string Save(string path, string p, ref List<Node> nlist, ref string output)
        {
            //convert nodelist to parent/child objects
            //json serialize the objects
            
           

            return "";

        }
    }
}
