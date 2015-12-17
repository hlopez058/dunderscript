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
using StoryBoard.Controls;

namespace dunderscript
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region declarations
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

       
        public List<String> ContentPublicSource
        {
            get { return (List<String>)GetValue(ContentPublicSourceProperty); }
            set { SetValue(ContentPublicSourceProperty, value); }
        }
         // Using a DependencyProperty as the backing store for ContentPublicSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentPublicSourceProperty =
            DependencyProperty.Register("ContentPublicSource", typeof(List<String>), typeof(MainWindow), new UIPropertyMetadata(new List<string>()));

        
        public List<String> ContentPrivateSource
        {
            get { return (List<String>)GetValue(ContentPrivateSourceProperty); }
            set { SetValue(ContentPrivateSourceProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ContentPrivateSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentPrivateSourceProperty =
            DependencyProperty.Register("ContentPrivateSource", typeof(List<String>), typeof(MainWindow), new UIPropertyMetadata(new List<string>()));

        public List<char> ContentScriptTriggers
        {
            get { return (List<char>)GetValue(ContentScriptTriggersProperty); }
            set { SetValue(ContentScriptTriggersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ContentscriptTriggers.
        public static readonly DependencyProperty ContentScriptTriggersProperty =
            DependencyProperty.Register("ContentScriptTriggers", typeof(List<char>), typeof(MainWindow), new UIPropertyMetadata(new List<char>()));

        public List<char> ContentScriptTerminators
        {
            get { return (List<char>)GetValue(ContentScriptTerminatorsProperty); }
            set { SetValue(ContentScriptTerminatorsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ContentscriptTerminators.
        public static readonly DependencyProperty ContentScriptTerminatorsProperty =
            DependencyProperty.Register("ContentScriptTerminators", typeof(List<char>), typeof(MainWindow), new UIPropertyMetadata(new List<char>()));


        #endregion

        public MainWindow()
        {
            InitializeComponent();

            ContentScriptTriggers = LiveDoc.InitRichTextBoxScriptingTrigger();
            ContentScriptTerminators = LiveDoc.InitRichTextBoxScriptingTerminators();
            ContentAssistTriggers = LiveDoc.InitRichTextBoxIntellisenseTrigger();
            ContentAssistSource = LiveDoc.InitRichTextBoxSource();
            
            DataContext = this;
        }

        private void frmMain_Loaded(object sender, RoutedEventArgs e)
        {
            //begin initialization routine
            LoadItemsToTree();
            
            //TODO: handlers to add new public/private content when added to lists
        }

      

        #region controller
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

        #endregion



        #region event handlers

        void item_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var tvItem = (TreeViewItem)sender;
            var file = (scriptFile)tvItem.Tag;
            LiveDoc.OpenFileForEditing(file,ref rtfEditor,ref tabDocuments);
        }
        #endregion

        



    }

}
