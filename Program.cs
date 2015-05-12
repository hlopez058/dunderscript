
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace dunderscript
    {
        /// <summary>
        /// Linked List object
        /// </summary>
        public class Node
        {

            //this will hold the value field
            public object value { get; set; }

            //the name of the object as 
            public string key { get; set; }

            public string parentKey { get; set; }
        }

        class Program
        {
            static void Main(string[] args)
            {
                List<Node> nlist = new List<Node>();

                //ask for the input
                var stdin = new stdInput();
                var path = "";


                while (stdin.command != "Exit")
                {

                    stdin = stdIn(path);
                    
                    var output = "";

                    //Check if the command is a custom command
                    bool iscust;
                    try
                    {Commands.CustomCommands.Exists(x => x.Name == stdin.command);
                     iscust = true;}
                    catch{ iscust = false;}

                    //Custom Command
                    if(iscust)
                    {
                        CustomCommands(ref path, stdin, ref nlist, ref output);
                    }
                    else
                    // Native Command
                    {
                        NativeCommands(ref path, stdin, ref nlist, ref output);
                    }
                    
                    //write the command output
                    Console.WriteLine(output);
                }
            }

            private static void CustomCommands(ref string path,stdInput stdin, ref List<Node> nlist, ref string output)
            {
                //perform the command
                var cmd = Commands.CustomCommands.Find(x => x.Name == stdin.command);
                //Commands.CustomCommandInterpreter.Run(cmd);
                output = "Not implemented";
            }



            /// <summary>
            /// 
            /// </summary>
            /// <param name="path">current node command was called in</param>
            /// <param name="stdin">input text</param>
            /// <param name="nlist">current node linked/list instance</param>
            /// <param name="output">output text</param>
            private static void NativeCommands(ref string path,stdInput stdin,ref List<Node> nlist, ref string output)
            {
                
                //dosomehting with input
                switch (stdin.command)
                {
                    case "system.load":
                        path = Commands.Load(path, stdin.args[0], ref nlist, ref output);
                        break;
                    case "system.save":
                        path = Commands.Save(path, stdin.args[0], ref nlist, ref output);
                        break;
                    case "system.clear":
                        Console.Clear();
                        break;
                    case "system.create":
                        path = Commands.Create(path, stdin.args[0], ref nlist, ref output);
                        break;
                    case "system.copy":
                        path = Commands.Copy(path, stdin.args[0], ref nlist, ref output);
                        break;
                    case "system.paste":
                        path = Commands.Paste(path, stdin.args[0], ref nlist, ref output);
                        break;
                    case "system.enter":
                        path = Commands.Enter(path, stdin.args[0], nlist, ref output);
                        break;
                    case "system.exit":
                        path = Commands.Exit(path, stdin.args[0], ref output);
                        break;
                    case "system.search":
                        Commands.Search(path, stdin.args[0], nlist, ref output);
                        break;
                    case "system.delete":
                        Commands.Delete(path, stdin.args[0], nlist, ref output);
                        break;
                    case "?":
                        Commands.Help(path, stdin.args[0], nlist);
                        //Commands.LoadCustomCommands();
                        break;
                    default:
                        Console.WriteLine("Unrecognized command.");

                        break;
                }
                    
            }

            class stdInput
            {
                public string command { get; set; }
                public List<string> args { get; set; }
            }

            static stdInput stdIn(string path)
            {
                Console.Write("\n{0}> ", path);
                var input = Console.ReadLine();

                var command = "";
                List<string> args = new List<string>();


                //look for command with arguments
                if (input.Contains(' '))
                {

                    try
                    {
                        var inputs = input.Split(' ');
                        //command will be the first one
                        command = inputs[0];
                        for (int i = 1; i < inputs.Length; i++)
                        {
                            args.Add(inputs[i]);
                        }
                    }
                    catch
                    {
                        //could not split 
                    }
                }
                else
                {
                    //possibly a single command was entered no parameters
                    command = input;
                    args.Add("");
                }

                var stdinput = new stdInput();
                stdinput.args = args;
                stdinput.command = command;
                return stdinput;
            }


        }


    }
