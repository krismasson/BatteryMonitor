namespace BattStuff;

public partial class MainPage : ContentPage
{
    private bool isTimerRunning;
    private Timer timer;

    public MainPage()
	{
		InitializeComponent();
	}

    private async void StartStopButton_Clicked(object sender, EventArgs e)
    {
        if (isTimerRunning)
        {
            // Stop the timer
            timer?.Dispose();
            timer = null;
            isTimerRunning = false;

            // Update button text
            StartStopButton.Text = "Start";
        }
        else
        {
            // Start the timer
            timer = new Timer(SendBatteryLife, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
            isTimerRunning = true;

            // Update button text
            StartStopButton.Text = "Stop";
        }
    }

    private async void SendBatteryLife(object state)
    {
        try
        {
            var batteryLife = Battery.EnergySaverStatus == EnergySaverStatus.On ? 0 : Battery.ChargeLevel * 100;

            using (var client = new HttpClient())
            {
                var url = $"http://peezy.us.to/batt.php?batteryLife={batteryLife}";

                // Send the HTTP POST request
                await client.PostAsync(url, null);
            }
        }
        catch (Exception ex)
        {
            // Handle any errors or exceptions
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

}

