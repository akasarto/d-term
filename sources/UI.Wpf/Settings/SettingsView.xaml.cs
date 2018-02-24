using ReactiveUI;
using System.Windows;

namespace UI.Wpf.Settings
{
	/// <summary>
	/// General settings view.
	/// </summary>
	public partial class SettingsView : IViewFor<ISettingsViewModel>
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public SettingsView()
		{
			InitializeComponent();
		}

		/// <summary>
		/// View model dependency property backing field.
		/// </summary>
		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(ISettingsViewModel), typeof(SettingsView), new PropertyMetadata(null));

		/// <summary>
		/// Gets or sets the view model instance.
		/// </summary>
		public ISettingsViewModel ViewModel
		{
			get { return (ISettingsViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		/// <summary>
		/// Gets or sets the view model instance.
		/// </summary>
		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (ISettingsViewModel)value; }
		}
	}
}
