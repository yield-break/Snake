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
    /// Interaction logic for KeyIcon.xaml
    /// </summary>
    public partial class KeyIcon : UserControl
    {
        public KeyIcon()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
          "Icon",
          typeof(object),
          typeof(KeyIcon),
          new PropertyMetadata(false));

        public object Icon
        {
            get { return (object)this.GetValue(IconProperty); }
            set { this.SetValue(IconProperty, value); }
        }

    }
}
