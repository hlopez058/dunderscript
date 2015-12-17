using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

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

            return ContentAssistSource;
        }

        internal static List<char> InitRichTextBoxIntellisenseTrigger()
        {
            var ContentAssistTriggers = new List<char>();

            ContentAssistTriggers.Add('.');

            return ContentAssistTriggers;
        }

        internal static List<char> InitRichTextBoxScriptingTrigger()
        {
            var ContentScriptTriggers = new List<char>();

            ContentScriptTriggers.Add('{');
            
            return ContentScriptTriggers;
        }

        internal static void OpenFileForEditing(scriptFile file, 
            ref System.Windows.Controls.RichTextBox rtfEditor,
            ref System.Windows.Controls.TabControl tabDocuments)
        {
            //create a new tab window
            var tab = new TabItem();
            //add a rich text box
            var rtf = new System.Windows.Forms.RichTextBox();
            var data = System.IO.File.ReadAllText(file.path);
            TextRange txt = new TextRange(rtfEditor.Document.ContentStart, rtfEditor.Document.ContentEnd);
            txt.Text = "";
            txt.Text = data;
            //bring editor to foreground
            var ctab = tabDocuments.Items.GetItemAt(1);
            tabDocuments.SelectedItem = ctab;
        }


        internal static List<char> InitRichTextBoxScriptingTerminators()
        {
            var ContentScriptTerminators = new List<char>();

            ContentScriptTerminators.Add('}');

            return ContentScriptTerminators;
        }
    }


}
