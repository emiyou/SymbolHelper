using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using log4net;
using SymbolAnalysis;

namespace Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            string handRecord;
            List<Tag> tags = TagHandler.GetCorrectTags(@"E:\2017\DebugStandardTable\SymbolAnalysis\V4_BAIC\Symbol\ZULI3.SDF",
                out handRecord);
            ILog logger = LogManager.GetLogger("Tag.Logging");
            logger.Debug(handRecord);
            MessageBox.Show("ok");
        }
    }
}
