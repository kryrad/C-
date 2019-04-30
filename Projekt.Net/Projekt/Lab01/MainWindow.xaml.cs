using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Net;
using System.ComponentModel;
using System.IO;
using System.Timers;
using System.Data.Entity;

namespace Lab01
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        BackgroundWorker worker = new BackgroundWorker();
        PersonDbContext _db = new PersonDbContext();
        protected void UpdateProgressBlock(string text, TextBlock textBlock)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    textBlock.Text = text;
                });
            }
            catch { } 
        }

        private static System.Timers.Timer aTimer;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
      
            aTimer = new System.Timers.Timer();

            aTimer.Elapsed += new ElapsedEventHandler(OnTimeWorker);
            aTimer.Interval = 15000;
            aTimer.Start();

            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
        }

        private void OpenImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg;*.png;*.jpeg)|*.jpg;*.png;*.jpeg"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                PersonPicture = GetJPGFromImage(new BitmapImage(new Uri(openFileDialog.FileName)));
            }
        }
        public uint Number { get; set; }

        public byte[] GetJPGFromImage(BitmapImage image)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(memStream);
                return memStream.ToArray();
            }
        }

        private void AddNewPersonButton_Click(object sender, RoutedEventArgs e)
        {
            progressTextBlock3.Text = "";
            if (!String.IsNullOrEmpty(nameTextBox.Text)  && image.Source != null)
            {
                 people.Add(new Person {Name = nameTextBox.Text, Picture = GetJPGFromImage((BitmapImage)image.Source) });

                _db.People.Add(new Person()
                {
                    Name = nameTextBox.Text,
                    Picture = GetJPGFromImage((BitmapImage)image.Source),
                });

                PersonPicture = null;
                PersonName = String.Empty;
            }
            else
            {
                MessageBox.Show("Some fields are empty!");
            }
        }
        private void SavePersonButton_Click(object sender, RoutedEventArgs e)
        {
            progressTextBlock3.Text = "";
            if (!String.IsNullOrEmpty(nameTextBox.Text) && image.Source != null)
            {
                var id = Person.Id;
                var result = _db.People.SingleOrDefault(b => b.Id == id);
                if (result != null)
                {
                    result.Name = PersonName;
                    result.Picture = PersonPicture;
                }
                
                var index = People.IndexOf(Person);
                People[index] = new Person()
                {
                    Picture = PersonPicture,
                    Name = PersonName,
                };
                PersonPicture = null;
                PersonName = String.Empty;
            }
            else
            {
                MessageBox.Show("Some fields are empty!");
            }
        }
        private void RemovePersonButton_Click(object sender, RoutedEventArgs e)
        {
            progressTextBlock3.Text = "";
            if (Person!=null)
            {
                var remove = new Person { Id = Person.Id };
                if (remove.Id > -1)
                {
                    var local = _db.Set<Person>()
                                 .Local
                                 .FirstOrDefault(f => f.Id == Person.Id);
                    if (local != null)
                    {
                        _db.Entry(local).State = EntityState.Detached;
                    }
                    _db.Entry(remove).State = EntityState.Modified;
                    _db.People.Attach(remove);
                    _db.People.Remove(remove);
                }
                People.Remove(Person);

                PersonPicture = null;
                PersonName = String.Empty;
            }
        }
        public byte[] GetJPGFromUrl(string image)
        {
            byte[] imageBytes;
            using (var webClient = new WebClient())
            {
                imageBytes = webClient.DownloadData(new Uri(image)); 
            }
            return imageBytes;
        }

        private void AddPerson(string name,String image)
        {
            try
            { 
                Application.Current.Dispatcher.Invoke(() =>
                {
                    people.Add(new Person { Name = name, Picture = GetJPGFromUrl(image) });
                });
            }
            catch { } 
        }

    private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (worker.IsBusy != true)
                worker.RunWorkerAsync();
        }

        private void OnTimeWorker(object sender, ElapsedEventArgs e)
        {        
            if (worker.IsBusy != true)
                worker.RunWorkerAsync();
        }       
        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            { 
                Application.Current.Dispatcher.Invoke(() =>
                {
                    progressBar.Value = e.ProgressPercentage;
                    progressTextBlock2.Text = e.UserState as string;
                    progressTextBlock3.Text = "";
            });
            }
            catch { } 
        }

    private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {         
            aTimer.Stop();
            BackgroundWorker worker = sender as BackgroundWorker;
            for (int i = 0; i < Number; i++)
            {
                if (worker.CancellationPending == true)
                {
                    worker.ReportProgress(0, "Cancelled");
                    e.Cancel = true;
                    return;
                }
                else
                {
                    worker.ReportProgress(
                        (int)Math.Round((float)i * 100.0 / (float)Number),
                        "Loading...");

                    string response = WikiConnection.LoadDataAsync().Result;
                    WikiDataEntry result;
                    using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(response)))
                    {
                        result = WikiParser.Parse(response);
                        AddPerson(result.Title, result.Picture);
                        _db.People.Add(new Person { Name = result.Title,  Picture = GetJPGFromUrl(result.Picture) });
                    }
                
                    Thread.Sleep(2000);
                }
            }
            worker.ReportProgress(100, "Done");
            aTimer.Stop();
            aTimer.Start();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (worker.WorkerSupportsCancellation == true)
            {
                progressTextBlock2.Text = "Cancelling...";
                worker.CancelAsync();
            }
        }

        private void AddToDb(object sender, RoutedEventArgs e)
        {
            _db.SaveChanges();
            progressTextBlock3.Text = "Database data has been updated";
        }

        public void LoadData(object sender, RoutedEventArgs e)
        {
            var result = from peoplelist in _db.People
                             select peoplelist;
            foreach (Person p in result)
            {
                people.Add(p);
            }
            loadData.IsEnabled = false;
            progressTextBlock3.Text = "Database data has been loaded";
        }

        ObservableCollection<Person> people = new ObservableCollection<Person>();
        Person person = new Person();
        String personName;
        byte[] personPicture;

        public ObservableCollection<Person> People
        {
            get { return this.people; }
            set
            {
                this.people = value;
                this.OnPropertyChanged("People");
            }
        }
        public Person Person
        {
            get { return this.person; }
            set
            {
                this.person = value;
                this.OnPropertyChanged("Person");
                if (Person != null)
                {
                    PersonPicture = Person.Picture;
                    PersonName = Person.Name;
                }
            }
        }
        public String PersonName
        {
            get { return this.personName; }
            set
            {
                this.personName = value;
                this.OnPropertyChanged("PersonName");
            }
        }
        public byte[] PersonPicture
        {
            get { return this.personPicture; }
            set
            {
                this.personPicture = value;
                this.OnPropertyChanged("PersonPicture");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName)
        {
            PropertyChangedEventHandler eh = this.PropertyChanged;
            if (null != eh)
            {
                eh(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}