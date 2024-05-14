namespace Programming_Examination_Platform.Services;

public class TimerService
{
  private Timer _timer;
  public TimeSpan TimeRemaining { get; private set; }
  public event Action? OnTick;
  public event EventHandler? TimerElapsed;

  public TimerService()
  {
    // Initialize the timer
    _timer = new Timer(UpdateTimer, null, Timeout.Infinite, 1000);
    // Set the initial time, for example, 1 hour
    TimeRemaining = TimeSpan.FromHours(1);
  }

  private void UpdateTimer(object? state)
  {
    TimeRemaining = TimeRemaining.Add(TimeSpan.FromSeconds(-1));
    OnTick?.Invoke(); // Notify subscribers that the timer has ticked
    if (TimeRemaining <= TimeSpan.Zero)
    {
      _timer.Change(Timeout.Infinite, 1000); // Stop the timer
    }

    TimerElapsed!(this, EventArgs.Empty);
  }

  public void SetTimeRemaining(TimeSpan time)
  {
    TimeRemaining = time;
  }

  public void StartTimer()
  {
    _timer.Change(0, 1000); // Start the timer
  }

  public void StopTimer()
  {
    _timer.Change(Timeout.Infinite, 1000); // Stop the timer
  }

  public void ReinitializeTimer(TimeSpan newDuration)
  {
    TimeRemaining = newDuration;
    _timer.Change(0, 1000); // Restart the timer
  }

  
}