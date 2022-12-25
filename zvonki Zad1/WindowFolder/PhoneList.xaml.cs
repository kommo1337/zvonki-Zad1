using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using zvonki_Zad1.ClassFolder;

namespace zvonki_Zad1.WindowFolder
{
    /// <summary>
    /// Логика взаимодействия для PhoneList.xaml
    /// </summary>
    public partial class PhoneList : Window
    {
        SqlConnection sqlConnection = new SqlConnection(
        @"Data Source=DESKTOP-Q9BEC2K;Initial Catalog=zad1;Integrated Security=True");
        string selectZvonkiSql = "SELECT * FROM dbo.Zvonki";
        string selectLoginsSql = "SELECT Login FROM dbo.[User]";
        string selectDlinnaSql = "SELECT ZvonokDlinna FROM dbo.Zvonki";

        SqlConnection connection = null;
        SqlCommand command = null;
        SqlDataReader reader = null;
        DataTable dataTable = null;
        DataSet dataSet = new DataSet();
        DGClass dGClass;
        public PhoneList()
        {
            InitializeComponent();
            dGClass = new DGClass(callDataGrid);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                connection = new SqlConnection(@"Data Source=DESKTOP-Q9BEC2K;Initial Catalog=zad1;Integrated Security=True");
                connection.Open();

                
                command = new SqlCommand(selectZvonkiSql, connection);
                reader = command.ExecuteReader();

                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                callDataGrid.ItemsSource = dataTable.DefaultView;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dataSet, "Zvonki");

                
                callDataGrid.ItemsSource = dataSet.Tables["Zvonki"].DefaultView;

                reader.Close();
                command.Dispose();

                
                command = new SqlCommand(selectLoginsSql, connection);
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string login = reader.GetString(0);
                    userComboBox.Items.Add(login);
                }
                reader.Close();
                command.Dispose();
                                
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
               connection.Close();
            }
            userComboBox.SelectionChanged += userComboBox_SelectionChanged;
        }
        

        private void userComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedLogin = (string)userComboBox.SelectedItem;

            if (selectedLogin == null)
            {
                callDataGrid.ItemsSource = dataSet.Tables["Zvonki"].DefaultView;
            }
            else
            {
                DataTable dataTable = dataSet.Tables["Zvonki"];
                dataTable.DefaultView.RowFilter = $"Login = '{selectedLogin}'";
                callDataGrid.ItemsSource = dataTable.DefaultView;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new AddWindow().Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new DeleteWindow().Show();
            this.Close();
        }

        private void callDataGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void callDataGrid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (MessageBoxResult.Yes == MessageBox.Show("delete", "вы хотете удалиьт",MessageBoxButton.YesNo, MessageBoxImage.Question))
            {
                try
                {
                    sqlConnection.Open();
                    command = new SqlCommand($"DELETE FROM dbo.Zvonki Where Id='{dGClass.SelectId()}'", sqlConnection);
                    command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    throw;
                }
                finally { sqlConnection.Close(); }
            }
        }
    }
}
