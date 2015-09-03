namespace XamlBrewer.Uwp.SqLiteSample.ViewModels
{
    using Mvvm;
    using System;
    using System.Windows.Input;
    using Windows.Storage;
    using Windows.Storage.Pickers;
    using Windows.UI.Xaml.Media;
    using Models;

    internal class PersonViewModel : ViewModelBase
    {
        private Person model;
        private ImageSource picture = null;
        private DelegateCommand uploadImageCommand;

        public PersonViewModel(Person model)
        {
            this.model = model;
            this.uploadImageCommand = new DelegateCommand(this.UploadImage_Executed);
        }

        public string Description
        {
            get
            {
                if (this.model == null)
                {
                    return string.Empty;
                }

                return this.model.Description;
            }

            set
            {
                if (this.model != null)
                {
                    this.model.Description = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public int Id
        {
            get
            {
                if (this.model == null)
                {
                    return 0;
                }

                return this.model.Id;
            }

            set
            {
                if (this.model != null)
                {
                    this.model.Id = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public Person Model
        {
            get
            {
                return this.model;
            }

            set
            {
                this.SetProperty(ref this.model, value);
                this.picture = null;
                this.OnPropertyChanged(string.Empty);
            }
        }
        public string Name
        {
            get
            {
                if (this.model == null)
                {
                    return string.Empty;
                }

                return this.model.Name;
            }

            set
            {
                if (this.model != null)
                {
                    this.model.Name = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public ImageSource ImageSource
        {
            get
            {
                if (this.model != null && this.picture == null)
                {
                    this.picture = this.model.Picture.AsBitmapImage();
                }

                return this.picture;
            }
        }

        public byte[] Picture
        {
            set
            {
                this.picture = null;
                this.model.Picture = value;
                this.OnPropertyChanged("ImageSource");
            }
        }

        public ICommand UploadImageCommand
        {
            get { return this.uploadImageCommand; }
        }

        private async void UploadImage_Executed()
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".bmp");
            openPicker.FileTypeFilter.Add(".png");
            StorageFile imgFile = await openPicker.PickSingleFileAsync();
            if (imgFile != null)
            {
                this.Picture = await imgFile.AsByteArray();
            }
        }
    }
}