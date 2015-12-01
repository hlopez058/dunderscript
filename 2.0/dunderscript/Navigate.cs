using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dunderscript
{
    public class Navigate
    {
        /// <summary>
        /// holds the parent nodes, and current directory
        /// </summary>
        Stack<Node> breadcrumb = new Stack<Node>();

        /// <summary>
        ///  returns the current breadcrumb trail of the current directory
        /// </summary>
        public string path
        {
            get
            {
                var p = "";
                foreach (var item in breadcrumb.Reverse())
                {
                    p += String.Format("{0}{1}", item.name, CommandFactory.delim);
                }
                p.TrimEnd(CommandFactory.delim);
                return p;
            }
        }

        /// <summary>
        /// current node
        /// </summary>
        public Node current { get; set; }

        /// <summary>
        /// move into a given directory/node or subdirectory/subnode 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool MoveIn(string input)
        {

            if (input.Contains(CommandFactory.delim))
            {
                var names = input.Split(CommandFactory.delim);

                foreach (var name in names)
                {
                    MoveInTo(name);
                }
            }
            else
            {
                MoveInTo(input);
            }

            return true;
        }

        /// <summary>
        /// move out of the current directory/node
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool MoveOut(string input)
        {
            //if there is no argument assume input is current nav
            if (input == "") { input = current.name; }
            if (input.Contains(CommandFactory.delim))
            {
                var names = input.Split(CommandFactory.delim);

                foreach (var name in names.Reverse())
                {
                    MoveOutFrom(name);
                }
            }
            else
            {
                MoveOutFrom(input);
            }

            return true;
        }

        /// <summary>
        ///   move into a given directory/node
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool MoveInTo(string name)
        {
            //search the current node for the child
            if (current.children.Exists(x => x.name == name))
            {
                //set the current as a parent. in breadcrumb 
                breadcrumb.Push(current);

                //assign child as new current 
                current = current.children.Find(x => x.name == name);

                return true;
            }
            return false;
        }

        /// <summary>
        /// move out of current node and into a node in parent directory/node
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool MoveOutFrom(string name)
        {
            try
            {
                //search the parent for the child
                //search the current node for the child
                if (breadcrumb.Peek().children.Exists(x => x.name == name))
                {
                    current = breadcrumb.Pop();
                    return true;
                }
                return false;
            }
            catch
            {
                //trying to escape from root
                return false;
            }
        }

    }
}
