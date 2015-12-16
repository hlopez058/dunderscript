using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Rabbit.Controls;

namespace dunderscript
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<String> ContentAssistSource
        {
            get { return (List<String>)GetValue(ContentAssistSourceProperty); }
            set { SetValue(ContentAssistSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ContentAssisteSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentAssistSourceProperty =
            DependencyProperty.Register("ContentAssistSource", typeof(List<String>), typeof(MainWindow), new UIPropertyMetadata(new List<string>()));


        public List<char> ContentAssistTriggers
        {
            get { return (List<char>)GetValue(ContentAssistTriggersProperty); }
            set { SetValue(ContentAssistTriggersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ContentAssistTriggers.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentAssistTriggersProperty =
            DependencyProperty.Register("ContentAssistTriggers", typeof(List<char>), typeof(MainWindow), new UIPropertyMetadata(new List<char>()));


        public MainWindow()
        {
            InitializeComponent();
            ContentAssistTriggers = LiveDoc.InitRichTextBoxIntellisenseTrigger();
            ContentAssistSource = LiveDoc.InitRichTextBoxSource();
            DataContext = this;
        }

        private void frmMain_Loaded(object sender, RoutedEventArgs e)
        {
            //begin initialization routine
            LoadItemsToTree();
            
        }

        private void LoadItemsToTree()
        {
            var n = new ObjectLibrary();
            n.LoadFiles();

            tvItemObjLib.Items.Clear();
            foreach (var i in n.ScriptFiles)
            {
                var item = new TreeViewItem();
                //store item object and header
                item.Tag = i;
                item.Header = i.name;
                //create item handler on click
                item.MouseDoubleClick += item_MouseDoubleClick;
                tvItemObjLib.Items.Add(item);
            }

        }

        void item_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var tvItem = (TreeViewItem)sender;
            var file = (scriptFile)tvItem.Tag;
            OpenFileForEditing(file);
        }

        private void OpenFileForEditing(scriptFile file)
        {
            //create a new tab window
            var tab =  new TabItem();
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

   
    }

}
