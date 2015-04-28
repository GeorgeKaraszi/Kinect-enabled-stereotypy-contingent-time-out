const int COUNT_CONST = 10;
int countdown;
private Timer timer;


//Somewhere during program inisalization
timer = new Timer();
countdown = COUNT_CONST;
timer.Tick += timer_Tick;


//Timer Event
void timer_Tick(object sender, EventArgs e)
{
	if (countdown > 0)
		countdown -= 1;
	else
	{
		quiet_window_running = true;
		countdown = COUNT_CONST;
		QuietHandsWindow win = new QuietHandsWindow();
		win.ShowDialog();
		timer.Stop();
		quiet_window_running = false;
	}


}



// Placed some where in code that starts the timer
if (quiet_window_running == false)
{
    if (this.Detected)
    { 
	this.Confidence = detectionConfidence;
	//this.ImageSource = this.seatedImage;

	timer.Start();
	}
	else
                    {
		//this.ImageSource = this.notSeatedImage;
		timer.Stop();
		countdown = COUNT_CONST;
	}
}
else
{
   timer.Stop();
   countdown = COUNT_CONST;
}
