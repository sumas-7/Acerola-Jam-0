using Godot;

public partial class credits : RichTextLabel
{
	private string originalText;

	public override void _Ready()
	{
		originalText = Text;
		MetaUnderlined = false;

		MetaClicked += OnClick;
		MetaHoverStarted += HoverStarted;
		MetaHoverEnded += HoverEnded;
	}

	private void OnClick(Variant meta)
	{
		GameManager.Instance.ToggleCredits();
	}
	private void HoverStarted(Variant meta)
	{
		Text = "[u]" + originalText + "[/u]";
	}
	private void HoverEnded(Variant meta)
	{
		Text = originalText;
	}
}