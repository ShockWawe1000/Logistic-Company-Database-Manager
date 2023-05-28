using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace K_project.MVVM.View
{
    /// <summary>
    /// Interaction logic for DiscoveryView.xaml
    /// </summary>
    public partial class DiscoveryView : UserControl
    {
        private IFirebaseClient client;

        public DiscoveryView()
        {
            InitializeComponent();

            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = "MIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQDBHg5yoVxrdlrh\nULYV6A909b1YOk7qKgDURxfjjWFdlEpATaO11dyeSFFNnhC8LOSqYwdBK1lpH5wE\nGfDYkRMf9kKQ/aoplDGnYIxeeT3KHi2Z1udgElbM/cZtsjAD7GnAyJlgdiO78DxS\ng/JiouNlq8r3g115i0oTzD9mKVy8HMtw4oCpybz/UdOQqBO9TlfFWb5HCxvTfbwK\nhkAUnTmTVIlLyBbyKkYkVyzBX5ZIkUqetdE7Swk1lAnAa01ulYvi0pWPQfRrXf2c\n10ZGBG9nhAKw/2tvnbtExsfAyaDECZC7/TqNJhgl+HDbYwsS6TYwxXox2TZs4HDy\nx8rVs3nfAgMBAAECggEACMEvEH1GLkz4Q7yf9JBHmxrE0aobB7yNuxHaAGPZZR+i\n2hVq9YRTcw1+OhGPWqiCWUMSAh9P91cKgJkYdQjmoA1O6TnGrAne0mKrGmFOrsn9\ncTrv1EggjTx38WMhkBv4k1fEtD3G0u9KHMeEhnox6tsF4YCgDTGGUy1wNlZrsM9d\nI+5MIL09lN7dA0zOZgK56JifDOknQz2WcpOQfsmNKZUvBjbG9aFksYCIfCUonA2V\n9yERH+ca7HjfR2OKdj231VPFTnq2g742QilZOW0DjCfy8cXVW+Y/1UIGvF9sy9tD\nJexVBfqAxo4kaDfmxrkYZiNha1U07JnrohDlrSRIaQKBgQDxDxvuNRNh/Biz57lM\nZhEpfcNeqd7IbekncUliT5+CEqZIHzsxC1Xirmxn6MdhRomGb20HPAl8PLhtGANl\ny/vtyBk9iGjN7y4WWpGJwPZqgiEcTomh8jg2E2Mni5Z96UUqQwgQJ4SLKdLabYYV\nwoxALfPATkdM6dY9Hq0+DIJFWwKBgQDNFkGcyu7aW1U5UsgzwPRKg6+QgrPZsjHy\na2VxxdlF1njzlxGqU15mJAoic0syD16IwSk69rSvC1Clrsw3GWhpBaW6wHLfGaLv\naZkcs2RGG/kUP8EVxR7jEglpBuojhQbVhlu1RS/rKN/L0jhhkjSJ2iryRDHQikqM\naJvSUZjQzQKBgAt9oOM1/HqLPdI1lYuiweasbAezKT98ncSXjdv117Cnmu2NL5Ei\n7TElB13cpsRoTF3wKc5SelFFw7TPlDniA1xOUYWxXu2SHSxLnOxoGlbxZQqoY78o\ngK1zNyLHcKEH4ZgmIdhDSfooQDjRR7b50x2sExZzpMpxyxiWwcNXbU5JAoGBAJYi\nyeunoA3YON6bHJbOlcgK+TaYiGNBEVF/j6cniLSYmrjwie0f1QZ/MbAgqRa/q5v/\nlFhnDMTsSqDQAw3/GrFvgfDiO/XZRa9wpbceGLU+eyx4s7hlMwRWSu9JMZTJKMSd\n9HsSaE8GADes8Lt1FeMpBLGQfMtGweZiGlhwhaetAoGBALGL6nP15+BOzpUwsffd\nrQ1WqK0gK4Th6Z4gNVDWne3z4jEozb2Qh+h1Q0bOqDbwoOkg2b5dnA0RRgriiOl3\nBf9DfrBu3uRAVKLZWj4LfloYtc/FZHH2vkKgYg9uo8DurCnBoZ56VWRCmkJyizde\noI4d2Be220cgt1CyvsrRdkXH",
                BasePath = "https://logindb-26c0e-default-rtdb.europe-west1.firebasedatabase.app/"
            };

            client = new FireSharp.FirebaseClient(config);
            Loaded += DiscoveryView_Loaded;
        }

        private async void DiscoveryView_Loaded(object sender, RoutedEventArgs e)
        {
            await PopulateStartDestinations();
            await PopulateEndDestinations();
            await PopulateLDM();
        }

        private async Task PopulateStartDestinations()
        {
            FirebaseResponse response = await client.GetAsync("Start");
            Dictionary<string, string> startDestinations = response.ResultAs<Dictionary<string, string>>();
            startAutoComplete.ItemsSource = startDestinations.Keys;
        }

        private async Task PopulateLDM()
        {
            FirebaseResponse response = await client.GetAsync("KG");
            Dictionary<string, string> ldmData = response.ResultAs<Dictionary<string, string>>();
            ldmAutoComplete.ItemsSource = ldmData.Keys;
        }

        private async Task PopulateEndDestinations()
        {
            FirebaseResponse response = await client.GetAsync("End");
            Dictionary<string, string> endDestinations = response.ResultAs<Dictionary<string, string>>();
            endAutoComplete.ItemsSource = endDestinations.Keys;
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string startDestination = startAutoComplete.SelectedItem as string;
            string endDestination = endAutoComplete.SelectedItem as string;
            string ldmValue = ldmAutoComplete.SelectedItem as string;
            string company = companyAutoComplete.Text;
            string price = priceAutoComplete.Text;

            string variableName = startDestination + endDestination + ldmValue;

            FirebaseResponse response = await client.GetAsync($"combinations/{variableName}/Variables");
            Dictionary<string, object> variableGroup = response.ResultAs<Dictionary<string, object>>();

            if (variableGroup == null)
                variableGroup = new Dictionary<string, object>();

            variableGroup[company] = new
            {
                Name = company,
                Value = price
            };
            if (string.IsNullOrEmpty(startAutoComplete.Text) ||
                string.IsNullOrEmpty(endAutoComplete.Text) ||
                string.IsNullOrEmpty(ldmAutoComplete.Text) ||
                string.IsNullOrEmpty(companyAutoComplete.Text) ||
                string.IsNullOrEmpty(priceAutoComplete.Text))
            {
                MessageBox.Show("Please fill all the fields.");
            }
            else
            {
                await client.UpdateAsync($"combinations/{variableName}/Variables", variableGroup);
                MessageBox.Show("Value added to the database successfully.");
            }





            ldmAutoComplete.Text = string.Empty;
            
            priceAutoComplete.Text = string.Empty;

            await PopulateStartDestinations();
            await PopulateEndDestinations();
            await PopulateLDM();
        }
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
    }
}