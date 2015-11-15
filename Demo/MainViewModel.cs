using Demo.Common;
using Neumann.HelpFrame;
using System.Windows.Input;

namespace Demo
{
    public class MainViewModel : ModelBase
    {
        private bool _showHelp;
        private bool _showBackgroundLayer = true;
        private HelpFrameDisplayStyle _displayStyle = HelpFrameDisplayStyle.Classic;
        private ICommand _showCommand;
        private ICommand _closeCommand;

        public MainViewModel()
        {
            this.ShowCommand = new DelegateCommand(p => this.ShowHelp = true);
            this.CloseCommand = new DelegateCommand(p => this.ShowHelp = false);
        }

        public bool ShowBackgroundLayer
        {
            get { return _showBackgroundLayer; }
            set { SetProperty(ref _showBackgroundLayer, value); }
        }

        public bool ShowHelp
        {
            get { return _showHelp; }
            set { SetProperty(ref _showHelp, value); }
        }

        public HelpFrameDisplayStyle DisplayStyle
        {
            get { return _displayStyle; }
            set { SetProperty(ref _displayStyle, value); }
        }

        public ICommand CloseCommand
        {
            get { return _closeCommand; }
            set { SetProperty(ref _closeCommand, value); }
        }

        public ICommand ShowCommand
        {
            get { return _showCommand; }
            set { SetProperty(ref _showCommand, value); }
        }
    }
}
