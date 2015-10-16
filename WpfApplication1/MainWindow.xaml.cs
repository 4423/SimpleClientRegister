using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

namespace WpfApplication1
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataClassesDataContext context;
        private ObservableCollection<Student> observableStudent = new ObservableCollection<Student>();

        public MainWindow()
        {
            InitializeComponent();
        }
        

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string nameText = this.textBox1.Text;
            string schoolText = this.textBox.Text;

            // school変数に学校名を設定
            School school = new School();
            school.Name = schoolText;

            // データベースにschoolを登録
            this.context.School.InsertOnSubmit(school);
            this.context.SubmitChanges();

            // student変数に氏名を設定
            Student student = new Student();
            student.Name = nameText;
            student.SchoolId = school.Id;

            // データベースにstudentを登録
            this.context.Student.InsertOnSubmit(student);
            this.context.SubmitChanges();
            
            // 画面表示用コレクションにschoolを登録
            this.observableStudent.Add(student);
            this.dataGrid.ItemsSource = this.observableStudent;        
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // データベースに接続する文字列を作成
            // メモ：C#6.0記法$"{ }"において、" と { の間で改行すると生成される文字列に改行が含まれるっぽい 
            string connection = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""{ 
                                Directory.GetCurrentDirectory() }\Database.mdf"";Integrated Security=True";

            // データベースに接続
            this.context = new DataClassesDataContext(connection);

            // 画面表示用コレクションをデータベースに格納されているStudent全てで初期化
            this.observableStudent = new ObservableCollection<Student>(this.context.Student);
            this.dataGrid.ItemsSource = this.observableStudent;
        }
    }
}
