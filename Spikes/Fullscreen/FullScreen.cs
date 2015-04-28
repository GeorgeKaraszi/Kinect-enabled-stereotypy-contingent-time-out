/// <summary>
/// Sets the windows size and functionality. This also hides the mouse courser from displaying
/// </summary>
private void Setupwindow()
{
	Cursor.Hide();
	this.FormBorderStyle = FormBorderStyle.None;
	this.WindowState = FormWindowState.Maximized;
	this.TopMost = true;
}



//Center the text with the screen.
//Move the text slightly up to make it more appealing
int xQhands = this.Size.Width / 2 - qhands_lbl.Width / 2;
int yQhands = (this.Size.Height / 2 - qhands_lbl.Height / 2) - 150;