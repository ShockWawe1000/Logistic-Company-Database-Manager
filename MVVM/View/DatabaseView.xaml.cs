using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Windows;
using System.Windows.Controls;
using static K_project.MVVM.View.DatabaseView;

namespace K_project.MVVM.View
{
    /// <summary>
    /// Interaction logic for DatabaseView.xaml
    /// </summary>
    public partial class DatabaseView : UserControl
    {
        private IFirebaseClient client;

        public DatabaseView()
        {
            InitializeComponent();
            InitializeFirebaseClient();
            Loaded += DatabaseView_Loaded;
            backButton.Click += BackButton_Click;
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Revert the DataGrid to display the main variables
            DataGridche.ItemsSource = combinationMainVariables;

            // Re-attach the event handler to the DataGrid's SelectionChanged event
            DataGridche.SelectionChanged += DataGrid_SelectionChanged;
            backButton.Visibility = Visibility.Visible;
        }

        private void InitializeFirebaseClient()
        {
            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = "MIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQDBHg5yoVxrdlrh\nULYV6A909b1YOk7qKgDURxfjjWFdlEpATaO11dyeSFFNnhC8LOSqYwdBK1lpH5wE\nGfDYkRMf9kKQ/aoplDGnYIxeeT3KHi2Z1udgElbM/cZtsjAD7GnAyJlgdiO78DxS\ng/JiouNlq8r3g115i0oTzD9mKVy8HMtw4oCpybz/UdOQqBO9TlfFWb5HCxvTfbwK\nhkAUnTmTVIlLyBbyKkYkVyzBX5ZIkUqetdE7Swk1lAnAa01ulYvi0pWPQfRrXf2c\n10ZGBG9nhAKw/2tvnbtExsfAyaDECZC7/TqNJhgl+HDbYwsS6TYwxXox2TZs4HDy\nx8rVs3nfAgMBAAECggEACMEvEH1GLkz4Q7yf9JBHmxrE0aobB7yNuxHaAGPZZR+i\n2hVq9YRTcw1+OhGPWqiCWUMSAh9P91cKgJkYdQjmoA1O6TnGrAne0mKrGmFOrsn9\ncTrv1EggjTx38WMhkBv4k1fEtD3G0u9KHMeEhnox6tsF4YCgDTGGUy1wNlZrsM9d\nI+5MIL09lN7dA0zOZgK56JifDOknQz2WcpOQfsmNKZUvBjbG9aFksYCIfCUonA2V\n9yERH+ca7HjfR2OKdj231VPFTnq2g742QilZOW0DjCfy8cXVW+Y/1UIGvF9sy9tD\nJexVBfqAxo4kaDfmxrkYZiNha1U07JnrohDlrSRIaQKBgQDxDxvuNRNh/Biz57lM\nZhEpfcNeqd7IbekncUliT5+CEqZIHzsxC1Xirmxn6MdhRomGb20HPAl8PLhtGANl\ny/vtyBk9iGjN7y4WWpGJwPZqgiEcTomh8jg2E2Mni5Z96UUqQwgQJ4SLKdLabYYV\nwoxALfPATkdM6dY9Hq0+DIJFWwKBgQDNFkGcyu7aW1U5UsgzwPRKg6+QgrPZsjHy\na2VxxdlF1njzlxGqU15mJAoic0syD16IwSk69rSvC1Clrsw3GWhpBaW6wHLfGaLv\naZkcs2RGG/kUP8EVxR7jEglpBuojhQbVhlu1RS/rKN/L0jhhkjSJ2iryRDHQikqM\naJvSUZjQzQKBgAt9oOM1/HqLPdI1lYuiweasbAezKT98ncSXjdv117Cnmu2NL5Ei\n7TElB13cpsRoTF3wKc5SelFFw7TPlDniA1xOUYWxXu2SHSxLnOxoGlbxZQqoY78o\ngK1zNyLHcKEH4ZgmIdhDSfooQDjRR7b50x2sExZzpMpxyxiWwcNXbU5JAoGBAJYi\nyeunoA3YON6bHJbOlcgK+TaYiGNBEVF/j6cniLSYmrjwie0f1QZ/MbAgqRa/q5v/\nlFhnDMTsSqDQAw3/GrFvgfDiO/XZRa9wpbceGLU+eyx4s7hlMwRWSu9JMZTJKMSd\n9HsSaE8GADes8Lt1FeMpBLGQfMtGweZiGlhwhaetAoGBALGL6nP15+BOzpUwsffd\nrQ1WqK0gK4Th6Z4gNVDWne3z4jEozb2Qh+h1Q0bOqDbwoOkg2b5dnA0RRgriiOl3\nBf9DfrBu3uRAVKLZWj4LfloYtc/FZHH2vkKgYg9uo8DurCnBoZ56VWRCmkJyizde\noI4d2Be220cgt1CyvsrRdkXH",
                BasePath = "https://logindb-26c0e-default-rtdb.europe-west1.firebasedatabase.app/"
            };

            client = new FireSharp.FirebaseClient(config);
        }

        private async void DatabaseView_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                FirebaseResponse response = await client.GetAsync("combinations");
                Dictionary<string, CombinationData> combinationDataDict = response.ResultAs<Dictionary<string, CombinationData>>();

                combinationMainVariables = new List<CombinationMainVariable>();

                foreach (var combinationDataEntry in combinationDataDict)
                {
                    string combinationName = combinationDataEntry.Key;
                    CombinationData combinationData = combinationDataEntry.Value;

                    CombinationMainVariable mainVariable = new CombinationMainVariable
                    {
                        CombinationName = combinationName,
                        Variables = combinationData.Variables.ToDictionary(kv => kv.Key, kv => kv.Value)
                    };

                    combinationMainVariables.Add(mainVariable);
                }

                DataGridche.ItemsSource = combinationMainVariables;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }



        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the selected combination main variable
            CombinationMainVariable selectedVariable = (CombinationMainVariable)DataGridche.SelectedItem;

            // Check if a variable is selected
            if (selectedVariable != null)
            {
                // Check if the selected variable has any subvariables
                if (selectedVariable.Variables != null && selectedVariable.Variables.Count > 0)
                {
                    // Display the subvariables and their values in the DataGrid
                    DataGridche.ItemsSource = selectedVariable.Variables.Select(kv => new CombinationSubVariable
                    {
                        Name = kv.Key,
                        Value = kv.Value.Value
                    });
                }
                else
                {
                    // No subvariables found, return without performing any action
                    return;
                }
            }

            // Remove the event handler to disable further selection changes
            DataGridche.SelectionChanged -= DataGrid_SelectionChanged;
        }


        public class CombinationData
        {
            public Dictionary<string, CombinationVariable> Variables { get; set; }
        }

        public class CombinationVariable
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }
        public class CombinationMainVariable
        {
            public string Value { get; set; }
            public string CombinationName { get; set; }
            public Dictionary<string, CombinationVariable> Variables { get; set; }
        }
        public class CombinationSubVariable
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }
        private List<CombinationMainVariable> combinationMainVariables;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DataGridche.ItemsSource = combinationMainVariables;
            DataGridche.SelectionChanged += DataGrid_SelectionChanged;
            backButton.Visibility = Visibility.Collapsed;
        }
    }
}