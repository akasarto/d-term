using ReactiveUI;
using System.Windows;

namespace UI.Wpf.Workspace
{
	/// <summary>
	/// General settings view.
	/// </summary>
	public partial class GeneralSettingsView : IViewFor<IGeneralSettingsViewModel>
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public GeneralSettingsView()
		{
			InitializeComponent();
		}

		/// <summary>
		/// View model dependency property backing field.
		/// </summary>
		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(IGeneralSettingsViewModel), typeof(GeneralSettingsView), new PropertyMetadata(null));

		/// <summary>
		/// Gets or sets the view model instance.
		/// </summary>
		public IGeneralSettingsViewModel ViewModel
		{
			get { return (IGeneralSettingsViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		/// <summary>
		/// Gets or sets the view model instance.
		/// </summary>
		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (IGeneralSettingsViewModel)value; }
		}
	}
}
