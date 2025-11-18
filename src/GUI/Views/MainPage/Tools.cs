using XML_Utils;
using System.Diagnostics;
using System.Text;

namespace GUI;

public partial class MainPage : ContentPage
{
	private Filters CollectFilters()
	{
		var filters = new Filters();
		if (NameCheckbox.IsChecked)
		{
			filters.Name = NameEntry.Text ?? "";
		}

		if (FacultyCheckbox.IsChecked)
		{
			filters.Faculty = FacultyEntry.Text ?? "";
		}

		if (ChairCheckbox.IsChecked)
		{
			filters.Chair = ChairEntry.Text ?? "";
		}

		if (SubjectCheckBox.IsChecked)
		{
			filters.Subject = SubjectEntry.Text ?? "";
		}

		if (DateCheckBox.IsChecked)
		{
			filters.Date = DateEntry.Text ?? "";
		}

		if (GroupCheckBox.IsChecked)
		{
			filters.Group = GroupEntry.Text ?? "";
		}

		if (AudienceCheckBox.IsChecked)
		{
			filters.Audience = AudienceEntry.Text ?? "";
		}

		return filters;
	}

	private void ClearFilters()
	{
		NameEntry.Text = "";
		NameCheckbox.IsChecked = false;
		ChairEntry.Text = "";
		ChairCheckbox.IsChecked = false;
		FacultyEntry.Text = "";
		FacultyCheckbox.IsChecked = false;
		SubjectEntry.Text = "";
		SubjectCheckBox.IsChecked = false;
		DateEntry.Text = "";
		DateCheckBox.IsChecked = false;
		GroupEntry.Text = "";
		GroupCheckBox.IsChecked = false;
		AudienceEntry.Text = "";
		AudienceCheckBox.IsChecked = false;
	}

	private async Task ValidateFile()
	{
		if (parser == null || ChosenFile == null)
		{
			return;
		}
		if (parser.Load(await ChosenFile.OpenReadAsync(), validationSettings))
		{
			return;
		}

		Title = "XML Labwork - File is not chosen";
		ChosenFile = null;
		await DisplayAlert("Invalid file", "The file does not satisfy XSD Schema", "Ok");
	}
	private async Task CopyFileToAppDataDirectory(string filename)
	{
		using Stream inputStream = await FileSystem.Current.OpenAppPackageFileAsync(filename);

		string targetFile = Path.Combine(AppDataDirectory, filename);

		using FileStream outputStream = File.Create(targetFile);
		await inputStream.CopyToAsync(outputStream);
	}

	private void InitAppData()
	{
		AppDataDirectory = Path.Combine(FileSystem.AppDataDirectory);
		if (!Path.Exists(AppDataDirectory))
		{
			while (!Path.Exists(AppDataDirectory))
			{
				AppDataDirectory = Directory.GetParent(AppDataDirectory).FullName;
			}

			AppDataDirectory = Path.Combine(AppDataDirectory, "XML Labwork");

			if (!Path.Exists(AppDataDirectory))
			{
				Directory.CreateDirectory(AppDataDirectory);
			}
		}
	}

	private void ClearResults()
	{
		while (GridForResults.Children.Count > 6)
		{
			GridForResults.Children.RemoveAt(6);
		}

		while (GridForResults.RowDefinitions.Count > 1)
		{
			GridForResults.RowDefinitions.RemoveAt(1);
		}
	}

	private void MakeSearch()
	{
		var filterOptions = CollectFilters();
		var results = parser.Find(filterOptions);

		ClearResults();
		DisplayResults(results);
	}

	private void DisplayResults(IList<Class> results)
	{
		for (var i = 0; i < results.Count; ++i)
		{
			var result = results[i];

			GridForResults.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

			CreateLabel(i, 0, result.Person.Name.ToString());
			CreateLabel(i, 1, result.Person.Faculty);
			CreateLabel(i, 2, result.Person.Chair);
			CreateLabel(i, 3, result.Date.ToString());
			CreateLabel(i, 4, result.Audience);
			CreateButton(i, 5, result);
		}
	}

	private void CreateLabel(int row, int column, string text)
	{
		var label = new Label
		{
			HorizontalOptions = LayoutOptions.Center,
			VerticalOptions = LayoutOptions.Center,
			Text = text
		};

		GridForResults.SetRow(label, row + 1);
		GridForResults.SetColumn(label, column);
		GridForResults.Children.Add(label);
	}

	private void CreateButton(int row, int column, Class result)
	{
		var button = new Button
		{
			HorizontalOptions = LayoutOptions.Center,
			VerticalOptions = LayoutOptions.Center,
			Text = "Show"
		};

		button.Clicked += async (object sender, EventArgs args) => await DisplayAlert($"Students for {result.Person.Name}, {result.Subject} on {result.Date}", FormatStudents(result.Students), "Ok");

		GridForResults.SetRow(button, row + 1);
		GridForResults.SetColumn(button, column);
		GridForResults.Children.Add(button);
	}

	private string FormatStudents(IList<Student> students)
	{
		StringBuilder b = new();

		for (var i = 0; i < students.Count; ++i)
		{
			b.Append(students[i].Name.ToString());
			b.Append(", ");
			b.Append(students[i].Group);

			if (i != students.Count - 1)
			{
				b.Append('\n');
			}
		}

		return b.ToString();
	}
}
