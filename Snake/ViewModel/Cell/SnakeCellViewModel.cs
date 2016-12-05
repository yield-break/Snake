using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jTech.Wpf.Mvvm;
using Snake.Model;

namespace Snake.ViewModel.Cell
{
    public class SnakeCellViewModel : NotifyPropertyChangedViewModel
    {
        private SnakeyToken? _snakeyToken;
        public SnakeyToken? SnakeyToken
        {
            get { return _snakeyToken; }
            set 
            { 
                if (OnChange(ref _snakeyToken, value))
                {
                    NotifyPropertyChanged(() => HasSnakey);
                }
            }
        }

        public bool HasSnakey
        {
            get { return _snakeyToken != null; }
        }

    }
}
