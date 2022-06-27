using BespokeFusion;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace fileupload.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        private string from_text;

        public string From_text
        {
            get { return from_text; }
            set { from_text = value; OnPropertyChanged(); }
        }
        private bool isencrypt;

        public bool Isencrypt
        {
            get { return isencrypt; }
            set { isencrypt = value; }
        }
        private bool isdecrypt;

        public bool Isdecrypt
        {
            get { return isdecrypt; }
            set { isdecrypt = value; }
        }
        private int value1;

        public int Value
        {
            get { return value1; }
            set { value1 = value; OnPropertyChanged(); }
        }
        private int password;

        public int Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged(); }
        }
        public RelayCommand Fromfile_button
        {
            get => new RelayCommand(() =>
            {


                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Multiselect = true;
                openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                if (openFileDialog.ShowDialog() == true)
                    From_text = (openFileDialog.FileName).ToString();

            });
        }
        public RelayCommand Start
        {
            get => new RelayCommand(() =>
            {
                try
                {
                    int LAZIM = int.Parse(Password.ToString());
                }
                catch (Exception)
                {
                    MaterialMessageBox.ShowError(@"DAYANDI!!!!!!!!!!!!!!!");
                    return;
                }
                ThreadPool.QueueUserWorkItem((o) =>
                {
                    Prosers(cancel.Token);
                });



            });
        }

        public RelayCommand Cancel
        {
            get => new RelayCommand(() =>
            {

                cancel.Cancel();


            });
        }

        private void Prosers( CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                cancel.Cancel();
                MaterialMessageBox.ShowError(@"DAYANDI!!!!!!!!!!!!!!!"); return;
            }
            
            if (Isencrypt==true)
            {
                encrypt(From_text, Password,cancel.Token);
            }
            if (Isdecrypt==true)
            {
                decrypt(From_text, Password,cancel.Token);
            }
        }

    
        private void encrypt(string inputFile, int password, CancellationToken token)  
        {



            FileStream fsCrypt = new FileStream(inputFile + "cevrilmis.txt", FileMode.Create);
            fsCrypt.Close();
            Value=0;
            using (FileStream fsRead = new FileStream(From_text, FileMode.Open, FileAccess.Read))
            {


                using (FileStream fsWrite = new FileStream(fsCrypt.Name, FileMode.Open, FileAccess.Write))
                {
                    var len = 10;
                    var fileSize = fsRead.Length;
                    byte[] buffer = new byte[len];

                    do
                    {
                        if (token.IsCancellationRequested)
                        {
                            cancel.Cancel();
                            MaterialMessageBox.ShowError(@"DAYANDI!!!!!!!!!!!!!!!"); return;
                        }

                        int option = 100 * len;
                        int lazimli = (int)(option / (int)fsRead.Length);
                        Value = Value + lazimli;
                        var sb = new StringBuilder();
                        len = fsRead.Read(buffer, 0, buffer.Length);
                        for (int i = 0; i < buffer.Length; i++)
                        {
                            sb.Append(buffer[i]^Password);
                        }
                
                        buffer = Encoding.ASCII.GetBytes(sb.ToString());
                        fsWrite.Write(buffer, 0, len);
                        Thread.Sleep(100);





                    } while (len != 0);

                    MaterialMessageBox.ShowError(@"Transfer tamamlandi!!!!!!!!!!!!");

                }
            }
        }
        private void decrypt(string inputFile, int password, CancellationToken token)
        {


            Value=0;
            FileStream fsCrypt = new FileStream(inputFile + "cevrilmis.txt", FileMode.Create);
            fsCrypt.Close();
            using (FileStream fsRead = new FileStream(From_text, FileMode.Open, FileAccess.Read))
            {


                using (FileStream fsWrite = new FileStream(fsCrypt.Name, FileMode.Open, FileAccess.Write))
                {
                    var len = 10;
                    var fileSize = fsRead.Length;
                    byte[] buffer = new byte[len];

                    do
                    {
                        if (token.IsCancellationRequested)
                        {
                            cancel.Cancel();
                            MaterialMessageBox.ShowError(@"DAYANDI!!!!!!!!!!!!!!!");
                            return;
                        }

                        int option = 100 * len;
                        int lazimli = (int)(option / (int)fsRead.Length);
                        Value = Value + lazimli;
                        var sb = new StringBuilder();
                        len = fsRead.Read(buffer, 0, buffer.Length);
                        for (int i = 0; i < buffer.Length; i++)
                        {
                            sb.Append(buffer[i]^Password);
                        }

                        buffer = Encoding.ASCII.GetBytes(sb.ToString());
                        fsWrite.Write(buffer, 0, len);
                        Thread.Sleep(100);





                    } while (len != 0);

                    MaterialMessageBox.ShowError(@"Transfer tamamlandi!!!!!!!!!!!!");

                }
            }
        }
        public CancellationTokenSource cancel { get; set; }=new CancellationTokenSource();

        public RelayCommand<object> Encrypt { get; set; }
    public MainViewModel()
        {
            CancellationTokenSource cancel = new CancellationTokenSource();
            
        }
    }
}
