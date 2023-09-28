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


        DataTable? table = null;
        DataSet? dataSet = null;
        SqlConnection? connection = null;
        SqlDataAdapter? adapter = null;
        SqlCommand? cmd = null;
        SqlDataReader? reader = null;

        public MainWindow()
        {
            string constr = "Data Source=HOMEPC\\SQLEXPRESS;Initial Catalog=Library;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            connection = new SqlConnection(constr);
            InitializeComponent();
        }

        


    }
}
