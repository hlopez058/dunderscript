using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dunderscript
{
    public class Node
    {
        public string name;
        public string value;
        public List<Node> children = new List<Node>();
    }

    /// <summary>
    /// Actions, possible ways to manipulate a node/directory
    /// </summary>
    public class NodeAction
    {
        /// <summary>
        /// Delegate for node actions
        /// </summary>
        /// <param name="master"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        public delegate string Del(Node master, object[] arg);

        #region node actions
        /// <summary>
        /// Updates the value of the current node
        /// </summary>
        /// <param name="master"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static string SetNode(Node master, object[] arg)
        {
            try
            {
                var argg = (string)arg[0];
                var g = argg.Split(CommandFactory.set.name.ToCharArray()[0]);
                var nodeName = g[0];
                var nodeValue = g[1];

                var node = master.children.FirstOrDefault(n => n.name == nodeName);
                if (node == null) { return ""; }
                var index = master.children.IndexOf(node);
                node.value = nodeValue;
                if (nodeValue == "null") { node.value = null; }
                master.children.RemoveAt(index);
                master.children.Insert(index, node);
            }
            catch
            {
                //error parsing input.
            }
            return "";
        }

        /// <summary>
        /// Print the node children to the screen
        /// </summary>
        /// <param name="node"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static string ShowNode(Node node, object[] arg)
        {

            string output = "";
            try
            {
                if (arg != null)
                {
                    if ((string)arg[0] != "")
                    {
                        //search current node for child with name of argument
                        //show the child node or if not found show the root node
                        var pony = node.children.FirstOrDefault(n => n.name == (string)arg[0]);
                        if (pony == null) { throw new ArgumentException("Could not find, {0}.", (string)arg[0]); }
                        else { ShowNode(pony, null); }
                    }
                }
                else
                {
                    //read through the json output and print the children
                    foreach (var child in node.children)
                    {
                        //only show colon when value present
                        var childvalue = (child.value != null) ? ":" + child.value : child.value;
                        output += string.Format("{0}{1}\n", child.name, childvalue);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine(output);
            return output;
        }

        /// <summary>
        /// Print the node children and values to screen 
        /// as JSON formatted output.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static string ShowNodeJson(Node node, object[] arg)
        {

            string output = "";
            try
            {
                //stringiyfy the object into a 
                //json string and show it on the screen
                output = Newtonsoft.Json.JsonConvert.SerializeObject(node, Formatting.Indented);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine(output);
            return output;
        }

        /// <summary>
        /// Add a new node at the current node location.
        /// </summary>
        /// <param name="master"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static string AddNode(Node master, object[] arg)
        {
            try
            {
                var argg = (string)arg[0];
                var node = new Node();
                node.name = argg;
                master.children.Add(node);
            }
            catch (Exception ex) 
            { 
                Console.WriteLine(ex.Message);
            }

            return "";
        }
        #endregion
    }

    public class Command
    {
        public string name { get; set; }
        public NodeAction.Del cmdaction { get; set; }
    }

    public class CommandFactory
    {
        public const char delim = '.';//delimiter for navigation
        public const char argdelim = ' ';//delimiter for arguments

        //--------------------------------------------------------------
        //  System Commands
        //--------------------------------------------------------------
        // Searchable index of commands
        public List<Command> sys_commands =
            new List<Command>()
        {
            make,
            view,
            viewjson
        };
        // command definitions
        /// <summary>
        /// "make" : Creates a new node under the current node
        /// </summary>
        public static Command make = new Command() { name = "mk", cmdaction = NodeAction.AddNode };
        /// <summary>
        /// "view" : Shows all nodes under current node
        /// </summary>
        public static Command view = new Command() { name = "view", cmdaction = NodeAction.ShowNode };
        /// <summary>
        /// "view json": Shows all nodes but exposed the json format
        /// </summary>
        public static Command viewjson = new Command() { name = "view -j", cmdaction = NodeAction.ShowNodeJson };
        //--------------------------------------------------------------

        //--------------------------------------------------------------
        //  In-line Operators
        //--------------------------------------------------------------
        // Searchable index of inline opertions
        public List<Command> sys_operators =
            new List<Command>()
        {
            set
        };
        /// <summary>
        /// "=" : assign a value to a node
        /// </summary>
        public static Command set = new Command() { name = "=", cmdaction = NodeAction.SetNode };


        //list of navigation commands
        public List<string> nav_commands = new List<string>()
        {
            stepin,
            stepout
        };
        //nav commands
        public const string stepin = "cd";
        public const string stepout = "cd..";

        //to be completed
        //cut
        //copy
        //move
        //delete
        //paste
        //empty
        //search
        //load
        //load.at
        //save
        //save.at
        //rename
        //system 
        public const string clear = "clear";
        public const string exit = "exit";

    }

    public class InputParser
    {
        public string Input { get; set; }
        public NodeAction.Del NodeActionCollection { get; set; }
        public List<Object> NodeActionCollectionArgs { get; set; }
        public CommandFactory CmdFactory;
        public InputParser()
        {
            //clear the collection
            NodeActionCollection = null;
            NodeActionCollectionArgs = new List<object>();
            CmdFactory = new CommandFactory();
        }

        /// <summary>
        /// Convert input into a collection of actions based on
        /// commands and inline operators found within the string.
        /// </summary>
        /// <param name="input"></param>
        public void Parse(string input)
        {
            //take the input and apply a filter to it
            //check if filter comes back with a command
            //if command exists then add to action collection
            var syscmdarg = "";
            var syscmd = SysCmdFilter(input, out syscmdarg);
            if (syscmd != null)
            {
                //save the system command action
                NodeActionCollection += syscmd.cmdaction;

                //check the syscmdarg for inline parameters
                var op1 = ""; var op2 = "";
                var opcmd = SysOpFilter(syscmdarg, out op1, out op2);
                if (opcmd != null)
                {
                    NodeActionCollection += opcmd.cmdaction;
                    //set argument for sysaction and for op action
                    NodeActionCollectionArgs.Add(op1);
                    NodeActionCollectionArgs.Add(op2);
                }
                else
                {
                    //set the argument for the sys action
                    NodeActionCollectionArgs.Add(op1);
                }

            }
        }


        private Command SysCmdFilter(string msg, out string arg)
        {
            foreach (var cmdo in CmdFactory.sys_commands)
            {
                var cmd = cmdo.name;
                //syscommands must have argument delim after it
                var p = msg.IndexOf(cmd + CommandFactory.argdelim);
                if (p == 0)
                {
                    //the rest of the msg is an argument
                    var cmdmsg = cmd + CommandFactory.argdelim;
                    arg = msg.Remove(0, cmdmsg.Length);

                    //found command
                    return cmdo;
                }
            }
            //no command found return the whole message
            arg = msg;
            return null;
        }

        private Command SysOpFilter(string msg, out string arg1, out string arg2)
        {
            try
            {
                foreach (var cmdo in CmdFactory.sys_operators)
                {
                    var cmd = cmdo.name;
                    //operators are inbetween two arguments
                    var p = msg.IndexOf(cmd);
                    if (p != 0)
                    {
                        //first arg is on theleft side of the operator
                        arg1 = msg.Substring(0, p);
                        arg2 = msg.Substring(p + cmd.Length, msg.Length);
                        //found command
                        return cmdo;
                    }
                }
            }
            catch
            {

            }
            finally
            {
                //no command found return the whole message
                arg1 = msg;
                arg2 = "";

            } return null;
        }

        private string NavCmdFilter(string msg, out string arg)
        {
            foreach (var cmdo in CmdFactory.sys_commands)
            {
                var cmd = cmdo.name;
                //nav commands may not need arguments
                if (msg.Contains(cmd + CommandFactory.argdelim))
                {
                    //the command contains an argument
                    var p = msg.IndexOf(cmd + CommandFactory.argdelim);
                    if (p == 0)
                    {
                        //the rest of the msg is an argument
                        var cmdmsg = cmd + CommandFactory.argdelim;
                        arg = msg.Remove(0, cmdmsg.Length);

                        //found command
                        return cmd;
                    }

                }
                else if (msg.Contains(cmd))
                {
                    //the command doesnt contain an argument 
                    //but it exists.
                    arg = "";
                    return cmd;
                }


            }
            //no command found return the whole message
            arg = "";
            return "";
        }

    }

}
