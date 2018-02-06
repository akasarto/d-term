using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace UI.Wpf.Notebook
{
	public partial class NoteFormView : UserControl
	{
		public NoteFormView()
		{
			InitializeComponent();
		}

		public static readonly DependencyProperty NoteProperty = DependencyProperty.Register("Note", typeof(NoteViewModel), typeof(NoteFormView), new PropertyMetadata(null));
		public static readonly DependencyProperty CancelCommandProperty = DependencyProperty.Register("CancelCommand", typeof(ICommand), typeof(NoteFormView), new PropertyMetadata(null));
		public static readonly DependencyProperty SaveCommandProperty = DependencyProperty.Register("SaveCommand", typeof(ICommand), typeof(NoteFormView), new PropertyMetadata(null));

		public NoteViewModel Note
		{
			get { return (NoteViewModel)GetValue(NoteProperty); }
			set { SetValue(NoteProperty, value); }
		}

		public ICommand CancelCommand
		{
			get { return (ICommand)GetValue(CancelCommandProperty); }
			set { SetValue(CancelCommandProperty, value); }
		}

		public ICommand SaveCommand
		{
			get { return (ICommand)GetValue(SaveCommandProperty); }
			set { SetValue(SaveCommandProperty, value); }
		}
	}
}
