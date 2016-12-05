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

namespace Snake.View
{
    /// <summary>
    /// Interaction logic for GameBoardView.xaml
    /// </summary>
    public partial class GameBoardView : UserControl
    {
        public GameBoardView()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty CellDataTemplateProperty = DependencyProperty.Register(
            "CellDataTemplate",
            typeof(DataTemplate),
            typeof(GameBoardView));

        public DataTemplate CellDataTemplate
        {
            get { return (DataTemplate) GetValue(CellDataTemplateProperty); }
            set { SetValue(CellDataTemplateProperty, (DataTemplate) value); }
        }

    }
}
