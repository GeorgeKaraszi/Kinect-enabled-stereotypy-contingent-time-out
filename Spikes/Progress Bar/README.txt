Add Progress bar to GUI.


During program inialization
progressBar1.Maximum = 10    //Max value the bar can reach
progressBar1.Value = 10      //Value we start at (Count down ward)
progressBar1.Minimum = 0;    //Last value it can be

IN TIMER EVENT
progressBar1.Value -= 1