using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dunderscript
{


    class Program : CommandFactory
    {
 
        
        
        /// <summary>
        /// system root node
        /// </summary>
        public static Node root;

        /// <summary>
        /// navigation object
        /// </summary>
        public static Navigate nav;
        
        static void Main(string[] args)
        {
            nav = new Navigate();
            root = new Node();

            root.name = "$";
            nav.current = root;

            bool keeprunning = true;

            //build a node object
            string input;
            string output;
            while(keeprunning)
            {
                var breadcrumb = (nav.path!=delim.ToString()) ? 
                    nav.path  + nav.current.name : nav.current.name; 
                Console.Write( breadcrumb+ ">");
                input = Console.ReadLine();

                string cmd="";
                string arg="";

                ////
                ////  TODO: A better way to parse input so that 
                ////  make foo=bar , 
                ////  make foo={$.home.desk.lamp}  {}, used as value reference
                ////  
                ////  there should be a way to reference math functions
                ////  
                
                try
                {
                
                    //parse the input
                    var parser = new InputParser();
                    parser.Parse(input);
                    parser.NodeActionCollection(nav.current,
                        parser.NodeActionCollectionArgs.ToArray());
                

                    //precursor commadns with an arg
                    //cmd = input.Split(' ')[0];
                    //arg = input.Split(' ')[1];
                }
                catch(Exception ex)
                {
                    //commands with no args
                    cmd = input;
                    arg = "";

                    //check if its an inline command
                    //if(input.Contains(set))
                    //{
                    //    cmd=set;
                    //    arg = input;
                    //}
        
                }
                


                switch (cmd)
                {
                    //case make:
                    //            //build a new node
                    //            AddNode(nav.current,arg);
                    //            break;
                    //case viewjson:
                    //            //show node tree
                    //            output = ShowNode(nav.current,true);
                    //            break;
                    //case view:  
                    //            //view node tree
                    //            output = ShowNode(nav.current);
                    //            break;
                    //case set:
                    //            //assign
                    //            SetNode(nav.current, arg);
                    //            break;
                    case clear:
                                System.Console.Clear();
                                break;
                    case stepin:
                                nav.MoveIn(arg);
                                break;
                    case stepout:
                                nav.MoveOut(arg);
                                break;
                    case exit:
                    case "q":
                    case "Q":
                                keeprunning = false;
                                break;
                    default: break;
                }

            }
            
        }

        //private static void SetNode(Node master, string arg)
        //{
        //    try
        //    {
        //        var g = arg.Split(set.ToCharArray()[0]);
        //        var nodeName = g[0];
        //        var nodeValue = g[1];

        //        var node = master.children.FirstOrDefault(n => n.name == nodeName);
        //        if (node == null) { return; }
        //        var index = master.children.IndexOf(node);
        //        node.value = nodeValue;
        //        if (nodeValue == "null") { node.value = null; }
        //        master.children.RemoveAt(index);
        //        master.children.Insert(index, node);
        //    }
        //    catch
        //    {
        //        //error parsing input.
        //    }
        //}

        //private static string ShowNode(Node node, bool verbose = false)
        //{
        //    string output="";

        //    if (verbose == true)
        //    {
        //        //stringiyfy the object into a 
        //        //json string and show it on the screen
        //        output = Newtonsoft.Json.JsonConvert.SerializeObject(node, Formatting.Indented);
                
        //    }
        //    else
        //    {
        //        //read through the json output and print the children

        //        foreach (var child in node.children)
        //        {
        //            //only show colon when value present
        //            var childvalue =(child.value!=null)?":"+child.value:child.value;

        //            output += string.Format("{0}{1}\n", child.name,childvalue);
        //        }

        //    }

        //    Console.WriteLine(output);
        //    return output;
        //}
        
        //private static void AddNode(Node master,string arg)
        //{
        //    var node =  new Node();
        //    node.name = arg;
        //    master.children.Add(node);
        //}

    }
}
