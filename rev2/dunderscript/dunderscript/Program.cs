using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dunderscript
{
    class Program
    {
        public class Node
        {
            public string name;
            public List<Node> children =  new List<Node>();
        }

        public class Pointer
        {
            //find the active node.
            //set the active node by setting 
            //the pointer to the node.
            //
        }

        public static Node root;

        static void Main(string[] args)
        {

            root = new Node();
            root.name = ".";

            //build a node boject
            string input;
            string output;

            bool keeprunning = true;

            while(keeprunning)
            {
                Console.Write(">");
                input = Console.ReadLine();

                string cmd;
                string arg;

                try
                {
                    cmd = input.Split(' ')[0];

                    arg = input.Split(' ')[1];
                }
                catch
                {
                    cmd = input;
                    arg = "";
                }
                
                
                switch (cmd)
                {
                    case "create":
                                //build a new node
                                AddNode(root,arg);
                                break;
                    case "show":
                                //show node tree
                                output = ShowNode(root);
                                break;
                    case "clear":
                                System.Console.Clear();
                                break;
                    case "exit":
                    case "q":
                    case "Q":
                                keeprunning = false;
                                break;
                    default: break;
                }

            }
            
        }

        private static string ShowNode(Node master)
        {
            //stringiyfy the object into a 
            //json string and show it on the screen

            string output = Newtonsoft.Json.JsonConvert.SerializeObject(master,Formatting.Indented);

            Console.WriteLine(output);
            return output;
        }

        private static void AddNode(Node master,string arg)
        {
            var node =  new Node();
            node.name = arg;
            master.children.Add(node);
        }

       
    }
}
