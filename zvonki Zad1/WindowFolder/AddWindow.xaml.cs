using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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

namespace zvonki_Zad1.WindowFolder
{
    /// <summary>
    /// Логика взаимодействия для AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        SqlConnection sqlConnection = new SqlConnection(
        @"Data Source=DESKTOP-Q9BEC2K;Initial Catalog=zad1;Integrated Security=True");
        SqlCommand command = new SqlCommand();

        public AddWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sqlConnection.Open();
                command = new SqlCommand($"Insert Into dbo.Zvonki (UserWhoZvon, Login, Password, ZvonokDlinna) Values ('{ktozvon.Text}', '{login.Text}', '{pass.Text}', '{dlinnaa.Text}')",sqlConnection);
                command.ExecuteNonQuery();
                MessageBox.Show("done");

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally { sqlConnection.Close(); }
        }

        private void BackButton_Click_1(object sender, RoutedEventArgs e)
        {
            new PhoneList().Show();
            Close();    
        }
    }
}
