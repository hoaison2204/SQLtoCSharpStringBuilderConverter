using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SQLtoCSharpStringBuilder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Deactivated += MainWindow_Deactivated;
        }

        private void MainWindow_Deactivated(object? sender, EventArgs e)
        {
            txtCopyStatus.Visibility = Visibility.Collapsed;
        }

        private void ConvertButton_Click(object sender, RoutedEventArgs e)
        {
            string sql = txtSqlInput.Text;

            string csharp = ConvertSqlToCSharp(sql);

            txtCSharpOutput.Text = csharp;

            btnCopyToClipboard.IsEnabled = true;
        }
        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            txtSqlInput.Text = "";
            txtCSharpOutput.Text = "";
            btnCopyToClipboard.IsEnabled = false;
        }
        private void BtnPaste_Click(object sender, RoutedEventArgs e)
        {
            BtnClear_Click(sender, e);
            txtSqlInput.Text = Clipboard.GetText();
            ConvertButton_Click(sender, e);
        }
        private void CopyToClipboardButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(txtCSharpOutput.Text, TextDataFormat.UnicodeText);

            txtCopyStatus.Text = "Copied to clipboard!";
            txtCopyStatus.Visibility = Visibility.Visible;
        }

        private string ConvertSqlToCSharp(string sql)
        {
            StringBuilder csharp = new();

            string[] separators = ["\r\n", "\r", "\n"];
            string[] lines = sql.Split(separators, StringSplitOptions.None);
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    csharp.AppendLine();
                }
                else if (line.Contains('{') || line.Contains('}'))
                {
                    csharp.AppendLine($"sql.AppendLine($\"{line.TrimEnd()}\");");
                }
                else
                {
                    csharp.AppendLine($"sql.AppendLine(\"{line.TrimEnd()}\");");
                }
            }

            return csharp.ToString();
        }

        private void SqlTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtSqlInput.Text))
            {
                btnConvert.IsEnabled = true;
                btnClear.IsEnabled = true;
            }
            else
            {
                btnConvert.IsEnabled = false;
                btnClear.IsEnabled = false;
            }
        }

    }
}
