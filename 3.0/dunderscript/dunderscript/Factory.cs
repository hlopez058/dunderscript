using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
namespace dunderscript
{
   public  class Factory
    {
       
        public static List<string> InitRichTextBoxSource()
        {
            var ContentAssistSource = new List<string>();

            ContentAssistSource.Add("Save");

            return ContentAssistSource;
        }

        public static List<StoryBoard.Controls.RichTextBoxEx.jsObject> InitRichTextBoxIntellisenseTrigger()
        {
            var ContentAssistTriggers = new List<StoryBoard.Controls.RichTextBoxEx.jsObject>();
            var obj = new StoryBoard.Controls.RichTextBoxEx.jsObject();
            obj.name = "lib";
            obj.json = "{ 'Name': 'Jon Smith', 'Address': { 'City': 'New York', 'State': 'NY' }, 'Age': 42 }";
            obj.trigger = "lib.";
            
            ContentAssistTriggers.Add(obj);

            return ContentAssistTriggers;
        }


        internal static void OpenFileForEditing(scriptFile file, 
            ref System.Windows.Controls.RichTextBox rtfEditor,
            ref System.Windows.Controls.TabControl tabDocuments,int tabFocusIndex)
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
            var ctab = tabDocuments.Items.GetItemAt(tabFocusIndex);
            tabDocuments.SelectedItem = ctab;
        }
        
    }


}
