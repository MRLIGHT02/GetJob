using GetJob.Entities;

namespace GetJob.App.Views.Pages;

public partial class ChooseRole : ContentPage
{
	public UserRole SelectedRole { get; set; }
	public ChooseRole()
	{
		InitializeComponent();
	}

    private void RoleRadioCheckedChanged(object sender, CheckedChangedEventArgs e)
    {

        if (!e.Value) return; // Only act when a radio is selected

        if (sender is RadioButton selectedRadio && selectedRadio.Value is UserRole selectedRole)
        {
            // Navigate directly to RegisterPage with selected role
            Shell.Current.GoToAsync($"{nameof(RegisterPage)}?role={selectedRole}");
        }
    }
}