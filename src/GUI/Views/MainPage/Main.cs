using XML_Utils;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Maui;
using System.Xml;
using System.Xml.Xsl;

namespace GUI;

public partial class MainPage : ContentPage
{
	private IParser parser;

	private string AppDataDirectory;

	private FileResult ChosenFile;

	private IFileSaver fileSaver;

	private IFilePicker filePicker;

	private XmlReaderSettings validationSettings;

	private XslCompiledTransform exporter;
	public MainPage()
	{
		InitializeComponent();
		fileSaver = FileSaver.Default;
		filePicker = FilePicker.Default;
		ParserPicker.SelectedIndex = 1;
	}
}
