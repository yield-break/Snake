using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jTech.Wpf.Mvvm;

namespace Snake.ViewModel.GameArgs
{
    public class SnakePlayerArgs : NotifyPropertyChangedViewModel
    {
        public SnakePlayerArgs()
        {
            DoesSnakeSpeedUp = true;
        }

        private bool _doesSnakeSpeedUp;
        public bool DoesSnakeSpeedUp 
        {
            get { return _doesSnakeSpeedUp; }
            set { OnChange(ref _doesSnakeSpeedUp, value); }
        }

    }
}
