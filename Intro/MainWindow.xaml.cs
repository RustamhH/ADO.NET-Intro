using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Intro
{
    
    public partial class MainWindow : Window
    {


        
        SqlConnection? connection = null;
        SqlDataReader? reader = null;
        bool isconstraintdeleted = false;

        public MainWindow()
        {
            string constr = "Data Source=HOMEPC\\SQLEXPRESS;Initial Catalog=Library;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            connection = new SqlConnection(constr);
            InitializeComponent();
            ReadCategoriesDataFromSql();
        }

        public void ReadAuthorsDataFromSql()
        {

            try
            {
                connection.Open();
                SqlDataReader? reader = null;
                string catname = CategoryComboBox.SelectedItem.ToString();

                using SqlCommand cmd = new SqlCommand($"SELECT DISTINCT Authors.* FROM Authors INNER JOIN Books ON Authors.Id = Books.Id_Author INNER JOIN Categories ON Categories.Id = Books.Id_Category WHERE Categories.Name = '{catname}'", connection);

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    AuthorComboBox.Items.Add($"{reader["FirstName"]} {reader["LastName"]}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }


        }


        public void ReadCategoriesDataFromSql()
        {

            try
            {
                connection.Open();
                SqlDataReader? reader = null;

                using SqlCommand cmd = new SqlCommand("SELECT * FROM Categories", connection);

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    CategoryComboBox.Items.Add($"{reader["Name"]}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }


        }








        

        

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AuthorComboBox.Items.Clear();
            ReadAuthorsDataFromSql();
        }

       

        private void SearchBookButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(BooksNameTextBox.Text)) return;
            try
            {
                connection?.Open();
                

                using SqlCommand cmd = new SqlCommand($"SELECT * FROM BOOKS WHERE Name='{BooksNameTextBox.Text}'", connection);
                reader = cmd.ExecuteReader();

                DataTable table = new DataTable();


                bool isColumnName = true;
                do
                {
                    while (reader.Read())
                    {
                        if (isColumnName)
                            for (int i = 0; i < reader.FieldCount; i++)
                                table.Columns.Add(reader.GetName(i));

                        isColumnName = false;


                        DataRow newRow = table.NewRow();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            newRow[i] = reader[i];
                        }

                        table.Rows.Add(newRow);
                    }
                } while (reader.NextResult());

                DataGrid.ItemsSource = table.DefaultView;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection?.Close();
            }
        }

        private void DeleteBookButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(BooksNameTextBox.Text)) return;
            try
            {
                connection.Open();

                string query;
                if (isconstraintdeleted) query = $"DELETE FROM BOOKS WHERE Name='{BooksNameTextBox.Text}'";
                else
                {
                    query = $"ALTER TABLE S_Cards DROP Constraint FK_S_Cards_Book DELETE FROM BOOKS WHERE Name='{BooksNameTextBox.Text}'";
                    isconstraintdeleted = true;
                }

                using SqlCommand cmd = new SqlCommand(query, connection);
                reader = cmd.ExecuteReader();
                MessageBox.Show("Deletion Successful");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }


        

        private void InsertBookButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataGrid.Items.Count==0) return; 
            DataRowView rowView = DataGrid.Items[DataGrid.Items.Count - 2] as DataRowView;

            try
            {
                connection.Open();

                string query = $"INSERT INTO Books(Id,Name,Pages,YearPress,Id_Themes,Id_Category,Id_Author,Id_Press,Comment,Quantity) VALUES({rowView.Row.ItemArray[0]},'{rowView.Row.ItemArray[1]}',{rowView.Row.ItemArray[2]},{rowView.Row.ItemArray[3]},{rowView.Row.ItemArray[4]},{rowView.Row.ItemArray[5]},{rowView.Row.ItemArray[6]},{rowView.Row.ItemArray[7]},'{rowView.Row.ItemArray[8]}',{rowView.Row.ItemArray[9]})";

                using SqlCommand cmd = new SqlCommand(query, connection);
                reader = cmd.ExecuteReader();
                MessageBox.Show("Insertion Successful");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }

    
}
