using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dunderscript
{
    class LiveDoc
    {
        
       internal static List<string> InitRichTextBoxSource()
        {
            var ContentAssistSource = new List<string>();

            ContentAssistSource.Add("aaal");
            ContentAssistSource.Add("as");
            ContentAssistSource.Add("aacp");
            ContentAssistSource.Add("aid");
            ContentAssistSource.Add("asap");

            ContentAssistSource.Add("boy");
            ContentAssistSource.Add("big");
            ContentAssistSource.Add("before");
            ContentAssistSource.Add("belong");
            ContentAssistSource.Add("can");
            ContentAssistSource.Add("clever");
            ContentAssistSource.Add("cool");
            ContentAssistSource.Add("data");
            ContentAssistSource.Add("delete");

            return ContentAssistSource;
        }


        internal static List<char> InitRichTextBoxIntellisenseTrigger()
        {
            var ContentAssistTriggers = new List<char>();

            ContentAssistTriggers.Add('@');
            ContentAssistTriggers.Add('.');

            return ContentAssistTriggers;
        }
    }


}
