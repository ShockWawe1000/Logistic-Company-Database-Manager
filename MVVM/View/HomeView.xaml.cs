using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace K_project.MVVM.View
{

    public partial class HomeView : UserControl
    {
        private System.Windows.Threading.DispatcherTimer timer;
        private IFirebaseClient client;
        private Dictionary<string, string> variables;
        private void AutoCompleteBox_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            var _acb = sender as AutoCompleteBox;
            if (_acb != null && string.IsNullOrEmpty(_acb.Text))
            {
                _acb.Dispatcher.BeginInvoke((Action)(() => { _acb.IsDropDownOpen = true; }));
            }
        }
        
        public HomeView()
        {
            InitializeComponent();
            DataContext = this;

            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = "MIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQDBHg5yoVxrdlrh\nULYV6A909b1YOk7qKgDURxfjjWFdlEpATaO11dyeSFFNnhC8LOSqYwdBK1lpH5wE\nGfDYkRMf9kKQ/aoplDGnYIxeeT3KHi2Z1udgElbM/cZtsjAD7GnAyJlgdiO78DxS\ng/JiouNlq8r3g115i0oTzD9mKVy8HMtw4oCpybz/UdOQqBO9TlfFWb5HCxvTfbwK\nhkAUnTmTVIlLyBbyKkYkVyzBX5ZIkUqetdE7Swk1lAnAa01ulYvi0pWPQfRrXf2c\n10ZGBG9nhAKw/2tvnbtExsfAyaDECZC7/TqNJhgl+HDbYwsS6TYwxXox2TZs4HDy\nx8rVs3nfAgMBAAECggEACMEvEH1GLkz4Q7yf9JBHmxrE0aobB7yNuxHaAGPZZR+i\n2hVq9YRTcw1+OhGPWqiCWUMSAh9P91cKgJkYdQjmoA1O6TnGrAne0mKrGmFOrsn9\ncTrv1EggjTx38WMhkBv4k1fEtD3G0u9KHMeEhnox6tsF4YCgDTGGUy1wNlZrsM9d\nI+5MIL09lN7dA0zOZgK56JifDOknQz2WcpOQfsmNKZUvBjbG9aFksYCIfCUonA2V\n9yERH+ca7HjfR2OKdj231VPFTnq2g742QilZOW0DjCfy8cXVW+Y/1UIGvF9sy9tD\nJexVBfqAxo4kaDfmxrkYZiNha1U07JnrohDlrSRIaQKBgQDxDxvuNRNh/Biz57lM\nZhEpfcNeqd7IbekncUliT5+CEqZIHzsxC1Xirmxn6MdhRomGb20HPAl8PLhtGANl\ny/vtyBk9iGjN7y4WWpGJwPZqgiEcTomh8jg2E2Mni5Z96UUqQwgQJ4SLKdLabYYV\nwoxALfPATkdM6dY9Hq0+DIJFWwKBgQDNFkGcyu7aW1U5UsgzwPRKg6+QgrPZsjHy\na2VxxdlF1njzlxGqU15mJAoic0syD16IwSk69rSvC1Clrsw3GWhpBaW6wHLfGaLv\naZkcs2RGG/kUP8EVxR7jEglpBuojhQbVhlu1RS/rKN/L0jhhkjSJ2iryRDHQikqM\naJvSUZjQzQKBgAt9oOM1/HqLPdI1lYuiweasbAezKT98ncSXjdv117Cnmu2NL5Ei\n7TElB13cpsRoTF3wKc5SelFFw7TPlDniA1xOUYWxXu2SHSxLnOxoGlbxZQqoY78o\ngK1zNyLHcKEH4ZgmIdhDSfooQDjRR7b50x2sExZzpMpxyxiWwcNXbU5JAoGBAJYi\nyeunoA3YON6bHJbOlcgK+TaYiGNBEVF/j6cniLSYmrjwie0f1QZ/MbAgqRa/q5v/\nlFhnDMTsSqDQAw3/GrFvgfDiO/XZRa9wpbceGLU+eyx4s7hlMwRWSu9JMZTJKMSd\n9HsSaE8GADes8Lt1FeMpBLGQfMtGweZiGlhwhaetAoGBALGL6nP15+BOzpUwsffd\nrQ1WqK0gK4Th6Z4gNVDWne3z4jEozb2Qh+h1Q0bOqDbwoOkg2b5dnA0RRgriiOl3\nBf9DfrBu3uRAVKLZWj4LfloYtc/FZHH2vkKgYg9uo8DurCnBoZ56VWRCmkJyizde\noI4d2Be220cgt1CyvsrRdkXH",
                BasePath = "https://logindb-26c0e-default-rtdb.europe-west1.firebasedatabase.app/"
            };

            client = new FireSharp.FirebaseClient(config);

            Loaded += HomeView_Loaded;
        }
     

        private void LdmAutoComplete_TextChanged(object sender, RoutedEventArgs e)
        {
         
        }


        private async void HomeView_Loaded(object sender, RoutedEventArgs e)
        {
            await PopulateStartDestinations();
            await PopulateEndDestinations();
            await PopulateLDM();
            
        }

        private async Task PopulateStartDestinations()
        {
            FirebaseResponse response = await client.GetAsync("/Start");
            Dictionary<string, string> startDestinations = response.ResultAs<Dictionary<string, string>>();
            startAutoComplete.ItemsSource = startDestinations.Keys;
        }

        private async Task PopulateLDM()
        {
            FirebaseResponse response = await client.GetAsync("/KG");
            Dictionary<string, string> ldmData = response.ResultAs<Dictionary<string, string>>();
            ldmAutoComplete.ItemsSource = ldmData.Keys;
        }

        private async Task PopulateEndDestinations()
        {
            FirebaseResponse response = await client.GetAsync("/End");
            Dictionary<string, string> endDestinations = response.ResultAs<Dictionary<string, string>>();
            endAutoComplete.ItemsSource = endDestinations.Keys;
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
           
            if (startAutoComplete.SelectedItem != null &&
                endAutoComplete.SelectedItem != null &&
                ldmAutoComplete.SelectedItem != null)
            {
                string variableName = $"{startAutoComplete.SelectedItem}{endAutoComplete.SelectedItem}{ldmAutoComplete.SelectedItem}";

                FirebaseResponse response = await client.GetAsync($"combinations/{variableName}/Variables");
                Dictionary<string, VariableData> variables = response.ResultAs<Dictionary<string, VariableData>>();

                if (variables != null)
                {
                    List<VariableData> variableDataList = variables.Values.ToList();

                    // Update the values from the DataGrid
                    foreach (VariableData variableData in variableDataList)
                    {
                        DataGridCell cell = GetCell(DataGridche, variableData.Name, "Value");
                        if (cell != null)
                        {
                            TextBox textBox = FindVisualChild<TextBox>(cell);
                            if (textBox != null)
                            {
                                variableData.Value = textBox.Text;
                            }
                        }
                    }

                    // Sort the values numerically
                    variableDataList.Sort((x, y) =>
                    {
                        int xValue, yValue;
                        if (!int.TryParse(x.Value, out xValue))
                            return -1; // Handle non-numeric values
                        if (!int.TryParse(y.Value, out yValue))
                            return 1; // Handle non-numeric values
                        return xValue.CompareTo(yValue);
                    });

                    // Save the changes back to the database
                    var updateData = new Dictionary<string, VariableData>();
                    foreach (VariableData variableData in variableDataList)
                    {
                        updateData[variableData.Name] = variableData;
                    }

                    FirebaseResponse updateResponse = await client.UpdateAsync($"combinations/{variableName}/Variables", updateData);

                    // Clear the existing items from the DataGrid
                    DataGridche.ItemsSource = null;
                    DataGridche.Items.Clear();

                    // Rebind the data to the DataGrid
                    DataGridche.ItemsSource = variableDataList;
                }
            }
        }
        private void LdmAutoComplete_SelectionChanged(object sender, RoutedEventArgs e)
        {
            string selectedText = ldmAutoComplete.Text;

            // Remove the comma from the selected text and replace "LDM" with an empty string
            string cleanedText = selectedText.Replace(",", "").Replace("LDM", "");

            // Parse the cleaned text into a double
            if (double.TryParse(cleanedText, NumberStyles.Number, CultureInfo.InvariantCulture, out double number))
            {
                // Calculate the weight based on the formula
                double weight = (number / 0.4) * 700 / 10;

                // Update the Text property with the calculated weight
                Text.Text = weight.ToString() + " Max KG";
            }
            else
            {
                // Handle the case when the selected text is not a valid number
                Text.Text = "Invalid number";
            }
        }
        // Helper method to get the DataGridCell based on row and column
        private DataGridCell GetCell(DataGrid dataGrid, string rowName, string columnName)
        {
            this.DataGridche.Resources["columnForeground"] = Brushes.White;
            
            DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(rowName);
            if (row != null)
            {
                int columnIndex = dataGrid.Columns.IndexOf(dataGrid.Columns.FirstOrDefault(c => c.Header.ToString() == columnName));
                if (columnIndex >= 0)
                {
                    DataGridCellsPresenter presenter = FindVisualChild<DataGridCellsPresenter>(row);
                    if (presenter != null)
                    {
                        return presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex) as DataGridCell;
                    }
                }
            }
            return null;
        }

        // Helper method to find a child control within a parent control
        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is T)
                {
                    return (T)child;
                }
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                    {
                        return childOfChild;
                    }
                }
            }
            return null;
        }


        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            // Your event handling code here
        }
        public class VariableData
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        private void Text_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }

}