namespace Store.UI.Wrapper
{
    using System;
    using System.Runtime.CompilerServices;
    using Store.Model;
    using Store.UI.ViewModel;

    public class HumanWrapper : ViewModelBase
    {

        private Human human;

        private bool _isChanged;

        public HumanWrapper(Human human)
        {
            this.human = human;
        }

        public Human Model { get { return this.human; } }

        public bool IsChanged
        {
            get
            {
                return this._isChanged;
            }
            private set
            {
                this._isChanged = value;
                OnPropertyChanged();
            }
        }

        public void AcceptChanges()
        {
            IsChanged = false;
        }

        public int Id { get { return this.human.Id; } }

        public string FirstName
        {
            get { return this.human.FirstName; }
            set
            {
                this.human.FirstName = value;
                OnPropertyChanged();
            }
        }
        public string LastName
        {
            get { return this.human.LastName; }
            set
            {
                this.human.LastName = value;
                OnPropertyChanged();
            }
        }

        public DateTime? Birthday
        {
            get { return this.human.Birthday; }
            set
            {
                this.human.Birthday = value;
                OnPropertyChanged();
            }
        }

        public bool IsDeveloper
        {
            get { return this.human.IsFriend; }
            set
            {
                this.human.IsFriend = value;
                OnPropertyChanged();
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName != nameof(IsChanged))
            {
                IsChanged = true;
            }
        }
    }
}