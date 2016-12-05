using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jTech.Common.Core;
using jTech.Wpf.Mvvm;
using Snake.ViewModel;

namespace Snake
{
    public class MainWindowViewModel : NotifyPropertyChangedViewModel
    {
        public MainWindowViewModel()
        {
            try
            {
                RunBootstrap();
                Task.Run(() => StartAsync());
            }
            catch (Exception e)
            {
                // TODO:
            }
        }

        private void RunBootstrap()
        {
            var container = CompositionContainerFactory.Instance;

            GameSummary = container.GetExportedValue<ISnakeyGameSummaryViewModel>();
            SnakeMasterViewModel = container.GetExportedValue<ISnakeMasterViewModel>();

            SnakeMasterViewModel.Initialise();
        }

        private async Task StartAsync()
        {
            try
            {
                while (true)
                {
                    var gameArgs = await GameSummary.GetGameArgsAsync();
                    await SnakeMasterViewModel.GoSnakeAsync(gameArgs);

                    SnakeMasterViewModel.Reset();
                }
            }
            catch (Exception)
            {
                // TODO
            }
        }
        
        public ISnakeyGameSummaryViewModel GameSummary { get; private set; }

        public ISnakeMasterViewModel SnakeMasterViewModel { get; private set; }

    }
}
