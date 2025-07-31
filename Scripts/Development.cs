using Godot;

public partial class Development : Control
{
    string[] Responses = ["Newer release(s) are availiable.", "Already on latest release." ,"Failed to send request. Check your internet connection.", "Unexpected error. Try after some time."];
    public override void _Ready()
    {
        GetNode<Label>("Version").Text = ProjectSettings.GetSetting("application/config/version").ToString();
        GetNode<LinkButton>("VBoxContainer/UpdateButton").Pressed += GetNode<Settings>("../Settings").CheckForUpdate;
    }
    public void UpdateCode(int Code)
    {
        Label Response = GetNode<Label>("VBoxContainer/UpdateButton/Update");
        Response.Text = Responses[Code];

        if (Code == 0) Response.GetChild<LinkButton>(0).Show();
    }
}
