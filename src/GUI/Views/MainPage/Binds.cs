using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using XML_Utils;

namespace GUI;

public partial class MainPage : ContentPage
{

	private async void ExitButton_Clicked(object sender, EventArgs e)
	{
		var option = await DisplayAlert("Confirm exit", "Are you sure tou want to exit the program ?", "Yes", "No");
		if (option)
		{
			System.Environment.Exit(0);
		}
	}

	private void ImportValidationSchema(string filepath)
	{
		var schema = new XmlSchemaSet();
		schema.Add("", filepath);

		validationSettings = new XmlReaderSettings
		{
			Schemas = schema
		};
		validationSettings.ValidationEventHandler += (object sender, ValidationEventArgs e) =>
		{
			if (e.Severity == XmlSeverityType.Error) throw new Exception();
		};
		validationSettings.ValidationType = ValidationType.Schema;
	}
	private async void OpenButton_Clicked(object sender, EventArgs e)
	{
		if (validationSettings is null)
		{
			if (AppDataDirectory is null)
			{
				InitAppData();
			}
			if (!Path.Exists(Path.Combine(AppDataDirectory, "schema.xsd")))
			{
				await CopyFileToAppDataDirectory("schema.xsd");
			}

			ImportValidationSchema(Path.Combine(AppDataDirectory, "schema.xsd"));
		}

		var customFileType = new FilePickerFileType(
				new Dictionary<DevicePlatform, IEnumerable<string>>
				{
					{ DevicePlatform.WinUI, new[] { ".xml" } },
				});
		var options = new PickOptions() { PickerTitle = "Select xml file", FileTypes = customFileType };
		ChosenFile = await filePicker.PickAsync(options);

		if (ChosenFile is null)
		{
			Title = "XML Labwork - File is not chosen";
			return;
		}

		Title = "XML Labwork - " + ChosenFile.FileName;
		await ValidateFile();

		if (ChosenFile is not null && parser is not null)
		{
			MakeSearch();
		}
	}

    private string CreateTempXmlFile(List<Class> data)
    {
        var xml = new XDocument(new XElement("TimeTable")); 
        foreach (var item in data)
        {
            var classElement = new XElement("Class",
                new XElement("Day", item.Date.Day),
                new XElement("Time", item.Date.Time),
                new XElement("Subject", item.Subject),
                new XElement("Audience", item.Audience),
                new XElement("Teacher", $"{item.Person.Name.LastName} {item.Person.Name.FirstName}"),
                new XElement("Groups", item.Students.Count > 0 ? item.Students[0].Group : "")
            );
            xml.Root.Add(classElement);
        }

        string tempPath = Path.Combine(FileSystem.CacheDirectory, "temp_export.xml");
        xml.Save(tempPath);
        return tempPath;
    }
    private async void ExportButton_Clicked(object sender, EventArgs e)
    {
        if (parser == null || parser.Classes == null || parser.Classes.Count == 0)
        {
            await DisplayAlert("Error", "No data to export. Please load a file first.", "Ok");
            return;
        }

        if (exporter == null)
        {
            string xslPath = Path.Combine(AppDataDirectory, "schema.xsl");
            
			await CopyFileToAppDataDirectory("schema.xsl");

            exporter = new System.Xml.Xsl.XslCompiledTransform();
            exporter.Load(xslPath);
        }

        try
        {
            List<Class> dataToExport;

            if (displayedClasses != null && displayedClasses.Count > 0 && displayedClasses.Count < parser.Classes.Count)
            {
                dataToExport = displayedClasses;
            }
            else
            {
                dataToExport = parser.Classes as List<Class> ?? parser.Classes.ToList();
            }

            string sourceXmlPath = CreateTempXmlFile(dataToExport);

            using var stream = new MemoryStream(Encoding.Default.GetBytes(""));
            string defaultFileName = (ChosenFile?.FileName?.Split(".")[0] ?? "Export") + ".html";

            var result = await fileSaver.SaveAsync(defaultFileName, stream, new CancellationTokenSource().Token);

            if (result.IsSuccessful)
            {
                exporter.Transform(sourceXmlPath, result.FilePath);
                await DisplayAlert("Success", "File was exported successfully", "Ok");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Export failed: {ex.Message}", "Ok");
        }
    }

    private async void AboutButton_Clicked(object sender, EventArgs args)
	{
		await DisplayAlert("Information", "Author: Soboleva Sofiia,K-24\n\nThis program allows you to read TimeTable XML Documents, filter them and export them in HTML format", "Ok");
	}

	private async void FindButton_Clicked(object sender, EventArgs e)
	{
		if (ChosenFile == null)
		{
			await DisplayAlert("Error", "Input file is not chosen", "Ok");
			return;
		}

		if (parser == null)
		{
			await DisplayAlert("Error", "Parser type is not chosen", "Ok");
			return;
		}

		MakeSearch();
	}

	private void ClearFiltersButton_Clicked(object sender, EventArgs e)
	{
		ClearFilters();
	}

	private void ClearResultsButton_Clicked(object sender, EventArgs e)
	{
		ClearResults();
	}

	private void OnFilterChanged(object sender, EventArgs args)
	{
		if (ChosenFile is null || parser is null)
		{
			return;
		}

		MakeSearch();
	}

	private async void Parser_Selected(object sender, EventArgs e)
	{
		switch (ParserPicker.SelectedIndex)
		{
			case 0:
				parser = new SAXParser();
				break;
			case 1:
				parser = new DOMParser();
				break;
			case 2:
				parser = new LINQParser();
				break;
		}
		await ValidateFile();

		if (ChosenFile is not null && parser is not null)
		{
			MakeSearch();
		}
	}
}
