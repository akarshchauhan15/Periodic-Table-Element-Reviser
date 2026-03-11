using Godot;

public partial class Updates : Control
{
    Label Response;
    Timer UpdateTimer;

    string[] Responses = [
        "Newer release(s) are availiable.",
        "Already on latest release." ,
        "Failed to send request. Check your internet connection.",
        "Unexpected error. Try after some time.", 
        "Checking for updates"];

    int DotCount = 0;

    public override void _Ready()
    {
        Response = GetNode<Label>("VBoxContainer/UpdateButton/Update");
        UpdateTimer = GetNode<Timer>("VBoxContainer/UpdateButton/Update/UpdateTimer");
        UpdateTimer.Timeout += OnUpdateTimerEnd;

        GetNode<Label>("../About/Version").Text = ProjectSettings.GetSetting("application/config/version").ToString();
        GetNode<LinkButton>("VBoxContainer/UpdateButton").Pressed += GetNode<Settings>("../Settings").CheckForUpdate;
    }
    public void UpdateCode(int Code)
    {
        Response.Text = Responses[Code];

        if (Code == 0) Response.GetChild<LinkButton>(0).Show();
        else if (Code == 4) {UpdateTimer.Start(); return; }
        UpdateTimer.Stop();
    }
    private void OnUpdateTimerEnd()
    {
        if (DotCount < 3){
            Response.Text += " .";
            DotCount++;
            return;
        }
        Response.Text = Responses[4];
        DotCount = 0;
    }
}
