namespace UI.Wpf.Infrastructure
{
	public interface IViewModelFactory
	{
		TViewModel Create<TViewModel>();
	}
}
