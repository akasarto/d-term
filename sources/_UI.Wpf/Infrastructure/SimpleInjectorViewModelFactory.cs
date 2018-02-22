using SimpleInjector;

namespace UI.Wpf.Infrastructure
{
	public class SimpleInjectorViewModelFactory : IViewModelFactory
	{
		private readonly Container _container = null;

		public SimpleInjectorViewModelFactory(Container container)
		{
			_container = container;
		}

		public TViewModel Create<TViewModel>() => (TViewModel)_container.GetInstance(typeof(TViewModel));
	}
}
