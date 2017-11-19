using dTerm.UI.Wpf.Infrastructure;
using System;

namespace dTerm.UI.Wpf.ViewModels
{
	public abstract class BaseViewModel : ObservableObject
	{
		public abstract void Init(IntPtr viewHandle);
	}
}
