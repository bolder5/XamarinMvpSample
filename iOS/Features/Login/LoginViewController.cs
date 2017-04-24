using System;
using Ninject;
using UIKit;

namespace XamarinMvpSample.iOS
{
	public class LoginViewController : MvpViewController<LoginPresenter>, ILoginView
	{
		private UITextField _usernameTextField = new UITextField();
		private UITextField _passwordTextField = new UITextField();
		private UIButton _loginButton = new UIButton(UIButtonType.System);
		private UILabel _messageLabel = new UILabel();

		public LoginViewController()
		{
			SetupView();
		}

		private void SetupView()
		{
			EdgesForExtendedLayout = UIRectEdge.None;
			Title = "Login";

			var containerView = new UIView();
			var stackView = new UIStackView(new UIView[] {
				_usernameTextField,
				_passwordTextField,
				_loginButton,
				_messageLabel,
			});
			View.AddSubview(containerView);
			containerView.AddSubview(stackView);

			containerView.TranslatesAutoresizingMaskIntoConstraints = false;
			stackView.TranslatesAutoresizingMaskIntoConstraints = false;
			_usernameTextField.TranslatesAutoresizingMaskIntoConstraints = false;
			_passwordTextField.TranslatesAutoresizingMaskIntoConstraints = false;
			_loginButton.TranslatesAutoresizingMaskIntoConstraints = false;

			stackView.LeadingAnchor.ConstraintEqualTo(containerView.LeadingAnchor, 15).Active = true;
			stackView.TopAnchor.ConstraintEqualTo(containerView.TopAnchor, 15).Active = true;
			stackView.TrailingAnchor.ConstraintEqualTo(containerView.TrailingAnchor, -15).Active = true;

			_usernameTextField.HeightAnchor.ConstraintEqualTo(55).Active = true;
			_passwordTextField.HeightAnchor.ConstraintEqualTo(55).Active = true;
			_loginButton.HeightAnchor.ConstraintEqualTo(55).Active = true;

			containerView.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor).Active = true;
			containerView.TopAnchor.ConstraintEqualTo(View.TopAnchor).Active = true;
			containerView.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor).Active = true;
			containerView.BottomAnchor.ConstraintEqualTo(View.BottomAnchor).Active = true;

			containerView.BackgroundColor = UIColor.White;
			stackView.Axis = UILayoutConstraintAxis.Vertical;
			_usernameTextField.Placeholder = "Username";
			_passwordTextField.Placeholder = "Password";
			_passwordTextField.SecureTextEntry = true;
			_loginButton.SetTitle("LOGIN", UIControlState.Normal);
			_loginButton.Enabled = false;
			_messageLabel.TextAlignment = UITextAlignment.Center;
			_messageLabel.Hidden = true;
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			AddObservers();
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

			RemoveObservers();
		}

		private void AddObservers()
		{
			_usernameTextField.EditingChanged += UsernameChanged;
			_passwordTextField.EditingChanged += PasswordChanged;
			_loginButton.TouchUpInside += LoginButtonPressed;
		}

		private void RemoveObservers()
		{
			_usernameTextField.EditingChanged -= UsernameChanged;
			_passwordTextField.EditingChanged -= PasswordChanged;
			_loginButton.TouchUpInside -= LoginButtonPressed;
		}

		public override IKernel CreateKernel()
		{
			return new StandardKernel(new ApplicationModule(), new LoginModule(this));
		}

		public void OnGoToNextScreen()
		{
			_messageLabel.Text = "Success! TODO: Go to next screen";
			_messageLabel.Hidden = false;
		}

		public void OnInvalidCredentials(string message)
		{
			_messageLabel.Text = message;
			_messageLabel.Hidden = false;
		}

		public void OnLoginButtonEnabled(bool enabled)
		{
			_loginButton.Enabled = enabled;
		}

		public override void OnNetworkError()
		{
			// TODO:
		}

		public void OnShouldClearError()
		{
			_messageLabel.Hidden = true;
			_messageLabel.Text = null;
		}

		public void OnWaiting()
		{
			//TODO: since there's no actual delay in the "API" we don't need a loading state
			//No Op
		}

		public void OnStopWaiting()
		{
			//TODO: see above
			//No Op
		}

		private void UsernameChanged(object sender, EventArgs e)
		{
			Presenter.UpdateUsername(_usernameTextField.Text);
		}

		private void PasswordChanged(object sender, EventArgs e)
		{
			Presenter.UpdatePassword(_passwordTextField.Text);
		}

		private void LoginButtonPressed(object sender, EventArgs e)
		{
			Presenter.Login().RunConcurrent();
		}
	}
}