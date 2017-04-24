using System;
using UIKit;
using Ninject;

namespace XamarinMvpSample.iOS
{
	public abstract class MvpViewController<T> : UIViewController, IBaseView where T : BasePresenter
	{
		protected T Presenter { get; set; }
		private IKernel _kernel;
		protected IKernel Kernel => _kernel;

		public abstract IKernel CreateKernel();
		public abstract void OnNetworkError();

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			_kernel = CreateKernel();
			Presenter = Kernel.Get<T>();
			Presenter.View = this;
			Presenter.Init();
		}
	}
}
