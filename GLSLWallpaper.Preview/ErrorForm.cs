namespace GLSLWallpaper.Preview;

public class ErrorForm : Form {
    readonly Window _owner;
    readonly RichTextBox _content;
    readonly Font _font = new(FontFamily.GenericMonospace, 10);

    public ErrorForm(Window owner) {
        _owner = owner;
        _content = new RichTextBox();

        SuspendLayout();

        FormBorderStyle = FormBorderStyle.None;
        TopMost = true;
        Opacity = 0.8;

        _content.Dock = DockStyle.Fill;
        _content.Font = _font;
        _content.BorderStyle = BorderStyle.None;
        _content.ReadOnly = true;

        Controls.Add(_content);

        ResumeLayout(false);
        PerformLayout();

        _owner.Resize += args => ClientSize = new Size(args.Width, args.Height / 2);
        _owner.Move += args => Location = new Point(args.X, args.Y);
    }

    public void SetMessage(string message) {
        _content.Text = message.Trim();
        Height = Math.Min((int)(Graphics.FromHwnd(Handle).MeasureString(_content.Text, _font).Height * 1.1), _owner.Size.Y);
    }

    protected override void OnShown(EventArgs e) {
        base.OnShown(e);
        Location = new Point(_owner.Location.X, _owner.Location.Y);
    }
}
