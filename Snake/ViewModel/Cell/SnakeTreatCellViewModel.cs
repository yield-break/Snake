using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.ViewModel.Cell
{
    public class SnakeTreatCellViewModel : SnakeCellViewModel
    {
        public SnakeTreatCellViewModel(TreatType treat)
        {
            Treat = treat;
        }

        private TreatType _treat;
        public TreatType Treat
        {
            get { return _treat; }
            set { OnChange(ref _treat, value); }
        }

        public enum TreatType
        {
            Cherry,
            Apple,
            Banana,
        }

    }
}
